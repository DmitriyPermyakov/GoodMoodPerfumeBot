using GoodMoodPerfumeBot.DTOs;
using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.Repository;
using GoodMoodPerfumeBot.UserRoles;

namespace GoodMoodPerfumeBot.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUserService userService;
        private readonly IOrderRepository repository;
        private readonly IOrderItemService orderItemService;
        public OrderService(IUserService userService, IOrderRepository orderRepository, IOrderItemService orderItemService)
        {
            this.userService = userService;
            this.repository = orderRepository;
            this.orderItemService = orderItemService;

        }
        public async Task<Order> CreateOrderAsync(CreateOrderDTO createOrderDTO)
        {


            AppUser user = await this.userService.CreateAsync(createOrderDTO.TelegramUserId, null, SharedData.UserRoles.Member);


            Order order = new Order()
            {
                OrderId = 0,
                AppUser = user,                
                OrderStatus = SharedData.OrderStatus.NotShipped,
                PayStatus = SharedData.PayStatus.NotPayed,                
            };
            var createdOrder = await this.repository.CreateOrderAsync(order);
            await this.orderItemService.CreateAsync(createdOrder, createOrderDTO.OrderItems);

            await this.repository.SaveAsync();


            return createdOrder;
        }
    }
}
