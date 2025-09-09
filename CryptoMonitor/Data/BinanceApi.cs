using Binance.Net.Clients;

namespace CryptoMonitor.Data
{
    public class BinanceApi
    {
        private const string BTCUSDT = "BTCUSDT";
        private const string ETHUSDT = "ETHUSDT";

        public async static Task<Price> GetPrice()
        {
            var price = new Price();

            var restClient = new BinanceRestClient();

            var tickerResults = await restClient.SpotApi.ExchangeData.GetTickersAsync(new string[] { BTCUSDT, ETHUSDT });

            if (tickerResults != null)
            {
                var p = tickerResults.Data.FirstOrDefault(x => x.Symbol.Equals(BTCUSDT));
                price.Btc = p.LastPrice;

                p = tickerResults.Data.FirstOrDefault(x => x.Symbol.Equals(ETHUSDT));
                price.Eth = p.LastPrice;
            }

            return price;
        }
    }
}
