using Binance.Net.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMonitor.Data
{
    public class BinanceApi
    {
        public async static Task<int> GetBtcPrice()
        {
            var restClient = new BinanceRestClient();
            var tickerResult = await restClient.SpotApi.ExchangeData.GetTickerAsync("BTCUSDT");
            var lastPrice = Convert.ToInt32(tickerResult.Data.LastPrice);

            return lastPrice;
        }
    }
}
