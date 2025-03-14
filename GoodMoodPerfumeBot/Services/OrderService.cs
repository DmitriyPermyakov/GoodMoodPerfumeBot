using GoodMoodPerfumeBot.DTOs;
using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.Repository;
using GoodMoodPerfumeBot.UserRoles;

namespace GoodMoodPerfumeBot.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository repository;
        public OrderService(IOrderRepository orderRepository)
        {
            this.repository = orderRepository;

        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            Order order = await this.repository.GetOrderByIdAsync(id);
            return order;
        }

        public async Task<List<Order>> GetAllUserOrdersAsync(long telegramUserId)
        {
            List<Order> userOrders = await this.repository.GetAllUserOrders(telegramUserId);
            return userOrders;
        }
        public async Task<List<Order>> GetNotPayedOrdersAsync()
        {
            List<Order> notPayedOrders = await this.repository.GetNotPayedOrdersAsync();
            return notPayedOrders;
        }
        public async Task<List<Order>> GetNotShippedOrdersAsync()
        {
            List<Order> notShippedOrders = await this.repository.GetNotShippedOrdersAsync();
            return notShippedOrders;
        }

        public async Task<int> RemoveAllNotPayedOrdersAsync()
        {
            int countOfRemoved = await this.repository.RemoveAllNotPayedOrdersAsync();
            return countOfRemoved;
        }
        public async Task<int> RemoveOrderAsync(int id)
        {
            int countOfRemoved = await this.repository.RemoveOrderAsync(id);
            return countOfRemoved;
        }
        public async Task<Order> CreateOrderAsync(CreateOrderDTO createOrderDTO)
        {
            AppUser user = new AppUser()
            {
                TelegramUserId = createOrderDTO.TelegramUserId,
                ChatId = null,
                UserRole = SharedData.UserRoles.Member
            };

            List<OrderItem> orderItems = new List<OrderItem>();
            foreach(var item in createOrderDTO.OrderItems)
            {
                orderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            Order order = new Order()
            {
                OrderId = 0,
                AppUser = user,                
                OrderStatus = SharedData.OrderStatus.NotShipped,
                PayStatus = SharedData.PayStatus.NotPayed,  
                OrderItems = orderItems,
                Address = createOrderDTO.Address
            };
            var orderToCreate = await this.repository.CreateOrderAsync(order);

            await this.repository.SaveAsync();

            var createdOrder = await this.repository.GetOrderByIdAsync(orderToCreate.OrderId);            

            return createdOrder;
        }

        public async Task UpdateOrderAsync(UpdateOrderDTO updatedOrder)
        {
            var orderToUpdate = await this.repository.GetOrderByIdAsync(updatedOrder.OrderId);
            if (orderToUpdate == null)
                throw new Exception("Order not found");

            if (updatedOrder.OrderItems != null && updatedOrder.OrderItems.Count > 0)
            {
                var equalOrders = orderToUpdate.OrderItems.IntersectBy(updatedOrder.OrderItems
                    .Select(i => i.OrderItemId), u => u.OrderItemId).ToList();

                if (equalOrders.Count() > 0)
                    foreach (var item in equalOrders)
                    {
                        orderToUpdate.OrderItems
                            .FirstOrDefault(o => o.OrderItemId == item.OrderItemId)
                            .Quantity = updatedOrder.OrderItems
                            .FirstOrDefault(o => o.OrderItemId == item.OrderItemId).Quantity;

                    }


                var orderItemsToDelete = orderToUpdate.OrderItems.ExceptBy(updatedOrder.OrderItems
                    .Select(i => i.OrderItemId), u => u.OrderItemId).ToList();
                foreach (var item in orderItemsToDelete)                
                    orderToUpdate.OrderItems.RemoveAll(i => i.OrderItemId == item.OrderItemId);
                

                var orderItemsToAdd = updatedOrder.OrderItems.ExceptBy(orderToUpdate.OrderItems
                    .Select(i => i.OrderItemId), u => u.OrderItemId).ToList();
                if(orderItemsToAdd.Count() > 0)
                    orderToUpdate.OrderItems.AddRange(orderItemsToAdd);
            }

            if (!string.IsNullOrEmpty(updatedOrder.Address))
                orderToUpdate.Address = updatedOrder.Address;

            await this.repository.UpdateOrderAsync(orderToUpdate);
            await this.repository.SaveAsync();
            
        }
    }
}
