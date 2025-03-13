using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.Repository
{
    public interface IOrderItemRepository
    {
        public Task CreateAsync(List<OrderItem> items);
        public Task<OrderItem> UpdateAsync(OrderItem item);
        public Task<OrderItem> GetByIdAsync(int id);
        public Task<List<OrderItem>> GetAllAsync();
        public Task DeleteAsync(int id);
        public Task SaveAsync();
    }
}
