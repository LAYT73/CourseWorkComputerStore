namespace ComputerStoreModels.Models;

public class AssemblySale
{
    public Guid Id { get; set; }

    public Guid AssemblyId { get; set; }
    public Assembly? Assembly { get; set; }

    public Guid SaleId { get; set; }
    public Sale? Sale { get; set; }
}
