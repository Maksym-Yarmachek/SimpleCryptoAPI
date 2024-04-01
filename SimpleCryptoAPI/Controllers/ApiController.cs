using Microsoft.AspNetCore.Mvc;
using SimpleCryptoAPI.Models;
using Binance.Spot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleCryptoAPI.Services;
using Kucoin.Net.Clients;

namespace SimpleCryptoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet("estimate")]
        public async Task<IActionResult> Estimate(double inputAmount, string inputCurrency, string outputCurrency)
        {
            try
            {
                InputEstimateData inputDataBinance = await GetInputEstimateDataService.GetInputEstimateData(inputCurrency, outputCurrency, inputAmount, MarketName.Binance);
                InputEstimateData inputDataKucoin = await GetInputEstimateDataService.GetInputEstimateData(inputCurrency, outputCurrency, inputAmount, MarketName.Kucoin);

                OrderBook orderBookBinance = await new OrderBookParseService().ParseBinance(inputDataBinance.Symbol);
                OrderBook orderBookKucoin = await new OrderBookParseService().ParseKucoin(inputDataKucoin.Symbol);

                List<OutputData> outputDataList = [];
                outputDataList.Add(new OutputData("Binance", new OrderEstimatePriceService().OrderEstimatePrice(orderBookBinance, inputDataBinance.InputAmount, inputDataBinance.IsBuying)));
                outputDataList.Add(new OutputData("Kucoin", new OrderEstimatePriceService().OrderEstimatePrice(orderBookKucoin, inputDataKucoin.InputAmount, inputDataKucoin.IsBuying)));

                outputDataList = [.. outputDataList.OrderBy(x => x.OutputAmount)];

                string outputString = JsonConvert.SerializeObject(outputDataList.Last(), Formatting.None);
                return Ok(outputString);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpGet("getRates")]
        public async Task<IActionResult> GetRates(string currency)
        {
            try
            {
                List<MarketPrices> marketPrices = new List<MarketPrices>();

                var binance = new Market(new HttpClient());
                var binanceResult = await binance.SymbolPriceTicker();
                var binanceJArray = JArray.Parse(binanceResult);

                foreach (var item in binanceJArray)
                {
                    if (item["symbol"].ToString().Contains(currency) == true)
                    {
                        marketPrices.Add(new MarketPrices("Binance", item["symbol"].ToString(), item["price"].ToString()));
                    }
                }

                var kucoin = new KucoinRestClient();
                var kucoinResult = await kucoin.SpotApi.ExchangeData.GetTickersAsync();

                foreach (var item in kucoinResult.Data.Data)
                {
                    if (item.Symbol.Contains(currency) == true)
                    {
                        marketPrices.Add(new MarketPrices("Kucoin", item.Symbol, item.LastPrice.ToString()));
                    }
                }

                if (marketPrices.Count == 0)
                {
                    return BadRequest($"There are no pairs with \"{currency}\" cryptocurrency.");
                }

                string outputString = JsonConvert.SerializeObject(marketPrices, Formatting.None);
                return Ok(outputString);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
