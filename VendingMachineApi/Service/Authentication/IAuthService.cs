using FlapKapBackendChallenge.Dto;
using Microsoft.AspNetCore.Identity.Data;

namespace FlapKapBackendChallenge.Service.Authentication
{
    public interface IAuthService
    {
        Task<BaseResponse> Login(LoginRequestDto request);
        Task<BaseResponse> Register(RegisterRequestDto register);
    }
}
