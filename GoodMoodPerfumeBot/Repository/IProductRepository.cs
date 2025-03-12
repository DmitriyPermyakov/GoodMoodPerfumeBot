using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.Repository
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAllProductsAsync();
        public Task<Product> GetProductByIdAsync(int id);
        public Task<Product> UpdateProductAsync(Product updatedProduct);
        public Task<Product> CreateProductAsync(Product product);
        public Task<Product> RemoveProductAsync(int id);
    }
}
