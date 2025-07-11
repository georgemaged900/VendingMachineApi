using FlapKapBackendChallenge.Models;

namespace FlapKap.Repository.UserManagement
{
    public interface IUserRepository
    {
        Task<List<User>> GetUser();
        Task<User> GetUser(int id);
        Task<User> GetUser(string userName);
        Task<User> UpdateUser(User user);
    }
}
