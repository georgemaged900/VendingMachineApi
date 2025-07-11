using FlapKap.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlapKapBackendChallenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected UserDetails GetCurrentUser()
        {
            UserDetails userDetails = new UserDetails()
            {
                username = User.FindFirstValue(ClaimTypes.Name),
                userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                roleName = User.FindFirstValue(ClaimTypes.Role)
            };
            return userDetails;
        }
    }
}
