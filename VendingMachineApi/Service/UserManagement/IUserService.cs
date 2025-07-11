using FlapKapBackendChallenge.Dto;

namespace FlapKap.Service.UserManagement
{
    public interface IUserService
    {
        Task<BaseResponse> GetUser();
        Task<BaseResponse> GetUser(int id);
    }
}
