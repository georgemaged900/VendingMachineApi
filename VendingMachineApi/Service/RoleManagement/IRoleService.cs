using FlapKap.Models;
using FlapKapBackendChallenge.Dto;

namespace FlapKap.Service.RoleManagement
{
    public interface IRoleService
    {
        Task<BaseResponse> GetRoles();
        Task<BaseResponse> GetRole(string roleName);
        Task<BaseResponse> AddUserRole(UserRole userRole);
    }
}
