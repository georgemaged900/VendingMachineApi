using FlapKapBackendChallenge;
using FlapKapBackendChallenge.Dto;
using FlapKapBackendChallenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace FlapKap.Repository.Authentication
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckUserExists(string userName)
        {
            return await _context.Users.AnyAsync(u => u.UserName == userName);
        }
        public async Task<int> AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync();
        }
    }
}
