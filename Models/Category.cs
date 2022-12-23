using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datalagring_1._0.Models
{
    internal class Category
    {
        public List<Product> ProductList { get; } = new List<Product>();

        public void AddProduct(Product product)
        {
            if (ProductList.Contains(product))
            {
                throw new ArgumentException("Produkten redan registrerad");
            }
            else
            {
                ProductList.Add(product);
            }
        }

        public Category(string categoryName)
        {
            CategoryName = categoryName;
        }

        public string CategoryName
        {
            get
            {
                return categoryName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Ogiltigt namn");
                }
                categoryName = value;
            }
        }

        private string categoryName;


    }
}
