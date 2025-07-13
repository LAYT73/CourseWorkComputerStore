namespace ComputerStoreModels.Models;

public class Assembly : IUserOwnedEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<AssemblySale> AssemblySales { get; set; } = [];
    public ICollection<AssemblyComponent> AssemblyComponents { get; set; } = [];

    public required User User { get; set; }
    public Guid UserId { get; set; }
}
