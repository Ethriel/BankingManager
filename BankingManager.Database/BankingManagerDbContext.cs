using BankingManager.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BankingManager.Database
{
    public class BankingManagerDbContext : DbContext
    {
        public DbSet<BankAccount> BankAccounts { get; set; }
        public IConfiguration Configuration { get; set; }
        public BankingManagerDbContext(IConfiguration configuration) : base() { Configuration = configuration; }
        public BankingManagerDbContext(DbContextOptions options, IConfiguration configuration) : base(options) 
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Default"));
            }
            optionsBuilder.UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>()
                        .HasKey(x => x.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
