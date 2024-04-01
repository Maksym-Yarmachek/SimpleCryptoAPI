namespace SimpleCryptoAPI.Models
{
    public class MarketPrices
    {
        public string MarketName { get; set; }
        public string Symbol { get; set; }
        public string Price { get; set; }

        public MarketPrices()
        {
            MarketName = string.Empty;
            Symbol = string.Empty;
            Price = string.Empty;
        }
        public MarketPrices(string marketName, string symbol, string price)
        {
            MarketName = marketName;
            Symbol = symbol;
            Price = price;
        }
    }
}
