using FlapKap.Models;
using FlapKapBackendChallenge.Dto;
using FlapKapBackendChallenge.Models;

namespace FlapKap.Repository.RoleManagement
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetRoles();
        Task<Role> GetRole(string roleName);
        Task<int> AddUserRole(UserRole userRole);

    }
}
