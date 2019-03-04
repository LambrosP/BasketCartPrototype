using BasketCartPrototype.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketCartPrototype.Data.Interfaces
{
    public interface IBasketContext
    {
        Task<(string message, int status)> AddItemAsync(int customerId, BasketItem basketItem);
        Task<(string message, int status)> UpdateItemQuantityAsync(int customerId, BasketItem basketItem);
        Task<(string message, int status)> RemoveItemAsync(int customerId, int productId);
        Task<(string message, int status)> ClearCustomerBasketAsync(int customerId);
        Task<(List<BasketItem> basketItems, bool isCustomerExists)> GetCustomerBasketItemsAsync(int customerId);
    }
}
