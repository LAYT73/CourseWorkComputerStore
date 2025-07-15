using ComputerStoreModels.Models;

namespace ComputerStoreContracts.Repositories;

public interface IComponentRepository : IRepository<Component>
{
    Task AttachToAssemblyAsync(Guid componentId, Guid assemblyId); // Привязка сборки к комплектующему
    Task DetachFromAssemblyAsync(Guid componentId, Guid assemblyId);
}
