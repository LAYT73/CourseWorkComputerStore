namespace ComputerStoreModels.Models;

public class Product : IUserOwnedEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<SaleProduct> SaleProducts { get; set; } = [];
    public ICollection<ProductComponent> ProductComponents { get; set; } = [];
    public ICollection<OrderProduct> OrderProducts { get; set; } = [];

    public User? User { get; set; }
    public Guid UserId { get; set; }
}
