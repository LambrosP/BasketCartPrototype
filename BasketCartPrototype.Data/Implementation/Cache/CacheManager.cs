using BasketCartPrototype.Core.Entities;
using BasketCartPrototype.Core.Interfaces.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketCartPrototype.Service.Implementation.Cache
{
    public class CacheManager : Dictionary<string, object>, ICacheManager
    {
        public List<Product> Products
        {
            get => ContainsKey(nameof(Products)) ? (List<Product>)base[nameof(Products)] : new List<Product>();
            set
            {
                if (ContainsKey(nameof(Products)))
                {
                    base[nameof(Products)] = value;
                }
                else
                {
                    Add(nameof(Products), value);
                }
            }
        }

        public List<CustomerBasket> Baskets
        {
            get => ContainsKey(nameof(Baskets)) ? (List<CustomerBasket>)base[nameof(Baskets)] : new List<CustomerBasket>();
            set
            {
                if (ContainsKey(nameof(Baskets)))
                {
                    base[nameof(Baskets)] = value;
                }
                else
                {
                    Add(nameof(Baskets), value);
                }
            }
        }
    }
}