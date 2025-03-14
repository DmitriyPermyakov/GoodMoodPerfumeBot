using GoodMoodPerfumeBot.DTOs;
using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.Repository;

namespace GoodMoodPerfumeBot.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository repository;
        public OrderItemService(IOrderItemRepository repository)
        {
            this.repository = repository;
        }
        public async Task CreateAsync(Order order, List<CreateOrderItemDTO> createOrderItemDTO)
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in createOrderItemDTO)
            {
                orderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
                
            }            

            await this.repository.CreateAsync(orderItems);            
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderItem>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderItem> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderItem> UpdateAsync(int id, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
