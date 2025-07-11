using FlapKap.Dto;
using FlapKap.Service.Deposit;
using FlapKap.Service.ProductService;
using FlapKapBackendChallenge.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlapKapBackendChallenge.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DepositController : BaseController
    {
        private readonly IDepositService _depositService;
        private readonly IProductService _productService;
        public DepositController(IDepositService depositService,IProductService productService)
        {
            _depositService = depositService;
            _productService = productService;
        }

        [Authorize(Roles ="buyer")]
        [HttpPost("/deposit")] 
        public async Task<IActionResult> Deposit(int amount)
        {
            var user = GetCurrentUser();
            return Ok(await _depositService.DepositAccount(amount,user.userId));
        }
        [Authorize(Roles = "buyer")]
        [HttpPost("/buy")]
        public async Task<IActionResult> Buy([FromBody] BuyRequestDto request)
        {
            var user = GetCurrentUser();
            return Ok(await _productService.BuyProduct(request,user));
        }

        [Authorize(Roles = "buyer")]
        [HttpPost("/reset")]
        public async Task<IActionResult> ResetDeposit()
        {
            var user = GetCurrentUser();
            return Ok(await _productService.ResetDeposit(user.userId));
        }
    }
}
