using CryptoExchange.Net.CommonObjects;

namespace caSever01
{
    public class Product
    {
        public int Id { get; set; }
        public string symbol { get; set; }
        public int exchange { get; set; }
        public string baseasset { get; set; }
        public string quoteasset { get; set; }
        public double volatility { get; set; }
        public double liquidity { get; set; }
        public int cnt1 { get; set; }
        public int cnt2 { get; set; }
        public int cnt3 { get; set; }
        public Product() { symbol = baseasset = quoteasset = ""; }
         
        public void CalcStat(List<Kline> klines)
        {
            int i = 0, c1 = 0, c2 = 0;
            double v = 0, d = 0;

            if(klines.Count == 0)
            {
                volatility = liquidity = cnt1 = cnt2 = cnt3 = -1;
                return;
            }

            double R = (double)klines.Average(k => k.ClosePrice)!;
            double H = (double)klines.Max(k => k.HighPrice)!;
            double L = (double)klines.Min(k => k.LowPrice)!;

            foreach (Kline k in klines)
            {
                ++i;

                double x = 100 * ((double)k.ClosePrice! - R);
                d += Math.Pow(x / (double)R, 2);

                c1 += k.Volume > 0 ? 1 : 0;
                c2 += k.HighPrice - k.LowPrice > 0 ? 1 : 0;

                v += (double)k.Volume!;
            }
            cnt1 = 100 * c1 / i;
            cnt2 = 100 * c2 / i;
            cnt3 = (int)(100 * (H - L) / R);

            volatility = Math.Round(Math.Sqrt((double)d / i) * 100, 2);
            liquidity = Math.Round(Math.Log10((double)v / i) * 100, 2);
            if(liquidity == double.NegativeInfinity) 
                liquidity = 0;

            Msg.Send(exchange, symbol,
                $"i={i};\tv={volatility};\tl={liquidity};\tc1={cnt1};\tc2={cnt2};\tc3={cnt3}");

        }
        public void SaveStat()
        {
            using (CaDb db = new())
            {
                try
                {
                    Product? p = db.Products!.FirstOrDefault(p => p.symbol == symbol && p.exchange == exchange);
                    if (p != null)
                    {
                        db.Products!.Remove(p);
                    }
                    db.Products!.Add(this);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Msg.Send(exchange, symbol, ex.Message);
                    if(ex.InnerException != null)
                        Msg.Send(exchange, symbol, ex.InnerException.Message);
                }
            }
        }
    }
}
