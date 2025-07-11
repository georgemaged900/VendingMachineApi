using FlapKapBackendChallenge.Dto;

namespace FlapKap.Service.Deposit
{
    public interface IDepositService
    {
        Task<BaseResponse> DepositAccount(int cent, int userId);
    }
}
