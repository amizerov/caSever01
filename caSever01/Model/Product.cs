using caStat01;
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
            Stat.DoCalc(klines);
 
            volatility = Stat.vola;
            liquidity = Stat.liqu;

            cnt1 = Stat.cnt1;
            cnt2 = Stat.cnt2;
            cnt3 = Stat.cnt3;
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
                    LogToForm.Write(new Msg(exchange, symbol, ex.Message));
                    if(ex.InnerException != null)
                        LogToForm.Write(new Msg(exchange, symbol, ex.InnerException.Message));
                }
            }
        }
    }
}
