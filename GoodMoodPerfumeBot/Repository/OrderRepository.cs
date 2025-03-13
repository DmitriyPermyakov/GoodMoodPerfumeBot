using GoodMoodPerfumeBot.DTOs;
using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.Repositiory;
using GoodMoodPerfumeBot.UserRoles;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Order>> GetAllUserOrders(long telegramUserId)
        {
            var userOrders = await this.context.Orders
                .Where(o => o.AppUser.TelegramUserId == telegramUserId)
                .ToListAsync();

            return userOrders;
        }

        public async Task<List<Order>> GetNotPayedOrdersAsync()
        {
            var userNotPayedOrders = await this.context.Orders
                .Where(o => o.PayStatus.Equals(SharedData.PayStatus.NotPayed))
                .ToListAsync();

            return userNotPayedOrders;
        }

        public async Task<List<Order>> GetNotShippedOrdersAsync()
        {
            var userNotShippedOrders = await this.context.Orders
                .Where(o => o.OrderStatus.Equals(SharedData.OrderStatus.NotShipped))
                .ToListAsync();

            return userNotShippedOrders;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await this.context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == id);

            return order;
        }

        public async Task<int> RemoveAllNotPayedOrdersAsync()
        {
            int count = await this.context.Orders
                .Where(o => o.PayStatus.Equals(SharedData.PayStatus.NotPayed))
                .ExecuteDeleteAsync();

            return count;
        }

        public async Task<int> RemoveOrderAsync(int id)
        {
            int count = await this.context.Orders.Where(o => o.OrderId == id).ExecuteDeleteAsync();
            return count;
        }

        public async Task UpdateOrderAsync(Order updatedOrder)
        {
            this.context.Orders.Update(updatedOrder);
            await Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }
}
