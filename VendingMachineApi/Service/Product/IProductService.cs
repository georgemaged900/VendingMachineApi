using FlapKap.Dto;
using FlapKapBackendChallenge.Dto;
using FlapKapBackendChallenge.Models;

namespace FlapKap.Service.ProductService
{
    public interface IProductService
    {
        Task<BaseResponse> GetProduct();
        Task<BaseResponse> GetProduct(int Id);
        Task<BaseResponse> AddProduct(CreateProductRequestDto createProductRequestDto,UserDetails currentUser);
        Task<BaseResponse> UpdateProduct(int Id,UpdateProductRequestDto productDto, UserDetails currentUser);
        Task<BaseResponse> DeleteProduct(int Id, UserDetails currentUser);
        Task<BaseResponse> BuyProduct(BuyRequestDto request, UserDetails currentUser);
        Task<BaseResponse> ResetDeposit(int userId);
    }
}
