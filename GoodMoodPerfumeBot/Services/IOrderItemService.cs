using GoodMoodPerfumeBot.DTOs;
using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.Services
{
    public interface IOrderItemService
    {
        public Task CreateAsync(Order order, List<CreateOrderItemDTO> items);
        public Task<OrderItem> UpdateAsync(int id, int quantity);
        public Task<OrderItem> GetByIdAsync(int id);
        public Task<List<OrderItem>> GetAllAsync();
        public Task DeleteAsync(int id);
    }
}
