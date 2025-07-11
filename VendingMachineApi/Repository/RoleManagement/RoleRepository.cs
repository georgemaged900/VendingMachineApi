using FlapKap.Models;
using FlapKapBackendChallenge;
using FlapKapBackendChallenge.Dto;
using FlapKapBackendChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace FlapKap.Repository.RoleManagement
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddUserRole(UserRole userRole)
        {
            await _context.UserRoles.AddAsync(userRole);
            return await _context.SaveChangesAsync();
        }
        public async Task<Role> GetRole(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.Name == roleName.ToLower());
        }
        public async Task<List<Role>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }
    }
}
