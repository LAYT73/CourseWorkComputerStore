using ComputerStoreContracts.Repositories;
using ComputerStoreModels.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerStoreDatabase.Implementations;

public class ComponentRepository : Repository<Component>, IComponentRepository
{
    public ComponentRepository(AppDbContext context) : base(context) { }

    public async Task AttachToAssemblyAsync(Guid componentId, Guid assemblyId)
    {
        await _context.AssemblyComponents.AddAsync(new AssemblyComponent { Id = Guid.NewGuid(), AssemblyId = assemblyId, ComponentId = componentId });
        await SaveChangesAsync();
    }

    public async Task DetachFromAssemblyAsync(Guid componentId, Guid assemblyId)
    {
        AssemblyComponent assemblyComponent = await _context.AssemblyComponents.Where(x => x.AssemblyId == assemblyId && x.ComponentId == componentId).FirstAsync();
        _context.AssemblyComponents.Remove(assemblyComponent);
        await SaveChangesAsync();
    }
}
