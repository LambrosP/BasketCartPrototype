using BasketCartPrototype.Core.Entities;
using BasketCartPrototype.Core.Interfaces.Cache;
using BasketCartPrototype.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketCartPrototype.Data.Implementation.Context
{
    public class BasketContext : IBasketContext
    {
        private readonly ICacheManager _cacheManager;

        public BasketContext(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
            _cacheManager.Baskets = new List<CustomerBasket>();
        }

        public async Task<(string message, int status)> AddItemAsync(int customerId, BasketItem basketItem)
        {
            string message = null;
            int status = 0;

            await Task.Run(() =>
            {
                var customerRecord = _cacheManager.Baskets.Where(c => c.CustomerId == customerId).FirstOrDefault();

                if (customerRecord != null)
                {
                    var basketItemRecord = customerRecord.Items.Where(p => p.ProductId == basketItem.ProductId).FirstOrDefault();

                    if(basketItemRecord != null)
                    {   
                        message = String.Format("Product Item:{0} already exists in Customer's:{1} basket ", basketItem.ProductId, customerId);
                        status = 500;
                    }
                    else
                    {
                        customerRecord.Items.Add(basketItem);
                        customerRecord.TotalItems += (basketItem.Quantity);
                        customerRecord.Subtotal += (basketItem.UnitPrice * basketItem.Quantity);
                        message = String.Format("Product Item:{0} was added successfully! for Customer:{1}", basketItem.ProductId, customerId);
                        status = 200;
                    }
                }
                else
                {
                    var customerBasketRecord = new CustomerBasket();

                    customerBasketRecord.Id = Guid.NewGuid();
                    customerBasketRecord.CustomerId = customerId;
                    customerBasketRecord.Items.Add(basketItem);
                    customerBasketRecord.TotalItems = basketItem.Quantity;
                    customerBasketRecord.Subtotal = (basketItem.Quantity * basketItem.UnitPrice);
                    message = String.Format("New Customer:{0} with Product Item:{1} was added successfully!", customerId, basketItem.ProductId);
                    status = 200;

                    _cacheManager.Baskets.Add(customerBasketRecord);

                }
            });

            return (message, status);
        }

        public async Task<(string message, int status)> UpdateItemQuantityAsync(int customerId, BasketItem basketItem)
        {
            string message = null;
            int status = 0;

            await Task.Run(() =>
            {
                var customerRecord = _cacheManager.Baskets.Where(c => c.CustomerId == customerId).FirstOrDefault();

                if (customerRecord != null)
                {
                    var basketItemRecord = customerRecord.Items.Where(p => p.ProductId == basketItem.ProductId).FirstOrDefault();

                    if (basketItemRecord != null)
                    {
                        basketItemRecord.Quantity += basketItem.Quantity;
                        customerRecord.TotalItems += (basketItem.Quantity);
                        customerRecord.Subtotal += (basketItem.UnitPrice * basketItem.Quantity);
                        message = String.Format("Product Item:{0} for Customer:{1} Updated successfully!", basketItem.ProductId, customerId);
                        status = 200;
                    }
                    else
                    {
                        message = String.Format("Product Item:{0} does not exist in Customer's Basket", basketItem.ProductId);
                        status = 500;
                    }
                }
                else
                {
                    message = String.Format("Customer:{0} does not exist in Database", customerId);
                    status = 500;
                }
            });

            return (message, status);
        }

        public async Task<(string message, int status)> RemoveItemAsync(int customerId, int productId)
        {
            string message = null;
            int status = 0;

            await Task.Run(() =>
            {
                var customerRecord = _cacheManager.Baskets.Where(c => c.CustomerId == customerId).FirstOrDefault();
                
                if (customerRecord != null)
                {
                    var basketItem = customerRecord.Items.Where(p => p.ProductId == productId).FirstOrDefault();

                    if (basketItem != null)
                    {
                        customerRecord.Items.Remove(basketItem);
                        customerRecord.TotalItems -= (basketItem.Quantity);
                        customerRecord.Subtotal -= (basketItem.UnitPrice * basketItem.Quantity);

                        var productItem = _cacheManager.Products.Where(p => p.ProductId == productId).FirstOrDefault();

                        if (productItem != null)
                            productItem.Quantity += basketItem.Quantity;

                        message = String.Format("Product Item:{0} for Customer:{1} was removed successfully!", productId, customerId);
                        status = 200;
                    }
                    else
                    {
                        message = String.Format("Product Item:{0} does not exist in Customer's Basket", productId);
                        status = 500;
                    }
                }
                else
                {
                    message = String.Format("Customer: {0} does not exist in Database", customerId);
                    status = 500;
                }
            });

            return (message, status);
        }

        public async Task<(string message, int status)> ClearCustomerBasketAsync(int customerId)
        {
            string message = null;
            int status = 0;

            await Task.Run(() =>
            {
                var customerRecord = _cacheManager.Baskets.Where(c => c.CustomerId == customerId).FirstOrDefault();

                if (customerRecord != null)
                {
                    customerRecord.Items.Clear();
                    customerRecord.TotalItems = 0;
                    customerRecord.Subtotal = 0;

                    message = String.Format("Customer:{0} Basket was deleted successfully!", customerId);
                    status = 200;
                }
                else
                {
                    message = String.Format("Customer:{0} does not exist in Database", customerId);
                    status = 500;
                }
            });

            return (message, status);
        }

        public async Task<(List<BasketItem> basketItems, bool isCustomerExists)> GetCustomerBasketItemsAsync(int customerId)
        {
            List<BasketItem> basketItems = new List<BasketItem>();
            bool isCustomerExists = false;

            await Task.Run(() =>
            {
                isCustomerExists = _cacheManager.Baskets.Exists(c => c.CustomerId == customerId);

                if(isCustomerExists)
                {
                    var customerRecord = _cacheManager.Baskets.Where(c => c.CustomerId == customerId).FirstOrDefault();

                    if (customerRecord != null)
                        basketItems = customerRecord.Items.ToList();
                }
            });

            return (basketItems, isCustomerExists);
        }
    }
}
