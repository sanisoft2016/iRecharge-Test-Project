using iRechargeTestProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRechargeTestProject.Application.IService
{
    public interface IProductService
    {
       Task<List<ProductModel>> GetProductsAsync();
        Task<ProductModel?> GetProductById(int productId);
        Task<ProductModel> GetProductByNameAsync(string productName);
        Task<int> CreateProduct(ProductModel product);
    }
}
