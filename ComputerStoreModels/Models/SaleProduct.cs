namespace ComputerStoreModels.Models;

public class SaleProduct
{
    public Guid Id { get; set; }
    
    public Guid SaleId { get; set; }
    public Sale? Sale { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
}
