using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.Repository
{
    public interface IOrderRepository
    {
        public Task<Order> CreateOrderAsync(Order order);
        public Task<Order> GetOrderByIdAsync(int id);
        public Task<List<Order>> GetAllUserOrders(long telegramUserId);
        public Task<List<Order>> GetNotShippedOrdersAsync();
        public Task<List<Order>> GetNotPayedOrdersAsync();
        public Task RemoveAllNotPayedOrdersAsync();
        public Task RemoveOrderAsync(int id);
    }
}
