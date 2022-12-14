using Binance.Net.Clients;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;

namespace caSever01
{
    public class Binance : Exchange
    {
        public override int ID => 1;
        public override string Name => "Binance";

        BinanceClient client = new();

        protected override Product ToProduct(object p)
        {
            BinanceProduct binaProd = (BinanceProduct)p;
            Product product = new();
            product.symbol = binaProd.Symbol;
            product.exchange = ID;
            product.baseasset = binaProd.BaseAsset;
            product.quoteasset = binaProd.QuoteAsset;

            return product;
        }
        protected override void ProcessProducts()
        {
            var r = client.SpotApi.ExchangeData.GetProductsAsync().Result;
            if (r.Success)
            {
                int i = 0;
                foreach (var p in r.Data)
                {
                    if (p.Status == "TRADING")
                    {
                        Product product = ToProduct(p);

                        List<Kline> klines = GetKlines(p.Symbol, 1, 5);
                        product.CalcStat(klines);
                        product.SaveStat();

                        Msg.Send(ID, ++i, product);
                        Thread.Sleep(1000);
                    }
                }
            }
        }
        protected override List<Kline> GetKlines(string symbol, int IntervarInMinutes, int PeriodInDays)
        {
            List<Kline> klines = new List<Kline>();

            int m = IntervarInMinutes % 60;
            int h = (IntervarInMinutes - m) / 60;
            TimeSpan klInterval = new TimeSpan(h, m, 0);
            
            var r = client.SpotApi.CommonSpotClient
                .GetKlinesAsync(symbol, klInterval, DateTime.Now.AddDays(-1 * PeriodInDays), DateTime.Now).Result;

            if (r.Success)
            {
                klines = r.Data.ToList();
            }
            else
            {
                LogToForm.Write(new Msg(ID, $"GetProductStat({symbol})", r.Error!.Message));
            }
            return klines;
        }
    }
}
