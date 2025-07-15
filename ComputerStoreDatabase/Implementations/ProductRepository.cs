using ComputerStoreContracts.Repositories;
using ComputerStoreModels.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ComputerStoreDatabase.Implementations;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context) { }

    public async Task AddComponentToProductAsync(Guid productId, Guid componentId)
    {
        await _context.ProductComponents.AddAsync(new ProductComponent { Id = Guid.NewGuid(), ProductId = productId, ComponentId = componentId });
        await SaveChangesAsync();
    }

    public async Task RemoveComponentFromProductAsync(Guid productId, Guid componentId)
    {
        ProductComponent productComponent = await _context.ProductComponents.Where(x => x.ProductId == productId && x.ComponentId == componentId).FirstAsync();
        _context.ProductComponents.Remove(productComponent);
        await SaveChangesAsync();
    }
}
