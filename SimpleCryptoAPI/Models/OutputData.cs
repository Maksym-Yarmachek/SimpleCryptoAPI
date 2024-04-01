namespace SimpleCryptoAPI.Models
{
    public class OutputData
    {
        public string ExchangeName {  get; set; }
        public double OutputAmount { get; set; }

        public OutputData(string exchangeName, double outputAmount)
        {
            ExchangeName = exchangeName;
            OutputAmount = outputAmount;
        }
    }
}
