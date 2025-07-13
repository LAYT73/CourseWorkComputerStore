using ComputerStoreModels.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerStoreDatabase;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Assembly> Assemblies { get; set; }
    public DbSet<AssemblyComponent> AssemblyComponents { get; set; }
    public DbSet<AssemblySale> AssemblySales { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductComponent> ProductComponents { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleProduct> SaleProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=ComputerStore;Username=postgres;Password=030405");
    }
}
