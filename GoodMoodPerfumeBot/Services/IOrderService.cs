using GoodMoodPerfumeBot.DTOs;
using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.Services
{
    public interface IOrderService
    {
        public Task<Order> CreateOrderAsync(CreateOrderDTO createOrderDTO);
    }
}
