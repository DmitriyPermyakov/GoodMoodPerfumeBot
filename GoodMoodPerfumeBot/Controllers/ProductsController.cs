using GoodMoodPerfumeBot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GoodMoodPerfumeBot.ServerResponse;
using System.Net;
using GoodMoodPerfumeBot.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using GoodMoodPerfumeBot.Models;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GoodMoodPerfumeBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(new Response()
                {
                    Status = HttpStatusCode.OK,
                    Result = await this.productService.GetAllProductsAsync()
                });
            } 
            catch(Exception ex)
            {
                return BadRequest(new Response()
                {
                    Status = HttpStatusCode.BadRequest,
                    Errors = new List<string>()
                    {
                        "Content not exists or not found",
                        $"{ex.Message}"
                    }
                });
            }
        }

        [HttpGet("getById/{id}", Name = nameof(GetById))]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (id < 1)
                    return BadRequest(new Response()
                    {
                        Status = HttpStatusCode.BadRequest,
                        IsSuccessful = false,
                        Errors = new List<string>()
                        {
                            "Id cant be less than one"
                        }
                    });

                Product product = await this.productService.GetProductByIdAsync(id);
                return Ok(new Response()
                {
                    Status = HttpStatusCode.OK,
                    Result = product
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
                        $"{ex.Message}"
                    }
                });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm]CreateProductDTO productDTO)
        {
            try
            {
                if(!ModelState.IsValid || productDTO == null)
                {
                    var modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                    List<string> errors = new List<string>();
                    foreach (var e in modelErrors)
                        errors.Add(e.ErrorMessage);

                    return BadRequest(new Response()
                    {
                        Status = HttpStatusCode.BadRequest,
                        IsSuccessful = false,
                        Errors = errors,
                        Result = productDTO ?? new object()
                    });
                }
                
                Product createdProduct = await this.productService.CreateProductAsync(productDTO);

                Response response = new Response()
                {
                    Status = HttpStatusCode.Created,
                    Result = createdProduct
                };

                foreach (var path in createdProduct.ProductImageUrl)
                    Console.WriteLine(path);
                return CreatedAtRoute(nameof(GetById), new { id = createdProduct.ProductId }, response);

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

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if(id < 0)
                {
                    return BadRequest(new Response()
                    {
                        Status = HttpStatusCode.BadRequest,
                        IsSuccessful = false,
                        Errors = new List<string>()
                        {
                            "wrong id"
                        }
                    });
                }

                await this.productService.RemoveProductAsync(id);
                return Ok(new Response()
                {
                    Status = HttpStatusCode.NoContent
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
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] UpdateProductDTO updatedProductDto)
        {
            try
            {
                if (!ModelState.IsValid || updatedProductDto == null)
                {
                    var modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                    List<string> errors = new List<string>();
                    foreach (var e in modelErrors)
                        errors.Add(e.ErrorMessage);

                    return BadRequest(new Response()
                    {
                        Status = HttpStatusCode.BadRequest,
                        IsSuccessful = false,
                        Errors = errors,
                        Result = updatedProductDto ?? new object()
                    });
                }

                Product updatedProduct = await this.productService.UpdateProductAsync(updatedProductDto);

                return Ok(new Response()
                {
                    Status = HttpStatusCode.OK,
                    Result = updatedProduct
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
    }
}
