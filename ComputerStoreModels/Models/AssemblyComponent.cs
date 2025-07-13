namespace ComputerStoreModels.Models;

public class AssemblyComponent
{
    public Guid Id { get; set; }

    public Guid AssemblyId { get; set; }
    public required Assembly Assembly { get; set; }

    public Guid ComponentId { get; set; }
    public required Component Component { get; set; }
}
