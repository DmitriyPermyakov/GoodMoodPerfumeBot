using GoodMoodPerfumeBot.DTOs;
using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GoodMoodPerfumeBot.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository repository;
        private readonly IImageService imageService;
        public ProductService(IProductRepository productRepository, IImageService imageService)
        {
            this.repository = productRepository;
            this.imageService = imageService;
        }

        public async Task<Product> CreateProductAsync(CreateProductDTO productDTO)
        {
            string imagesUrl = string.Empty;
            if (productDTO.Image != null && productDTO.Image.Length > 0)
               imagesUrl = await this.imageService.UploadImageAsync(productDTO?.Image, productDTO?.Name);

            Product createdProduct = new Product()
            {
                ProductName = productDTO.Name,
                ProductDescription = productDTO.Description,
                ProductPrice = productDTO.Price,
                ProductImageUrl = imagesUrl
            };

            Product productFromDb =  await this.repository.CreateProductAsync(createdProduct);
            await this.repository.SaveAsync();
            return productFromDb;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await repository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            Product product = await this.repository.GetProductByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found");
            return product;
        }

        public async Task RemoveProductAsync(int id)
        {
            Product deletedProduct = await this.repository.RemoveProductAsync(id);
            this.imageService.DeleteImages(deletedProduct?.ProductImageUrl);
            this.imageService.DeleteFolder(deletedProduct.ProductName);
        }

        public async Task<Product> UpdateProductAsync(UpdateProductDTO updatedProductDto)
        {
            if(!string.IsNullOrEmpty(updatedProductDto.OldImagesUrl) && updatedProductDto.ProductImageFile != null)
            {
                throw new Exception("Только одно изображение на один продукт");
            } else if(string.IsNullOrEmpty(updatedProductDto.OldImagesUrl) && updatedProductDto.ProductImageFile == null)
            {
                throw new Exception("По крайней мере одно изображение должно быть загружено");
            }

            Product productToUpdate = await this.GetProductByIdAsync(updatedProductDto.Id);

            if (productToUpdate == null)
                throw new Exception("Product not found");          

            // если страрая картинка удалена, то удаляем файл картинки
            if(string.IsNullOrEmpty(updatedProductDto.OldImagesUrl))
                this.imageService.DeleteImages(productToUpdate.ProductImageUrl);


            if (!updatedProductDto.Name.Equals(productToUpdate.ProductName))
                this.imageService.RenameProductImageFolder(productToUpdate.ProductName, updatedProductDto.Name);

            string image = string.Empty;
            //если передан файл для новой картинки, то загружаем его
            if(updatedProductDto.ProductImageFile != null && updatedProductDto.ProductImageFile.Length > 0)
               image = await this.imageService.UploadImageAsync(updatedProductDto.ProductImageFile, updatedProductDto.Name);

            
            productToUpdate.ProductName = updatedProductDto.Name;
            productToUpdate.ProductDescription = updatedProductDto.Description;
            productToUpdate.ProductPrice = updatedProductDto.Price;
            productToUpdate.ProductImageUrl = image;

            var updatedProductFromDb =  await this.repository.UpdateProductAsync(productToUpdate);
            await this.repository.SaveAsync();

            return updatedProductFromDb;
        }
    }
}
