using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Service
{
    public class ShoppingCart
    {
        private List<CartItem> items;
        private ShippingService shippingService;

        public ShoppingCart(ShippingService shippingService)
        {
            items = new List<CartItem>();
            this.shippingService = shippingService;
        }

        public void Add(Product product, int quantity)
        {
            // Validate inputs
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            if (product.IsOutOfStock())
                throw new InvalidOperationException($"Product {product.Name} is out of stock.");
            if (product.IsExpired())
                throw new InvalidOperationException($"Product {product.Name} is expired.");
            if (quantity > product.AvailableQuantity)
                throw new InvalidOperationException($"Requested quantity ({quantity}) of {product.Name} exceeds available quantity ({product.AvailableQuantity}).");

            // Add or update product in cart
            var existingItem = items.Find(item => item.Product.Name == product.Name);
            if (existingItem != null)
            {
                if (existingItem.Quantity + quantity > product.AvailableQuantity)
                    throw new InvalidOperationException($"Total quantity ({existingItem.Quantity + quantity}) of {product.Name} exceeds available quantity ({product.AvailableQuantity}).");
                existingItem.Quantity += quantity;
            }
            else
            {
                items.Add(new CartItem(product, quantity));
            }
        }

        public void Checkout(Customer customer)
        {
            // Validate inputs
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));
            if (items.Count == 0)
                throw new InvalidOperationException("Cart is empty.");

            // Re-validate product availability and expiry
            foreach (var item in items)
            {
                if (item.Product.IsOutOfStock())
                    throw new InvalidOperationException($"Product {item.Product.Name} is out of stock.");
                if (item.Product.IsExpired())
                    throw new InvalidOperationException($"Product {item.Product.Name} is expired.");
            }

            // Calculate costs
            decimal subtotal = items.Sum(item => item.TotalPrice);
            double totalWeight = items.Sum(item => item.TotalWeight);
            decimal shippingCost = totalWeight > 0 ? 30m : 0m; // Fixed shipping cost if there is weight
            decimal totalAmount = subtotal + shippingCost;

            // Validate customer balance
            if (customer.Balance < totalAmount)
                throw new InvalidOperationException($"Customer balance ({customer.Balance}) is insufficient to cover the total amount ({totalAmount}).");

            // Send shippable items to ShippingService
            var shippableItems = items
                .Where(item => item.Product.RequiresShipping)
                .Select(item => item.Product)
                .ToList<IShippable>();
            if (shippableItems.Any())
            {
                shippingService.ShipItems(shippableItems);
            }

            // Deduct quantities from stock
            foreach (var item in items)
            {
                item.Product.AvailableQuantity -= item.Quantity;
            }

            // Deduct total amount from customer balance
            customer.Balance -= totalAmount;

            // Output checkout details
            Console.WriteLine("** Checkout Receipt **");
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Quantity}x {item.Product.Name} {item.TotalPrice}");
            }
            Console.WriteLine($"Subtotal {subtotal}");
            Console.WriteLine($"Shipping {shippingCost}");
            Console.WriteLine($"Total Amount {totalAmount}");
            Console.WriteLine($"Customer Current Balance {customer.Balance}");
            Console.WriteLine("END.");

            // Clear the cart after successful checkout
            items.Clear();
        }
    }
}
