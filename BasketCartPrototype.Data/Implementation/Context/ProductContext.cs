using BasketCartPrototype.Core.Entities;
using BasketCartPrototype.Core.Interfaces.Cache;
using BasketCartPrototype.Data.Interfaces;
using BasketCartPrototype.Data.MemoryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketCartPrototype.Data.Implementation.Context
{
    public class ProductContext : IProductContext
    {
        private readonly ICacheManager _cacheManager;

        public ProductContext(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
            _cacheManager.Products = StockItems.FulFillListofProducts();
        }

        public async Task<List<Product>> GetAvailableProductsAsync()
        {
            return await Task.Run(() => _cacheManager.Products);
        }

        public async Task<Product> GetProductItemAsync(int productId)
        {
            return await Task.Run(() => GetProductItem(productId));
        }
        
        private Product GetProductItem(int productId)
        {
            return _cacheManager.Products.Where(p => p.ProductId == productId).FirstOrDefault();
        }
    }
}
