using GoodMoodPerfumeBot.DTOs;
using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GoodMoodPerfumeBot.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository repository;
        public ProductService(IProductRepository productRepository)
        {
            this.repository = productRepository;
        }

        public async Task<Product> CreateProductAsync(CreateProductDTO productDTO, List<string> imageUrls)
        {
            Product createdProduct = new Product()
            {
                ProductName = productDTO.ProductName,
                ProductDescription = productDTO.ProductDescription,
                ProductPrice = productDTO.ProductPrice,
                ProductImageUrls = imageUrls
            };
            return await this.repository.CreateProductAsync(createdProduct);
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await repository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await this.repository.GetProductByIdAsync(id);
        }

        public async Task RemoveProductAsync(int id)
        {
            await this.repository.RemoveProductAsync(id);
        }

        public  Product UpdateProduct(Product updatedProduct)
        {
            //Product productToUpdate = await this.GetProductByIdAsync(updatedProduct.ProductId);

            //if (productToUpdate == null)
            //    throw new Exception("Product not found");

            throw  new NotImplementedException();


        }
    }
}
