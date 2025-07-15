using ComputerStoreModels.Enums;

namespace ComputerStoreModels.Models;
    
public class User
{
    public Guid Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserType UserType { get; set; }

    // user type 0
    public ICollection<Sale> Sales { get; set; } = [];
    public ICollection<Assembly> Assemblies { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];

    // user type 1
    public ICollection<Component> Components { get; set; } = [];
    public ICollection<Product> Products { get; set; } = [];
    public ICollection<Order> Orders { get; set; } = [];
}
