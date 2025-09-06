
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using apptrade.Models;

namespace apptrade.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Asset> Assets { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<Trade> Trades { get; set; }
    public DbSet<Movement> Movements { get; set; }
    public DbSet<AssetPrice> AssetPrices { get; set; }
    public DbSet<Watchlist> Watchlists { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}

