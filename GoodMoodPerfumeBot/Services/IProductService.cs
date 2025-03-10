using GoodMoodPerfumeBot.DTOs;
using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.Services
{
    public interface IProductService
    {
        public Task<List<Product>> GetAllProductsAsync();
        public Task<Product> GetProductByIdAsync(int id);
        public Product UpdateProduct(Product updatedProduct);
        public Task<Product> CreateProductAsync(CreateProductDTO product, string[] imageIds);
        public Task RemoveProductAsync(int id);
    }
}
