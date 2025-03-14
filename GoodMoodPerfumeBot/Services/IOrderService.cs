﻿using GoodMoodPerfumeBot.DTOs;
using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.Services
{
    public interface IOrderService
    {
        public Task<Order> CreateOrderAsync(CreateOrderDTO createOrderDTO);
        public Task<Order> GetOrderByIdAsync(int id);
        public Task<List<Order>> GetAllUserOrdersAsync(long telegramUserId);
        public Task<List<Order>> GetNotPayedOrdersAsync();
        public Task<List<Order>> GetNotShippedOrdersAsync();
        public Task<int> RemoveAllNotPayedOrdersAsync();
        public Task<int> RemoveOrderAsync(int id);
        public Task UpdateOrderAsync(UpdateOrderDTO updatedOrder);
    }
}
