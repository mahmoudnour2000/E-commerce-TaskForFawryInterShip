using System;

namespace Models
{
    public class Product : IShippable
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int AvailableQuantity { get; set; }
        public double Weight { get; set; } 
        public bool RequiresShipping { get; set; }
        public DateTime? ExpiryDate { get; set; }


        public Product(string name, decimal price, int availableQuantity, double weight, bool requiresShipping, DateTime? expiryDate = null)
        {
            Name = name;
            Price = price;
            AvailableQuantity = availableQuantity;
            Weight = weight;
            RequiresShipping = requiresShipping;
            ExpiryDate = expiryDate;
        }


        public string GetName() => Name;
        public double GetWeight() => Weight;

        public bool IsExpired() => ExpiryDate.HasValue && ExpiryDate.Value < DateTime.Now;
        public bool IsOutOfStock() => AvailableQuantity <= 0;
    }
}
