using FlapKap.Service.UserManagement;
using FlapKapBackendChallenge.Controllers;
using FlapKapBackendChallenge.Dto;
using FlapKapBackendChallenge.Service.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace FlapKap.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Register USER
        [HttpPost("/register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            return Ok(await _authService.Register(registerRequestDto));
        }

        //Login USER
        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            return Ok(await _authService.Login(request));
        }
    }
}
