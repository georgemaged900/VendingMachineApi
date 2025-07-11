using FlapKap.Dto;
using FlapKap.Repository.UserManagement;
using FlapKapBackendChallenge.Dto;
using FlapKapBackendChallenge.Models;

namespace FlapKap.Service.UserManagement
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<BaseResponse> GetUser()
        {
            var result = await _userRepository.GetUser();

            var userDtos = result.Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Deposit = u.Deposit,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
            }).ToList();

            return new BaseResponse() { Data = userDtos };
        }
        public async Task<BaseResponse> GetUser(int id)
        {
            var result = await _userRepository.GetUser(id);
            return new BaseResponse() { Data = result };
        }
    }
}
