using BasketCartPrototype.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketCartPrototype.Data.MemoryData
{
    public class StockItems
    {
        public static async Task<List<Product>> GetProducts()
        {
            var listofProcts = await Task.Run(() => FulFillListofProducts());

            return listofProcts;
        }

        public static List<Product> FulFillListofProducts()
        {
            return new List<Product>
            {
                new Product {
                    ProductId = 1,
                    ProductName = "FIFA 2019",
                    UnitPrice = 70,
                    Quantity = 2000
                },
                new Product {
                    ProductId = 2,
                    ProductName = "PRO EVOLUTION SOCCER 2019",
                    UnitPrice = 70,
                    Quantity = 2000
                },
                new Product {
                    ProductId = 3,
                    ProductName = "Resident Evil 2",
                    UnitPrice = 80,
                    Quantity = 3000
                },
                new Product {
                    ProductId = 4,
                    ProductName = "Anthem",
                    UnitPrice = 80,
                    Quantity = 1500
                },
                new Product {
                    ProductId = 5,
                    ProductName = "Crackdown 3",
                    UnitPrice = 60,
                    Quantity = 2000
                },
                new Product {
                    ProductId = 6,
                    ProductName = "Devil May Cry 5",
                    UnitPrice = 70,
                    Quantity = 1000
                },
                new Product {
                    ProductId = 7,
                    ProductName = "The Division 2",
                    UnitPrice = 70,
                    Quantity = 3000
                },
                new Product {
                    ProductId = 8,
                    ProductName = "Days Gone",
                    UnitPrice = 80,
                    Quantity = 1000
                },
            };
        }
    }
}
