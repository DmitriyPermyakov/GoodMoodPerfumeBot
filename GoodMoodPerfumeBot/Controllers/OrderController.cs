using GoodMoodPerfumeBot.DTOs;
using GoodMoodPerfumeBot.ServerResponse;
using GoodMoodPerfumeBot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

    }
}
