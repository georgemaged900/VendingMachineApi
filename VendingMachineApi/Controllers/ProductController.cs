using FlapKap.Service.ProductService;
using FlapKapBackendChallenge.Dto;
using FlapKapBackendChallenge.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlapKapBackendChallenge.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet("/product")] // NO Auth
        public async Task <IActionResult>GetProduct()
        {
            return Ok(await _productService.GetProduct());
        }

        [HttpGet("/product/{Id}")] // NO Auth
        public async Task<IActionResult> GetProduct(int Id)
        {
            return Ok(await _productService.GetProduct(Id));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "seller")]
        [HttpPost("/product")] 
        public async Task<IActionResult> AddProduct([FromBody]CreateProductRequestDto productRequest)
        {
            var user = GetCurrentUser();
            return Ok(await _productService.AddProduct(productRequest,user));  
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "seller")]
        [HttpPut("/product{Id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequestDto productRequest,int Id)
        {
            var user = GetCurrentUser();
            return Ok(await _productService.UpdateProduct(Id,productRequest,user));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "seller")]
        [HttpDelete("/product{Id}")]
        public async Task<IActionResult> RemoveProduct(int Id)
        {
            var user = GetCurrentUser();
            return Ok(await _productService.DeleteProduct(Id,user));
        }

    }
}
