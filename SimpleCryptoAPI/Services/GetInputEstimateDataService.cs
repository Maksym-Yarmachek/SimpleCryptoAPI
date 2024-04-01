using Binance.Spot;
using Kucoin.Net.Clients;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleCryptoAPI.Models;

namespace SimpleCryptoAPI.Services
{
    public static class GetInputEstimateDataService
    {
        public static async Task<InputEstimateData> GetInputEstimateData(string inputCurrency, string outputCurrency, double inputAmount, MarketName marketName)
        {
            bool isBuying = false;
            string symbol = string.Empty;

            switch (marketName)
            {
                case MarketName.Binance:
                    symbol = inputCurrency + outputCurrency;
                    isBuying = false;
                    var binance = new Market(new HttpClient());
                    try
                    {
                        var binanceResult = await binance.SymbolPriceTicker(symbol);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            symbol = outputCurrency + inputCurrency;
                            isBuying = true;
                            var binanceResult = await binance.SymbolPriceTicker(symbol);
                        }
                        catch (Exception)
                        {
                            throw new Exception("The pair doesn't exist on Binance");
                        }
                    }
                    break;
                
                case MarketName.Kucoin:
                    symbol = inputCurrency + "-" + outputCurrency;
                    isBuying = false;
                    var kucoin = new KucoinRestClient();
                    var kucoinTicker = await kucoin.SpotApi.ExchangeData.GetTickerAsync(symbol);

                    if (kucoinTicker.Data == null)
                    {
                        symbol = outputCurrency + "-" + inputCurrency;
                        isBuying = true;
                        kucoinTicker = await kucoin.SpotApi.ExchangeData.GetTickerAsync(symbol);
                    }
                    if (kucoinTicker.Data == null)
                        throw new Exception("The pair doesn't exist on Kucoin");
                    break;
            }

            return new InputEstimateData(symbol, inputAmount, isBuying);
        }

    }
}
