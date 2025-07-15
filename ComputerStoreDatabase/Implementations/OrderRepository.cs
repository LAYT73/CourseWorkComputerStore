using ComputerStoreContracts.Repositories;
using ComputerStoreModels.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace ComputerStoreDatabase.Implementations;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context) { }

    public async Task AddProductToOrderAsync(Guid orderId, Guid productId)
    {
        await _context.OrderProducts.AddAsync(new OrderProduct { Id = Guid.NewGuid(), ProductId = productId, OrderId = orderId });
        await SaveChangesAsync();
    }

    public async Task RemoveProductFromOrderAsync(Guid orderId, Guid productId)
    {
        OrderProduct orderProduct = await _context.OrderProducts.Where(x => x.ProductId == productId && x.OrderId == orderId).FirstAsync();
        _context.OrderProducts.Remove(orderProduct);
        await SaveChangesAsync();
    }
}
