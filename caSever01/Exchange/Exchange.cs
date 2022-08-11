using CryptoExchange.Net.CommonObjects;

namespace caSever01
{
    public abstract class Exchange
    {
        public abstract int ID { get; }
        public abstract string Name { get; }
        public void UpdateProductsStat()
        {
            Task.Run(() => {
                ProcessProducts();
            });
        }
        protected abstract Product ToProduct(Object p);
        protected abstract void ProcessProducts();
        protected abstract List<Kline> GetKlines(string symbol, int IntervarInMinutes, int PeriodInDays);
    }
}
