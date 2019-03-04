using BasketCartPrototype.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketCartPrototype.Data.Interfaces
{
    public interface IProductContext
    {
        Task<List<Product>> GetAvailableProductsAsync();
        Task<Product> GetProductItemAsync(int productId);
    }
}
