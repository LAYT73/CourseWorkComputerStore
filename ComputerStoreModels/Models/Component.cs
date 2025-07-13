namespace ComputerStoreModels.Models;

public class Component : IUserOwnedEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public ICollection<AssemblyComponent> AssemblyComponents { get; set; } = [];
    public ICollection<ProductComponent> ProductComponents { get; set; } = [];

    public required User User { get; set; }
    public Guid UserId { get; set; }
}
