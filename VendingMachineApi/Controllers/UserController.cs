using FlapKap.Service.UserManagement;
using FlapKapBackendChallenge.Dto;
using FlapKapBackendChallenge.Service.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace FlapKapBackendChallenge.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET USER Details BY ID
        [HttpGet("/user/{Id}")]
        public async Task<IActionResult> GetUser(int Id)
        {
            return Ok(await _userService.GetUser(Id));
        }
        // GET USER Details
        [HttpGet("/user")]
        public async Task<IActionResult> GetUser()
        {
            return Ok(await _userService.GetUser());
        }
    }
}
