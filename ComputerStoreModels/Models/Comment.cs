namespace ComputerStoreModels.Models;

public class Comment : IUserOwnedEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Assembly? Assembly { get; set; }
    public Guid? AssemblyId { get; set; }

    public User? User { get; set; }
    public Guid UserId { get; set; }
}
