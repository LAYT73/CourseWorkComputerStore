namespace ComputerStoreModels.Models;

public class AssemblyComponent
{
    public Guid Id { get; set; }

    public Guid AssemblyId { get; set; }
    public Assembly? Assembly { get; set; }

    public Guid ComponentId { get; set; }
    public Component? Component { get; set; }
}
