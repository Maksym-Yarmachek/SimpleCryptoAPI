using SimpleCryptoAPI.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Kucoin.Net.Objects.Models.Spot;
using Kucoin.Net.Clients;
using Binance.Spot;

namespace SimpleCryptoAPI.Services
{
    public class OrderBookParseService
    {
        public async Task<OrderBook> ParseBinance(string symbol)
        {
            OrderBook orderBook = new OrderBook();

            var binance = new Market(new HttpClient());
            var jsonStr = await binance.OrderBook(symbol, limit: 1000);

            JObject jsonOrderBook = JObject.Parse(jsonStr);

            orderBook.LastUpdateId = long.Parse(jsonOrderBook["lastUpdateId"].ToString());

            var bids = JArray.Parse(jsonOrderBook["bids"].ToString());
            orderBook.Bids = bids.Select(x =>
                            new Order
                            {
                                Price = double.Parse((string)x[0]),
                                Qty = double.Parse((string)x[1])
                            })
                            .ToList();
            var asks = JArray.Parse(jsonOrderBook["asks"].ToString());
            orderBook.Asks = asks.Select(x =>
                            new Order
                            {
                                Price = double.Parse((string)x[0]),
                                Qty = double.Parse((string)x[1])
                            })
                            .ToList();

            return orderBook;
        }

        public async Task<OrderBook> ParseKucoin(string symbol)
        {
            var kucoin = new KucoinRestClient();
            var kucoinResult = await kucoin.SpotApi.ExchangeData.GetAggregatedPartialOrderBookAsync(symbol, 100);

            KucoinOrderBook kucoinOrderBook = kucoinResult.Data;
            OrderBook orderBook = new OrderBook
            {
                LastUpdateId = kucoinOrderBook.Sequence,
                Bids = kucoinOrderBook.Bids.Select(x =>
                                new Order
                                {
                                    Price = Decimal.ToDouble(x.Price),
                                    Qty = Decimal.ToDouble(x.Quantity)
                                })
                            .ToList(),
                Asks = kucoinOrderBook.Asks.Select(x =>
                                new Order
                                {
                                    Price = Decimal.ToDouble(x.Price),
                                    Qty = Decimal.ToDouble(x.Quantity)
                                })
                            .ToList()
            };

            return orderBook;
        }
    }
}
