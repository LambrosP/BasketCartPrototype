using BasketCartPrototype.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BasketCartPrototype.Core.Models.PSPMessage;

namespace BasketCartPrototype.Core.Interfaces.Services
{
    public interface IBasketCartService
    {
        Task<ReplyPSPMessage> AddItemAsync(int customerId, int productId, int quantity);
        Task<ReplyPSPMessage> UpdateItemQuantityAsync(int customerId, int productId, int quantity);
        Task<ReplyPSPMessage> RemoveItemAsync(int customerId, int productId);
        Task<ReplyPSPMessage> ClearCustomerBasketAsync(int customerId);
        Task<ReplyPSPMessage> GetCustomerBasketItemsAsync(int customerId);
        Task<ReplyPSPMessage> GetAvailableProductsInfoAsync();
        Task<ReplyPSPMessage> GetProductItemAsync(int productId);
    }
}
