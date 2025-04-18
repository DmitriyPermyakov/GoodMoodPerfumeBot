﻿using GoodMoodPerfumeBot.DTOs;
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

        public async Task<List<Order>> GetAllUserOrdersAsync(long telegramUserId)
        {
            var userOrders = await this.context.Orders
                .Where(o => o.AppUser.TelegramUserId == telegramUserId)
                .Include(o => o.AppUser)
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .ToListAsync();

            return userOrders;
        }

        public async Task<Order> GetOrderByUserIdAsync(long telegramUserId)
        {
            var order = await this.context.Orders
                .Where(o => o.AppUser.TelegramUserId == telegramUserId)
                .Include(o => o.AppUser)
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .OrderBy(o => o.Id)
                .ToListAsync();
            return order.Last();
        }


        public async Task<List<Order>> GetNotPayedOrdersAsync()
        {
            var userNotPayedOrders = await this.context.Orders
                .Where(o => o.PayStatus.Equals(SharedData.PayStatus.NotPayed))
                .Include(o => o.AppUser)
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .ToListAsync();

            return userNotPayedOrders;
        }

        public async Task<List<Order>> GetNotShippedOrdersAsync()
        {
            var userNotShippedOrders = await this.context.Orders
                .Where(o => o.OrderStatus.Equals(SharedData.OrderStatus.NotShipped))
                .Include(o => o.AppUser)
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .ToListAsync();

            return userNotShippedOrders;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await this.context.Orders
                .Where(o => o.Id == id)
                .Include(o => o.AppUser)
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync();

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
            int count = await this.context.Orders.Where(o => o.Id == id).ExecuteDeleteAsync();
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
