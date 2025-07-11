using FlapKapBackendChallenge.Dto;
using FlapKapBackendChallenge.Models;

namespace FlapKap.Repository.Authentication
{
    public interface IAuthRepository
    {
        Task<bool> CheckUserExists(string userName);
        Task<int> AddUser(User user);
      }
}
