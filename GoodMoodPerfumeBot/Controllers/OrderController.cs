using GoodMoodPerfumeBot.DTOs;
using GoodMoodPerfumeBot.Models;
using GoodMoodPerfumeBot.ServerResponse;
using GoodMoodPerfumeBot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GoodMoodPerfumeBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (id < 0)
                    return BadRequest(new Response()
                    {
                        Status = HttpStatusCode.BadRequest,
                        IsSuccessful = false,
                        Errors =  new List<string>()
                        {
                            "Order not found"
                        }
                    });

                Order order = await this.orderService.GetOrderByIdAsync(id);
                return Ok(new Response()
                {
                    Status = HttpStatusCode.OK,
                    Result = order
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new Response()
                {
                    Status = HttpStatusCode.BadRequest,
                    IsSuccessful = false,
                    Errors = new List<string>()
                    {
                            "Order not found"
                    }
                });
            }
        }
        [HttpGet("getAllUserOrders/{id}")]
        public async Task<IActionResult> GetAllUserOrders(long id)
        {
            try
            {
                if (id < 0)
                    return BadRequest(new Response()
                    {
                        Status = HttpStatusCode.BadRequest,
                        IsSuccessful = false,
                        Errors = new List<string>()
                        {
                            "Bad user id"
                        }
                    });

                List<Order> userOrders = await this.orderService.GetAllUserOrdersAsync(id);

                return Ok(new Response()
                {
                    Status = HttpStatusCode.OK,
                    Result = userOrders
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new Response()
                {
                    Status = HttpStatusCode.BadRequest,
                    IsSuccessful = false,
                    Errors = new List<string>()
                    {
                        ex.Message
                    }
                });
            }
        }

        

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateOrderDTO createOrderDTO)
        {
            try
            {
                if(!ModelState.IsValid || createOrderDTO == null)
                {
                    var modelErrors = ModelState.Values.SelectMany(v => v.Errors);

                    List<string> errors = new List<string>();
                    foreach (var e in modelErrors)
                        errors.Add(e.ErrorMessage);

                    return BadRequest(new Response()
                    {
                        Status = System.Net.HttpStatusCode.BadRequest,
                        IsSuccessful = false,
                        Errors = errors,
                        Result = createOrderDTO ?? new Object()
                    });                    
                }

                var order = await this.orderService.CreateOrderAsync(createOrderDTO);

                Response response = new Response()
                {
                    Status = System.Net.HttpStatusCode.OK,
                    Result = order
                };

                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(new Response()
                {
                    Status = System.Net.HttpStatusCode.BadRequest,
                    IsSuccessful = false,
                    Errors = new List<string>()
                    {
                        ex.Message
                    }                    
                });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateOrderDTO updateOrderDTO)
        {
            try
            {
                if (!ModelState.IsValid || updateOrderDTO == null)
                    return BadRequest(new Response()
                    {
                        Status = HttpStatusCode.BadRequest,
                        IsSuccessful = false,
                        Errors = new List<string>()
                        {
                            "Cant update order",

                        },
                        Result = updateOrderDTO
                    });

                await this.orderService.UpdateOrderAsync(updateOrderDTO);

                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(new Response()
                {
                    Status = HttpStatusCode.BadRequest,
                    IsSuccessful = false,
                    Errors = new List<string>()
                    {
                        ex.Message
                    }
                });
            }
        }

    }
}
