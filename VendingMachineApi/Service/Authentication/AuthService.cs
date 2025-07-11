using AutoMapper;
using FlapKap;
using FlapKap.Middleware;
using FlapKap.Models;
using FlapKap.Repository.Authentication;
using FlapKap.Repository.RoleManagement;
using FlapKap.Repository.UserManagement;
using FlapKapBackendChallenge.Dto;
using FlapKapBackendChallenge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlapKapBackendChallenge.Service.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;

        public AuthService(IAuthRepository authRepository, IPasswordHasher<User> passwordHasher, IUserRepository userRepository, IMapper mapper, IRoleRepository roleRepository)
        {
            _authRepository = authRepository;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }
        public async Task<BaseResponse> Login(LoginRequestDto request)
        {
            var user = await _userRepository.GetUser(request.userName);
            if (user == null) throw new NotFoundException("User not Found");

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.password);
            if (result == PasswordVerificationResult.Failed)
                return new BaseResponse { Success = false, Message = "Invalid credentials" };

            var token = await GenerateToken(user);

            var roleName = user.UserRoles.FirstOrDefault()?.Role?.Name ?? "unknown";


            LoginResponseDto loginResponse = new LoginResponseDto()
            {
                Username = request.userName,
                Id = user.Id,
                Token = token,
                Role = roleName
            };
            return new BaseResponse { Data = loginResponse };
        }
        private async Task<string> GenerateToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.UserRoles.FirstOrDefault().Role.Name.ToString()),
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("3409fdusfj8j82091JS8D021MDM@I3RJ$JMIL[P-012I3U9045UJdskopk340j09jBNO8560FD91dsjsLPD-02349F9Jsjdhu328u4ubnl91238RHVNKALA01923ncz920v,b*"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: "FlapKapSystem",
            audience: "FlapKap",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("3409fdusfj8j82091JS8D021MDM@I3RJ$JMIL[P-012I3U9045UJdskopk340j09jBNO8560FD91dsjsLPD-02349F9Jsjdhu328u4ubnl91238RHVNKALA01923ncz920v,b*")),
            SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<BaseResponse> Register(RegisterRequestDto register)
        {
            if (await _authRepository.CheckUserExists(register.userName))
                throw new BadRequestException("Username already exists");

            if (register.role != Enums.Roles.buyer.ToString() && register.role != Enums.Roles.seller.ToString())
                throw new BadRequestException("Invalid Role Name");

            var role = await _roleRepository.GetRole(register.role);
            if (role == null)
                throw new BadRequestException("Invalid Role Name");

            var user = new User()
            {
                UserName = register.userName,
                Password = _passwordHasher.HashPassword(null, register.password),
                UserRoles = new List<UserRole>
                {
                  new UserRole { roleId = role.Id }
                }
            };

            int result = await _authRepository.AddUser(user);

            var registerResponse = _mapper.Map<RegisterResponseDto>(user);
            registerResponse.Role = role.Name;

            if (result > 0)
                return new BaseResponse() { Data = registerResponse };
            return new BaseResponse() { Success = false, Message = "Error Inserting User" };
        }
    }
}
