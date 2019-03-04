using BasketCartPrototype.Core.Interfaces.Services;
using BasketCartPrototype.Helpers;
using BasketCartPrototype.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BasketCartPrototype.Controllers
{
    public class BasketCartController : ApiController
    {
        private readonly IBasketCartService _basketCartService;

        public BasketCartController(IBasketCartService basketCartService)
        {
            _basketCartService = basketCartService;
        }

        [HttpPut]
        [Route("api/v1/addBasketItem")]
        public async Task<IHttpActionResult> AddBasketItemAsync(BasketItemViewModel productItemViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            var response = await _basketCartService.AddItemAsync(productItemViewModel.CustomerId, productItemViewModel.ProductId, productItemViewModel.Quantity);

            if(response.status == 500)
            {
                return new HttpActionResult(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject(response));
            }

            return new HttpActionResult(HttpStatusCode.OK, JsonConvert.SerializeObject(response));
        }

        [HttpPost]
        [Route("api/v1/updateBasketItem")]
        public async Task<IHttpActionResult> UpdateBasketItemAsync(BasketItemViewModel productItemViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            var response = await _basketCartService.UpdateItemQuantityAsync(productItemViewModel.CustomerId, productItemViewModel.ProductId, productItemViewModel.Quantity);

            if (response.status == 500)
            {
                return new HttpActionResult(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject(response));
            }

            return new HttpActionResult(HttpStatusCode.OK, JsonConvert.SerializeObject(response));
        }

        [HttpPost]
        [Route("api/v1/removeBasketItem")]
        public async Task<IHttpActionResult> RemoveBasketItemAsync(BasketItemViewModel productItemViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            var response = await _basketCartService.RemoveItemAsync(productItemViewModel.CustomerId, productItemViewModel.ProductId);

            if (response.status == 500)
            {
                return new HttpActionResult(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject(response));
            }

            return new HttpActionResult(HttpStatusCode.OK, JsonConvert.SerializeObject(response));
        }

        [HttpPost]
        [Route("api/v1/clearCustomerBasket")]
        public async Task<IHttpActionResult> ClearCustomerBasketAsync(BasketItemViewModel productItemViewModel)
        {
            if (productItemViewModel.CustomerId <= 0)
                return BadRequest("Not a valid customer id");

            var response = await _basketCartService.ClearCustomerBasketAsync(productItemViewModel.CustomerId);

            if (response.status == 500)
            {
                return new HttpActionResult(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject(response));
            }

            return new HttpActionResult(HttpStatusCode.OK, JsonConvert.SerializeObject(response));
        }

        [HttpGet]
        [Route("api/v1/getCustomerBasketItems")]
        public async Task<IHttpActionResult> GetCustomerBasketItemsAsync(int customerId)
        {
            if (customerId <= 0)
                return BadRequest("Not a valid customer id");

            var response = await _basketCartService.GetCustomerBasketItemsAsync(customerId);

            if (response.status == 500)
            {
                return new HttpActionResult(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject(response));
            }

            return new HttpActionResult(HttpStatusCode.OK, JsonConvert.SerializeObject(response));
        }

        [HttpGet]
        [Route("api/v1/getAvailableProductsInfo")]
        public async Task<IHttpActionResult> GetAvailableProductsInfoAsync()
        {
            var response = await _basketCartService.GetAvailableProductsInfoAsync();

            if (response.status == 500)
            {
                return new HttpActionResult(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject(response));
            }

            return new HttpActionResult(HttpStatusCode.OK, JsonConvert.SerializeObject(response));
        }

        [HttpGet]
        [Route("api/v1/getProductItem")]
        public async Task<IHttpActionResult> GetProductItemAsync(int productId)
        {
            if (productId <= 0)
                return BadRequest("Not a valid product id");

            var response = await _basketCartService.GetProductItemAsync(productId);

            if (response.status == 500)
            {
                return new HttpActionResult(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject(response));
            }

            return new HttpActionResult(HttpStatusCode.OK, JsonConvert.SerializeObject(response));
        }
    }
}
