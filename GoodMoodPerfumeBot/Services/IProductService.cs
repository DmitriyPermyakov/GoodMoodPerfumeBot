using GoodMoodPerfumeBot.DTOs;
using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.Services
{
    public interface IProductService
    {
        public Task<List<Product>> GetByCategoryAsync(string category);
        public Task<List<Product>> GetProductByNameAsync(string name);
        public Task<Product> GetProductByIdAsync(int id);
        public Task<Product> UpdateProductAsync(UpdateProductDTO updatedProduct);
        public Task<Product> CreateProductAsync(CreateProductDTO product);
        public Task RemoveProductAsync(int id);
    }
}
