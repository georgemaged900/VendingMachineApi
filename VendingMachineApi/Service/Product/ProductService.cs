using AutoMapper;
using FlapKap.Dto;
using FlapKap.Middleware;
using FlapKap.Repository.ProductRepo;
using FlapKap.Repository.UserManagement;
using FlapKapBackendChallenge.Dto;
using FlapKapBackendChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace FlapKap.Service.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IProductRepository productRepository, IMapper mapper, IUserRepository userRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task<BaseResponse> AddProduct(CreateProductRequestDto createProductRequestDto, UserDetails currentUser)
        {
            _logger.LogInformation("Seller {UserId} is adding a new product: {ProductName}", currentUser.userId, createProductRequestDto.ProductName);

            Product product = _mapper.Map<Product>(createProductRequestDto);
            product.SellerId = currentUser.userId;

            var result = await _productRepository.AddProduct(product);

            _logger.LogInformation("Product {ProductId} added successfully by user {UserId}", result, currentUser.userId);

            return new BaseResponse() { Data = result };
        }

        public async Task<BaseResponse> DeleteProduct(int Id, UserDetails currentUser)
        {
            _logger.LogInformation("User {UserId} is attempting to delete Product {ProductId}", currentUser.userId, Id);

            var product = await _productRepository.GetProduct(Id);
            if (product == null)
            {
                _logger.LogWarning("Product {ProductId} not found", Id);
                throw new NotFoundException($"Product {Id} not Found");
            }

            if (product.SellerId != currentUser.userId)
            {
                _logger.LogWarning("User {UserId} is not authorized to delete Product {ProductId}", currentUser.userId, Id);
                throw new ForbiddenException($"You are not authorized to delete this product");
            }

            var result = await _productRepository.DeleteProduct(Id);

            _logger.LogInformation("Product {ProductId} deleted by user {UserId}", Id, currentUser.userId);

            return new BaseResponse() { Data = result };
        }
        public async Task<BaseResponse> GetProduct()
        {
            var result = await _productRepository.GetProduct();
            return new BaseResponse() { Data = result };
        }
        public async Task<BaseResponse> GetProduct(int Id)
        {
            var result = await _productRepository.GetProduct(Id);
            return new BaseResponse() { Data = result };
        }
        public async Task<BaseResponse> UpdateProduct(int Id, UpdateProductRequestDto productDto, UserDetails currentUser)
        {
            _logger.LogInformation("User {UserId} is updating Product {ProductId}", currentUser.userId, Id);

            var product = await _productRepository.GetProduct(Id);
            if (product == null)
            {
                _logger.LogWarning("Product {ProductId} not found for update", Id);
                throw new NotFoundException($"Product {Id} not Found");
            }

            if (product.SellerId != currentUser.userId)
            {
                _logger.LogWarning("User {UserId} is not authorized to update Product {ProductId}", currentUser.userId, Id);
                throw new ForbiddenException($"You are not authorized to update this product");
            }

            Product productEntity = _mapper.Map<Product>(productDto);
            productEntity.Id = Id;

            var result = await _productRepository.UpdateProduct(productEntity);

            _logger.LogInformation("Product {ProductId} updated by user {UserId}", Id, currentUser.userId);

            return new BaseResponse() { Data = result };
        }
        public async Task<BaseResponse> BuyProduct(BuyRequestDto request, UserDetails currentUser)
        {
            _logger.LogInformation("User {UserId} is attempting to buy product {ProductId} (Quantity: {Quantity})",
                currentUser.userId, request.ProductId, request.Quantity);

            var product = await _productRepository.GetProduct(request.ProductId);
            if (product == null)
            {
                _logger.LogWarning("Product {ProductId} not found for user {UserId}", request.ProductId, currentUser.userId);
                throw new NotFoundException("Product not found");
            }

            var user = await _userRepository.GetUser(currentUser.userId);
            if (user == null)
            {
                _logger.LogError("User {UserId} not found during purchase attempt", currentUser.userId);
                throw new NotFoundException("User not found");
            }

            if (product.AmountAvailable < request.Quantity)
            {
                _logger.LogWarning("Not enough stock for Product {ProductId}. Requested: {Requested}, Available: {Available}",
                    product.Id, request.Quantity, product.AmountAvailable);
                throw new BadRequestException("Not enough stock");
            }

            int totalCost = product.Cost * request.Quantity;

            if (user.Deposit < totalCost)
            {
                _logger.LogWarning("Insufficient deposit for user {UserId}. Required: {Cost}, Available: {Deposit}",
                    user.Id, totalCost, user.Deposit);
                throw new BadRequestException("Insufficient deposit");
            }

            product.AmountAvailable -= request.Quantity;
            user.Deposit -= totalCost;

            await _userRepository.UpdateUser(user);
            await _productRepository.UpdateProduct(product);

            _logger.LogInformation("User {UserId} successfully purchased {Quantity} of Product {ProductId} for {TotalCost} cents",
                user.Id, request.Quantity, product.Id, totalCost);

            var change = CalculateChange(user.Deposit);

            return new BaseResponse
            {
                Data = new
                {
                    TotalSpent = totalCost,
                    Product = product.ProductName,
                    Quantity = request.Quantity,
                    Change = change
                }
            };
        }
        private Dictionary<int, int> CalculateChange(int amount)
        {
            int[] coins = { 100, 50, 20, 10, 5 };
            var result = new Dictionary<int, int>();

            foreach (var coin in coins)
            {
                int count = amount / coin;
                if (count > 0)
                {
                    result[coin] = count;
                    amount -= coin * count;
                }
            }

            return result;
        }

        public async Task<BaseResponse> ResetDeposit(int userId)
        {
            _logger.LogInformation("User {UserId} is resetting deposit", userId);

            var user = await _userRepository.GetUser(userId);
            if (user == null)
            {
                _logger.LogWarning("User {UserId} not found during deposit reset", userId);
                throw new NotFoundException("User not found");
            }

            user.Deposit = 0;
            await _userRepository.UpdateUser(user);

            _logger.LogInformation("Deposit reset for user {UserId}", userId);

            return new BaseResponse { Data = "Deposit reset to 0." };
        }

    }
}
