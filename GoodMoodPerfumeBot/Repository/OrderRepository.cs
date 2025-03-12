using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.Repositiory;

namespace GoodMoodPerfumeBot.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDatabaseContext context;
        public OrderRepository(AppDatabaseContext context)
        {
            this.context = context;
        }
        public async Task<Order> CreateOrderAsync(Order order)
        {
            var orderEntity = await this.context.Orders.AddAsync(order);
            return orderEntity.Entity;
        }

        public Task<List<Order>> GetAllUserOrders(long telegramUserId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetNotPayedOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetNotShippedOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllNotPayedOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveOrderAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
