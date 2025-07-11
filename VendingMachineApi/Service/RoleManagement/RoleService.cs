using FlapKap.Models;
using FlapKap.Repository.RoleManagement;
using FlapKapBackendChallenge.Dto;

namespace FlapKap.Service.RoleManagement
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<BaseResponse> GetRole(string roleName)
        {
            var result = await _roleRepository.GetRole(roleName);
            return new BaseResponse() { Data = result };
        }
        public async Task<BaseResponse> GetRoles()
        {
            var result = await _roleRepository.GetRoles();
            return new BaseResponse() { Data = result };
        }
        public async Task<BaseResponse> AddUserRole(UserRole userRole)
        {
            var result = await _roleRepository.AddUserRole(userRole);
            return new BaseResponse() { Data = result };
        }
    }
}
