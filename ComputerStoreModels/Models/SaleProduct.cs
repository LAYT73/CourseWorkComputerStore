namespace ComputerStoreModels.Models;

public class SaleProduct
{
    public Guid Id { get; set; }
    
    public Guid SaleId { get; set; }
    public required Sale Sale { get; set; }

    public Guid ProductId { get; set; }
    public required Product Product { get; set; }
}
