using FlapKap.Models;
using FlapKap.Service.RoleManagement;
using FlapKapBackendChallenge.Controllers;
using FlapKapBackendChallenge.Service.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace FlapKap.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("/role")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _roleService.GetRoles());
        }
        [HttpGet("/role/{roleName}")]
        public async Task<IActionResult> GetRole(string roleName)
        {
            return Ok(await _roleService.GetRole(roleName));
        }
        [HttpPost("/role/user")]
        public async Task<IActionResult> AddRoleToUser(UserRole userRole)
        {
            return Ok(await _roleService.AddUserRole(userRole));
        }
    }
}
