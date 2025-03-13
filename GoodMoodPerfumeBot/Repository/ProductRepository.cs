using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.Repositiory;
using Microsoft.EntityFrameworkCore;

namespace GoodMoodPerfumeBot.Repository
{
    public class ProductRepository : IProductRepository
    {
        private AppDatabaseContext context;
        public ProductRepository(AppDatabaseContext context)
        {
            this.context = context;
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            var entity =  await this.context.Products.AddAsync(product);
            await this.context.SaveChangesAsync();

            return entity.Entity;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await this.context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await this.context.Products                    
                    .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<Product> RemoveProductAsync(int id)
        {
            Product productToDelete = await this.context.Products.FindAsync(id);
            if (productToDelete == null)
            {
                throw new Exception("Product not found");
            }
            
            var entity = this.context.Products.Remove(productToDelete);
            await this.context.SaveChangesAsync();
            return entity.Entity;            
        }

        public async Task<Product> UpdateProductAsync(Product updatedProduct)
        {
            var productEnity = this.context.Products.Update(updatedProduct);
            await this.context.SaveChangesAsync();

            return productEnity.Entity;
        }
    }
}
