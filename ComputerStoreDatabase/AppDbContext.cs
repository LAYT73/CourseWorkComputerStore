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

       protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // OrderProduct
        modelBuilder.Entity<OrderProduct>()
            .HasKey(op => op.Id);

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Product)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(op => op.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // ProductComponent
        modelBuilder.Entity<ProductComponent>()
            .HasKey(pc => pc.Id);

        modelBuilder.Entity<ProductComponent>()
            .HasOne(pc => pc.Product)
            .WithMany(p => p.ProductComponents)
            .HasForeignKey(pc => pc.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProductComponent>()
            .HasOne(pc => pc.Component)
            .WithMany(c => c.ProductComponents)
            .HasForeignKey(pc => pc.ComponentId)
            .OnDelete(DeleteBehavior.Cascade);

        // AssemblyComponent
        modelBuilder.Entity<AssemblyComponent>()
            .HasKey(ac => ac.Id);

        modelBuilder.Entity<AssemblyComponent>()
            .HasOne(ac => ac.Assembly)
            .WithMany(a => a.AssemblyComponents)
            .HasForeignKey(ac => ac.AssemblyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AssemblyComponent>()
            .HasOne(ac => ac.Component)
            .WithMany(c => c.AssemblyComponents)
            .HasForeignKey(ac => ac.ComponentId)
            .OnDelete(DeleteBehavior.Cascade);

        // SaleProduct
        modelBuilder.Entity<SaleProduct>()
            .HasKey(sp => sp.Id);

        modelBuilder.Entity<SaleProduct>()
            .HasOne(sp => sp.Sale)
            .WithMany(s => s.SaleProducts)
            .HasForeignKey(sp => sp.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SaleProduct>()
            .HasOne(sp => sp.Product)
            .WithMany(p => p.SaleProducts)
            .HasForeignKey(sp => sp.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // AssemblySale
        modelBuilder.Entity<AssemblySale>()
            .HasKey(asl => asl.Id);

        modelBuilder.Entity<AssemblySale>()
            .HasOne(asl => asl.Sale)
            .WithMany(s => s.AssemblySales)
            .HasForeignKey(asl => asl.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AssemblySale>()
            .HasOne(asl => asl.Assembly)
            .WithMany(a => a.AssemblySales)
            .HasForeignKey(asl => asl.AssemblyId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Database=Store;Username=postgres;Password=030405");
    }
}
