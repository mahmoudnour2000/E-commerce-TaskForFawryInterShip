using System;
using Models;
using Service;

class Program
{
    static void Main(string[] args)
    {
        // Create products
        var cheese = new Product("Cheese", 100m, 5, 400, true, DateTime.Now.AddDays(10));
        var biscuits = new Product("Biscuits", 150m, 3, 700, true, DateTime.Now.AddDays(-1)); // Expired
        var tv = new Product("TV", 1000m, 2, 5000, true, null);
        var scratchCard = new Product("Scratch Card", 50m, 10, 0, false, null);
        var outOfStockProduct = new Product("Laptop", 2000m, 0, 3000, true, null); // Out of stock

        // Create customers
        var customer1 = new Customer("Ahmed", 1000m); // Insufficient balance for some tests
        var customer2 = new Customer("Mohamed", 2000m); // Sufficient balance

        // Create shipping service and cart
        var shippingService = new ShippingService();
        var cart = new ShoppingCart(shippingService);

        // Test 1: Successful checkout
        Console.WriteLine("=== Test 1: Successful Checkout ===");
        try
        {
            cart.Add(cheese, 2);
            cart.Add(scratchCard, 1);
            cart.Checkout(customer2);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        // Reset cart for new tests
        cart = new ShoppingCart(shippingService);

        // Test 2: Insufficient balance
        Console.WriteLine("\n=== Test 2: Insufficient Balance ===");
        try
        {
            cart.Add(cheese, 2);
            cart.Add(tv, 1);
            cart.Add(scratchCard, 1);
            cart.Checkout(customer1); // Should fail (1000 < 1280)
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        // Reset cart
        cart = new ShoppingCart(shippingService);

        // Test 3: Expired product
        Console.WriteLine("\n=== Test 3: Expired Product ===");
        try
        {
            cart.Add(biscuits, 1);
            cart.Checkout(customer2);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        // Reset cart
        cart = new ShoppingCart(shippingService);

        // Test 4: Out-of-stock product
        Console.WriteLine("\n=== Test 4: Out-of-Stock Product ===");
        try
        {
            cart.Add(outOfStockProduct, 1);
            cart.Checkout(customer2);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        // Reset cart
        cart = new ShoppingCart(shippingService);

        // Test 5: Empty cart
        Console.WriteLine("\n=== Test 5: Empty Cart ===");
        try
        {
            cart.Checkout(customer2);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}