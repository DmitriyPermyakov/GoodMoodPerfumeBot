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
            var imagesUrls = new List<string>();
            if (productDTO.Images != null && productDTO.Images.Length > 0)
               imagesUrls = await this.imageService.UploadImageAsync(productDTO?.Images, productDTO?.ProductName);

            Product createdProduct = new Product()
            {
                ProductName = productDTO.ProductName,
                ProductDescription = productDTO.ProductDescription,
                ProductPrice = productDTO.ProductPrice,
                ProductImageUrls = imagesUrls
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
            this.imageService.DeleteImages(deletedProduct?.ProductImageUrls);
            this.imageService.DeleteFolder(deletedProduct.ProductName);
        }

        public async Task<Product> UpdateProductAsync(UpdateProductDTO updatedProductDto)
        {
            Product productToUpdate = await this.GetProductByIdAsync(updatedProductDto.ProductId);

            if (productToUpdate == null)
                throw new Exception("Product not found");          

            
            var imagesToDelete =  productToUpdate.ProductImageUrls.Except(updatedProductDto.OldImagesUrls ?? new List<string>()).ToList();
            this.imageService.DeleteImages(imagesToDelete);

            List<string> images = new List<string>();

            if (!updatedProductDto.ProductName.Equals(productToUpdate.ProductName))
                this.imageService.RenameProductImageFolder(productToUpdate.ProductName, updatedProductDto.ProductName);

            if(updatedProductDto.ProductImageFiles != null && updatedProductDto.ProductImageFiles.Length > 0)
               images.AddRange(await this.imageService.UploadImageAsync(updatedProductDto.ProductImageFiles, updatedProductDto.ProductName));

            if(updatedProductDto.OldImagesUrls != null && updatedProductDto.OldImagesUrls.Count() > 0)
                images.AddRange(updatedProductDto.OldImagesUrls);

            productToUpdate.ProductName = updatedProductDto.ProductName;
            productToUpdate.ProductDescription = updatedProductDto.ProductDescription;
            productToUpdate.ProductPrice = updatedProductDto.ProductPrice;
            productToUpdate.ProductImageUrls = images;

            var updatedProductFromDb =  await this.repository.UpdateProductAsync(productToUpdate);
            await this.repository.SaveAsync();

            return updatedProductFromDb;
        }
    }
}
