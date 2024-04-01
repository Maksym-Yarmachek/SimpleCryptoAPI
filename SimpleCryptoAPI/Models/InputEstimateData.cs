namespace SimpleCryptoAPI.Models
{
    public class InputEstimateData
    {
        public string Symbol { get; set; }
        public double InputAmount { get; set; }
        public bool IsBuying { get; set; }

        public InputEstimateData() 
        {
            Symbol = string.Empty;
            InputAmount = 0;
            IsBuying = false;
        }

        public InputEstimateData(string symbol, double inputAmount, bool isBuying)
        {
            Symbol = symbol;
            InputAmount = inputAmount;
            IsBuying = isBuying;
        }
    }

    public enum MarketName
    {
        Binance,
        Kucoin
    }
}
