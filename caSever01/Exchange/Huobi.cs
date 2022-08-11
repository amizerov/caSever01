using Huobi.Net.Enums;
using Huobi.Net.Clients;
using Huobi.Net.Objects.Models;
using CryptoExchange.Net.CommonObjects;

namespace caSever01
{
    public class Huobi : Exchange
    {
        public override int ID => 3;
        public override string Name => "Huobi";

        HuobiClient client = new();

        protected override Product ToProduct(object p)
        {
            HuobiSymbol binaProd = (HuobiSymbol)p;
            Product product = new();
            product.symbol = binaProd.Name;
            product.exchange = ID;
            product.baseasset = binaProd.BaseAsset;
            product.quoteasset = binaProd.QuoteAsset;

            return product;
        }
        public Huobi()
        {
            client = new HuobiClient();
        }
        protected override void ProcessProducts()
        {
            var r = client.SpotApi.ExchangeData.GetSymbolsAsync().Result;
            if (r.Success)
            {
                foreach (var p in r.Data)
                {
                    if (p.State == SymbolState.Online)
                    {
                        Product product = ToProduct(p);
                        List<Kline> klines = GetKlines(p.Name, 1, 5);
                        if (klines.Count > 0)
                        {
                            product.CalcStat(klines);
                            product.SaveStat();
                        }
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
                .GetKlinesAsync(symbol, klInterval).Result; // !!! не хочет фром ту как в др биржах
                                                            //, DateTime.Now.AddDays(-1 * PeriodInDays), DateTime.Now).Result;

            if (r.Success)
            {
                klines = r.Data.ToList();

                if (klines.Count == 0)
                {
                    Msg.Send(3, $"GetProductStat({symbol})", "i==0");
                }
            }
            else
            {
                Msg.Send(3, $"GetProductStat({symbol})", r.Error!.Message);
            }
            return klines;
        }
    }
}
