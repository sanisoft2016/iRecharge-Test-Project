using iRechargeTestProject.Application.IService;
using iRechargeTestProject.Domain.Entities;
using iRechargeTestProject.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iRechargeTestProject.Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<ProductModel> _productRepository;

        public ProductService(IGenericRepository<ProductModel> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> CreateProduct(ProductModel product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");

            try
            {
                _productRepository.Add(product);
                await _productRepository.SaveAsync();
                return product.Id; // Assuming Id is the primary key and is set after saving
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the product.", ex);
            }
        }

        public async Task<ProductModel?> GetProductById(int productId)
        {
            var productObject = await Task.Run(() =>
                _productRepository.GetAll(x => x.Id == productId).FirstOrDefault());

            if (productObject == null)
                return null;

            return productObject;
        }

        public async Task<ProductModel> GetProductByNameAsync(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Product name cannot be null or empty.", nameof(productName));

            try
            {
                var product = await Task.Run(() =>
                    _productRepository.GetAll(x => x.Name == productName).FirstOrDefault());

                if (product == null)
                    throw new KeyNotFoundException($"Product with name '{productName}' not found.");

                return product;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the product by name.", ex);
            }
        }

        public async Task<List<ProductModel>> GetProductsAsync()
        {
            try
            {
                var products = await Task.Run(() => _productRepository.GetAll().ToList());
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the products.", ex);
            }
        }
    }
}
