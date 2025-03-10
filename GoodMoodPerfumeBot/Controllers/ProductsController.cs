using GoodMoodPerfumeBot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GoodMoodPerfumeBot.ServerResponse;
using System.Net;
using GoodMoodPerfumeBot.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IUploadImageService uploadImageService;
        public ProductsController(IProductService productService, IUploadImageService uploadImageService)
        {
            this.productService = productService;
            this.uploadImageService = uploadImageService;
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
                return Ok(new Response()
                {
                    Status = HttpStatusCode.OK,
                    Result = await this.productService.GetProductByIdAsync(id)
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

        [HttpPost("createProduct")]
        public async Task<IActionResult> CreateProduct([FromForm]CreateProductDTO productDTO)
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
                        Errors = errors
                    });
                }

                string[] imageIds = this.uploadImageService.UploadImage("files");
                Product createdProduct = await this.productService.CreateProductAsync(productDTO, imageIds);

                Response response = new Response()
                {
                    Status = HttpStatusCode.Created,
                    Result = createdProduct
                };

                Console.WriteLine($"************************* {createdProduct.ProductId}");

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
                        "cant create product"
                    }
                });
            }
        }
    }
}
