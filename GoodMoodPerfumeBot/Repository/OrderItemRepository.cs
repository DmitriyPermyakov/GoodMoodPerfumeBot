using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.Repositiory;
using Microsoft.EntityFrameworkCore;

namespace GoodMoodPerfumeBot.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDatabaseContext context;
        public OrderItemRepository(AppDatabaseContext context)
        {
            this.context = context;
        }
        public async Task CreateAsync(List<OrderItem> items)
        {
            await this.context.OrderItems.AddRangeAsync(items);            
        }

        public async Task DeleteAsync(int id)
        {
            await this.context.OrderItems
                .Where(o => o.OrderItemId == id)
                .ExecuteDeleteAsync();
        }

        public async Task<List<OrderItem>> GetAllAsync()
        {
            var orderItems = await this.context.OrderItems.ToListAsync();
            return orderItems;
        }

        public async Task<OrderItem> GetByIdAsync(int id)
        {
            var orderItem = await this.context.OrderItems.FirstOrDefaultAsync(o => o.OrderItemId == id);
            return orderItem;
        }

        public async Task<OrderItem> UpdateAsync(OrderItem item)
        {
            var updatedItem = this.context.OrderItems.Update(item);
            return await Task.FromResult(updatedItem.Entity);
        }

        public async Task SaveAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }
}
