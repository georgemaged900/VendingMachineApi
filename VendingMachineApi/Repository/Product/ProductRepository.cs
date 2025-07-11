using FlapKap.Middleware;
using FlapKapBackendChallenge;
using FlapKapBackendChallenge.Dto;
using FlapKapBackendChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace FlapKap.Repository.ProductRepo
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        public async Task<int> AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            return await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) 
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Product>> GetProduct()
        {
            return await _context.Products.ToListAsync();
        }
        public async Task<Product> GetProduct(int Id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == Id);
        }
        public async Task<int> UpdateProduct(Product product)
        {
            var productEntity = await _context.Products.Where(c => c.Id == product.Id).FirstOrDefaultAsync();

            if(productEntity == null)
                throw new NotFoundException("Product not found");
            if (product.ProductName !=null)
                productEntity.ProductName = product.ProductName;
            if(product.SellerId !=null && product.SellerId !=0)
                productEntity.SellerId = product.SellerId;
            if(product.AmountAvailable !=null && product.AmountAvailable !=0)
                productEntity.AmountAvailable = product.AmountAvailable;
            if(product.Cost != null && product.Cost != 0)
                productEntity.Cost = product.Cost;
            return await _context.SaveChangesAsync();
        }
    }
 }
