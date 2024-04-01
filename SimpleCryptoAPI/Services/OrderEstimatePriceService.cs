using SimpleCryptoAPI.Models;

namespace SimpleCryptoAPI.Services
{
    public class OrderEstimatePriceService
    {
        public double OrderEstimatePrice(OrderBook orderBook, double orderAmount, bool isBuying)
        {
            double outputAmount = 0;
            double leftAmount = orderAmount;
            List<Order> ordersToFill = [];

            if (isBuying)
            {
                for (int i = 0; leftAmount > 0.0 && i < orderBook.Asks.Count; i++)
                {
                    leftAmount -= orderBook.Asks[i].Price * orderBook.Asks[i].Qty;

                    if (leftAmount >= 0.0)
                        ordersToFill.Add(orderBook.Asks[i]);
                    else
                        ordersToFill.Add(new Order(orderBook.Asks[i].Price, orderBook.Asks[i].Qty + (leftAmount / orderBook.Asks[i].Price)));
                }

                for (int i = 0; i < ordersToFill.Count; i++)
                    outputAmount += ordersToFill[i].Qty;
            }
            else
            {
                for (int i = 0; leftAmount > 0.0 && i < orderBook.Bids.Count; i++)
                {
                    leftAmount -= orderBook.Bids[i].Qty;

                    if (leftAmount >= 0.0)
                        ordersToFill.Add(orderBook.Bids[i]);
                    else
                        ordersToFill.Add(new Order(orderBook.Bids[i].Price, orderBook.Bids[i].Qty + leftAmount));
                }

                for (int i = 0; i < ordersToFill.Count; i++)
                    outputAmount += ordersToFill[i].Price * ordersToFill[i].Qty;
            }

            return outputAmount;
        }
    }
}
