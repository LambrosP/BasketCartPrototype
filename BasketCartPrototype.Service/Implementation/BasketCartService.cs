using BasketCartPrototype.Core.Interfaces.Cache;
using BasketCartPrototype.Data.Interfaces;
using BasketCartPrototype.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketCartPrototype.Core.Entities;
using static BasketCartPrototype.Core.Models.PSPMessage;
using Newtonsoft.Json;

namespace BasketCartPrototype.Service.Implementation
{
    public class BasketCartService : IBasketCartService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IProductContext _productContext;
        private readonly IBasketContext _basketContext;

        public BasketCartService(ICacheManager cacheManager, IProductContext productContext, IBasketContext basketContext)
        {
            _cacheManager = cacheManager;
            _productContext = productContext;
            _basketContext = basketContext;
        }

        public async Task<ReplyPSPMessage> AddItemAsync(int customerId, int productId, int quantity)
        {
            var PSPMessage = new ReplyPSPMessage();
           
            var productItem = await _productContext.GetProductItemAsync(productId);

            if(productItem != null)
            {
                if (productItem.Quantity < quantity)
                {
                    PSPMessage.command = "AddItem";
                    PSPMessage.status = 500;
                    PSPMessage.message = String.Format("Product Item:{0} is out of Stock", productId);
                    PSPMessage.data = new object();

                    return PSPMessage;
                }

                var basketItem = new BasketItem()
                {
                    ProductId = productItem.ProductId,
                    ProductName = productItem.ProductName,
                    UnitPrice = productItem.UnitPrice,
                    Quantity = quantity
                };

                var (message, status) = await _basketContext.AddItemAsync(customerId, basketItem);

                if(status == 200)
                    productItem.Quantity -= quantity;

                PSPMessage.command = "AddItem";
                PSPMessage.status = status;
                PSPMessage.message = message;
                PSPMessage.data = new object();
            }
            else
            {
                PSPMessage.command = "AddItem";
                PSPMessage.status = 500;
                PSPMessage.message = String.Format("Product Item:{0} does not exist in Database", productId);
                PSPMessage.data = new object();
                
                return PSPMessage;
            }
            
            return PSPMessage;
        }

        public async Task<ReplyPSPMessage> UpdateItemQuantityAsync(int customerId, int productId, int quantity)
        {
            var PSPMessage = new ReplyPSPMessage();

            var productItem = await _productContext.GetProductItemAsync(productId);

            if (productItem != null)
            {
                if (productItem.Quantity < quantity)
                {
                    PSPMessage.command = "UpdateItem";
                    PSPMessage.status = 500;
                    PSPMessage.message = String.Format("Product Item:{0} is out of Stock", productId);
                    PSPMessage.data = new object();

                    return PSPMessage;
                }

                var basketItem = new BasketItem()
                {
                    ProductId = productItem.ProductId,
                    ProductName = productItem.ProductName,
                    UnitPrice = productItem.UnitPrice,
                    Quantity = quantity
                };

                var (message, status) = await _basketContext.UpdateItemQuantityAsync(customerId, basketItem);

                if(status == 200)
                    productItem.Quantity -= quantity;

                PSPMessage.command = "UpdateItem";
                PSPMessage.status = status;
                PSPMessage.message = message;
                PSPMessage.data = new object();
            }
            else
            {
                PSPMessage.command = "UpdateItem";
                PSPMessage.status = 500;
                PSPMessage.message = String.Format("Product Item:{0} does not exist in Database", productId);
                PSPMessage.data = new object();

                return PSPMessage;
            }

            return PSPMessage;
        }

        public async Task<ReplyPSPMessage> RemoveItemAsync(int customerId, int productId)
        {
            var PSPMessage = new ReplyPSPMessage();

            var (message, status) = await _basketContext.RemoveItemAsync(customerId, productId);

            PSPMessage.command = "RemoveItem";
            PSPMessage.status = status;
            PSPMessage.message = message;
            PSPMessage.data = new object();

            return PSPMessage;
        }

        public async Task<ReplyPSPMessage> ClearCustomerBasketAsync(int customerId)
        {
            var PSPMessage = new ReplyPSPMessage();

            var (basketItems, isCustomerExists) = await _basketContext.GetCustomerBasketItemsAsync(customerId);

            if(basketItems == null || basketItems.Count == 0)
            {
                PSPMessage.command = "ClearCustomerBasket";
                PSPMessage.status = 200;
                PSPMessage.message = String.Format("Basket for Customer:{0} is empty", customerId);
                PSPMessage.data = new object();

                return PSPMessage;
            }

            foreach (var item in basketItems)
            {
                var productItem = await _productContext.GetProductItemAsync(item.ProductId);

                if(productItem != null)
                {
                    productItem.Quantity += item.Quantity;
                }
            }

            var(message, status) = await _basketContext.ClearCustomerBasketAsync(customerId);

            PSPMessage.command = "ClearCustomerBasket";
            PSPMessage.status = status;
            PSPMessage.message = message;
            PSPMessage.data = new object();

            return PSPMessage;
        }

        public async Task<ReplyPSPMessage> GetCustomerBasketItemsAsync(int customerId)
        {
            var PSPMessage = new ReplyPSPMessage();

            var (basketItems, isCustomerExists) =  await _basketContext.GetCustomerBasketItemsAsync(customerId);

            if(!isCustomerExists)
            {
                PSPMessage.command = "GetCustomerBasketItems";
                PSPMessage.status = 500;
                PSPMessage.message = String.Format("Customer:{0} does not exist in database", customerId);
                PSPMessage.data = new object();

                var test1 = _cacheManager.Products;
                var test2 = _cacheManager.Baskets;

                return PSPMessage;
            }
            
            if(basketItems != null && basketItems.Count > 0) 
            {
                PSPMessage.command = "GetCustomerBasketItems";
                PSPMessage.status = 200;
                PSPMessage.message = String.Format("Get Basket Items for Customer:{0}", customerId);
                PSPMessage.data = basketItems;
            }
            else
            {
                PSPMessage.command = "GetCustomerBasketItems";
                PSPMessage.status = 500;
                PSPMessage.message = String.Format("Basket for Customer:{0} is empty", customerId);
                PSPMessage.data = new object();

                return PSPMessage;
            }

            return PSPMessage;
        }

        public async Task<ReplyPSPMessage> GetAvailableProductsInfoAsync()
        {
            var PSPMessage = new ReplyPSPMessage();

            var productItems =  await _productContext.GetAvailableProductsAsync();

            if(productItems != null && productItems.Count > 0)
            {
                PSPMessage.command = "GetAvailableProductsInfo";
                PSPMessage.status = 200;
                PSPMessage.message = String.Format("Get all products Info");
                PSPMessage.data = productItems;
            }
            else
            {
                PSPMessage.command = "GetAvailableProductsInfo";
                PSPMessage.status = 500;
                PSPMessage.message = String.Format("Producst Info List is empty");
                PSPMessage.data = new object();

                return PSPMessage;
            }

            return PSPMessage;
        }

        public async Task<ReplyPSPMessage> GetProductItemAsync(int productId)
        {
            var PSPMessage = new ReplyPSPMessage();

            var productItem = await _productContext.GetProductItemAsync(productId);
            
            if(productItem != null)
            {
                PSPMessage.command = "GetProductItem";
                PSPMessage.status = 200;
                PSPMessage.message = String.Format("Get Product Item:{0} Info", productId);
                PSPMessage.data = productItem;
            }
            else
            {
                PSPMessage.command = "GetProductItem";
                PSPMessage.status = 500;
                PSPMessage.message = String.Format("Product Item:{0} does not exist in Database", productId);
                PSPMessage.data = new object();

                return PSPMessage;
            }

            return PSPMessage;
        }
    }
}
