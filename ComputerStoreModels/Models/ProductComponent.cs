namespace ComputerStoreModels.Models;

public class ProductComponent
{
    public Guid Id { get; set; }

    public Guid ComponentId { get; set; }
    public required Component Component { get; set; }

    public Guid ProductId { get; set; }
    public required Product Product { get; set; }
}
