using ComputerStoreModels.Models;

namespace ComputerStoreContracts.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task AddComponentToProductAsync(Guid productId, Guid componentId); // Привязка комплектующего к товару
    Task RemoveComponentFromProductAsync(Guid productId, Guid componentId);
}
