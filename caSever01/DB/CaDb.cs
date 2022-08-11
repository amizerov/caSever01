using Microsoft.EntityFrameworkCore;

namespace caSever01
{
    public class CaDb: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=progerx.svr.vc;Database=CryptoAlert;UID=ca;PWD=1qaz!QAZ");
        }
        public DbSet<Product>? Products { get; set; }
    }
}
