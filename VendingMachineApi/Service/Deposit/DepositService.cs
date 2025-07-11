using FlapKap.Dto;
using FlapKap.Middleware;
using FlapKap.Repository.UserManagement;
using FlapKapBackendChallenge.Dto;
using Microsoft.Extensions.Logging;

namespace FlapKap.Service.Deposit
{
    public class DepositService : IDepositService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DepositService> _logger;

        public DepositService(IUserRepository userRepository, ILogger<DepositService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<BaseResponse> DepositAccount(int cent, int userId)
        {
            _logger.LogInformation("User {UserId} is attempting to deposit {Cent} cents", userId, cent);

            if (!IsValidDepositAmount(cent))
            {
                _logger.LogWarning("User {UserId} attempted invalid deposit amount: {Cent}", userId, cent);
                throw new BadRequestException("Invalid Coin. User can only deposit 5,10,20,50 and 100 cents");
            }

            var user = await _userRepository.GetUser(userId);
            if (user == null)
            {
                _logger.LogWarning("Unauthorized deposit attempt by unknown user {UserId}", userId);
                throw new ForbiddenException("User not authorized to deposit");
            }

            user.Deposit += cent;
            var result = await _userRepository.UpdateUser(user);

            _logger.LogInformation("User {UserId} successfully deposited {Cent} cents. New Balance: {Balance}", userId, cent, result.Deposit);

            return new BaseResponse()
            {
                Message = $"User Current Account Balance is {result.Deposit}"
            };
        }

        private bool IsValidDepositAmount(int cent)
        {
            List<int> validAmounts = new() { 5, 10, 20, 50, 100 };
            return validAmounts.Contains(cent);
        }
    }
}
