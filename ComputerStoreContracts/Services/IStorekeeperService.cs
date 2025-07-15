using ComputerStoreModels.Models;
using ComputerStoreModels.Reports;

namespace ComputerStoreContracts.Services;

public interface IStorekeeperService
{
    // Component
    Task<Component?> GetComponentByIdAsync(Guid id);
    Task<IEnumerable<Component>> GetAllComponentsByUserIdAsync(Guid userId);
    Task CreateComponentAsync(Component component);
    Task UpdateComponentAsync(Component component);
    Task DeleteComponentAsync(Guid id);
    Task AttachToAssemblyAsync(Guid componentId, List<Guid> assemblyIds); // Привязка сборок к комплектующему
    Task DetachFromAssemblyAsync(Guid componentId, List<Guid> assemblyIds);

    // Product
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllProductsByUserIdAsync(Guid userId);
    Task CreateProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(Guid id);
    Task AddComponentsToProductAsync(Guid productId, List<Guid> componentIds); // Привязка комплектующих к товару
    Task RemoveComponentsFromProductAsync(Guid productId, List<Guid> componentIds);

    // Order
    Task<Order?> GetOrderByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetAllOrdersByUserIdAsync(Guid userId);
    Task CreateOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task DeleteOrderAsync(Guid id);
    Task AddProductsToOrderAsync(Guid orderId, List<Guid> productIds); // Привязка продуктов к заказу на товары
    Task RemoveProductsFromOrderAsync(Guid orderId, List<Guid> productIds);

    // Reports
    Task<byte[]> GenerateAssembliesByProductsReportAsync(List<Guid> productIds, string format);
    Task<byte[]> GenerateComponentDetailReportAsync(List<Guid> componentIds, DateTime startDate, DateTime endDate, string format);
    Task SendComponentDetailReportByEmailAsync(string email, List<Guid> componentIds, DateTime startDate, DateTime endDate);
    Task<List<ComponentDetailReport>> GetComponentDetailReportData(List<Guid> componentIds, DateTime startDate, DateTime endDate);

    // Auth
    Task<User?> LoginAsync(string login, string password);
    Task RegisterAsync(User user);
}
