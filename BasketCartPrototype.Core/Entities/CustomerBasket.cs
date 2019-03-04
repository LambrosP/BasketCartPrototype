using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketCartPrototype.Core.Entities
{
    public class CustomerBasket
    {
        public Guid Id { get; set; }

        public int CustomerId { get; set; }

        public IList<BasketItem> Items { get; set; } = new List<BasketItem>();

        public int TotalItems { get; set; }

        public decimal Subtotal { get; set; }
    }
}
