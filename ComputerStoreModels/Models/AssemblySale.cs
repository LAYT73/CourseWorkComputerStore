namespace ComputerStoreModels.Models;

public class AssemblySale
{
    public Guid Id { get; set; }

    public Guid AssemblyId { get; set; }
    public required Assembly Assembly { get; set; }

    public Guid SaleId { get; set; }
    public required Sale Sale { get; set; }
}
