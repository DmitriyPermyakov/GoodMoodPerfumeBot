using GoodMoodPerfumeBot.DTOs;
using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.Services
{
    public interface IProductService
    {
        public Task<List<Product>> GetAllProductsAsync();
        public Task<Product> GetProductByIdAsync(int id);
        public Task<Product> UpdateProductAsync(UpdateProductDTO updatedProduct);
        public Task<Product> CreateProductAsync(CreateProductDTO product);
        public Task RemoveProductAsync(int id);
    }
}
