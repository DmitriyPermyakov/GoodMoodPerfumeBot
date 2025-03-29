using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.Repository
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetByCategoryAsync(string category);
        public Task<List<Product>> GetProductByNameAsync(string name);
        public Task<Product> GetProductByIdAsync(int id);
        public Task<Product> UpdateProductAsync(Product updatedProduct);
        public Task<Product> CreateProductAsync(Product product);
        public Task<Product> RemoveProductAsync(int id);
        public Task SaveAsync();
    }
}
