namespace ComputerStoreModels.Models;

public class Sale : IUserOwnedEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<AssemblySale> AssemblySales { get; set; } = [];
    public ICollection<SaleProduct> SaleProducts { get; set; } = [];

    public required User User { get; set; }
    public Guid UserId { get; set; }
}
