using ComputerStoreModels.Enums;

namespace ComputerStoreModels.Models;

public class Order : IUserOwnedEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public OrderType Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<OrderProduct> OrderProducts { get; set; } = [];

    public User? User { get; set; }
    public Guid UserId { get; set; }
}
