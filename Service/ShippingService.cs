namespace Service
{
    public class ShippingService
    {
        public void ShipItems(List<IShippable> shippableItems)
        {
            Console.WriteLine("** Shipment Notice **");
            foreach (var item in shippableItems)
            {
                Console.WriteLine($"{item.GetName()} - Weight: {item.GetWeight()}g");
            }
            double totalWeight = shippableItems.Sum(item => item.GetWeight());
            Console.WriteLine($"Total package weight {totalWeight / 1000} kg");
            Console.WriteLine("Items sent for shipping.");
        }
    }
}
