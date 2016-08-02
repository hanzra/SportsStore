using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> list = new List<CartLine>();
        public void AddItem(Product product, int quantity)
        {
            CartLine line = list.FirstOrDefault(e => e.Product.ProductID == product.ProductID);                

            if (line == null)
            {
                list.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void ClearCart()
        {
            list.Clear();
        }

        public void RemoveLine(Product product)
        {
            list.RemoveAll(e => e.Product.ProductID == product.ProductID);
        }

        public decimal ComputeTotalValue()
        {
            return list.Sum(e => e.Product.Price * e.Quantity);
        }
        public IEnumerable<CartLine> AllCartLines
        {
            get { return list; }
        }

        public class CartLine
        {
            public Product Product { get; set; }
            public int Quantity { get; set; }
        }
    }
}
