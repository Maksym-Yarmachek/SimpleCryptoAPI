namespace SimpleCryptoAPI.Models
{
    public class OrderBook
    {
        public long LastUpdateId { get; set; }
        public List<Order> Bids { get; set; }
        public List<Order> Asks { get; set; }

        public OrderBook()
        {
            LastUpdateId = 0;
            Bids = new List<Order>();
            Asks = new List<Order>();
        }
    }
}
