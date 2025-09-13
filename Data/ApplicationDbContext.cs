
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

    public DbSet<Assest> DbAssest { get; set; }

    public DbSet<Customer> DbCustomer { get; set; }

    public DbSet<Contact> DbContact { get; set; }

    public DbSet<Watchlist> DbWatchlist { get; set; }

    public DbSet<Wallet> DbWallet { get; set; }

    public DbSet<WalletTransaction> DbWalletTransaction { get; set; }
}

