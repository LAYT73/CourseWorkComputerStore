using ComputerStoreModels.Models;

namespace ComputerStoreContracts.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task AddProductToOrderAsync(Guid orderId, Guid productId); // Привязка продукта к заказу на товары
    Task RemoveProductFromOrderAsync(Guid orderId, Guid productId);
}
