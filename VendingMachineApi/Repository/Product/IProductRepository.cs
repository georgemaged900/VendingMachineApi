using FlapKapBackendChallenge.Dto;
using FlapKapBackendChallenge.Models;

namespace FlapKap.Repository.ProductRepo
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProduct();
        Task<Product> GetProduct(int Id);
        Task<int> AddProduct(Product product);
        Task<int> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int Id);
    }
}
