namespace SimpleCryptoAPI.Models
{
    public class Order
    {
        public double Price { get; set; }
        public double Qty { get; set; }

        public Order() { }
        public Order( double price, double qty ) 
        {
            Price = price;
            Qty = qty;
        }
    }
}
