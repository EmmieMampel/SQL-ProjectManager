namespace Datalagring_1._0.Models
{
    class Product
    {
        public Product(string itemNumber, string productName, string productDescription, decimal productPrice)
        {
            ItemNumber = itemNumber;
            ProductName = productName;
            ProductDescription = productDescription;
            ProductPrice = productPrice;
        }

        public string ItemNumber { get; set; }
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal ProductPrice { get; set; }


    }

    
}
