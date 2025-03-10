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

        public async Task<Product> CreateProductAsync(CreateProductDTO productDTO, string[] imageIds)
        {
            Product createdProduct = new Product()
            {
                ProductName = productDTO.ProductName,
                ProductDescription = productDTO.ProductDescription,
                ProductPrice = productDTO.ProductPrice,
                ProductImageIds = imageIds
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

        public void RemoveProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Product UpdateProduct(Product updatedProduct)
        {
            throw new NotImplementedException();
        }
    }
}
