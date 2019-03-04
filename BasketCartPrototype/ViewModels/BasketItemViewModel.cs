using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasketCartPrototype.ViewModels
{
    public class BasketItemViewModel
    {
        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}