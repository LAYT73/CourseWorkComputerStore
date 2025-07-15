namespace ComputerStoreModels.Reports;

public class AssemblyByProductReportView
{
    public Guid AssemblyId { get; set; }
    public string AssemblyName { get; set; } = string.Empty;
    public string AssemblyDescription { get; set; } = string.Empty;
    public DateTime AssemblyCreatedAt { get; set; }

    public Guid ComponentId { get; set; }
    public string ComponentName { get; set; } = string.Empty;
    public decimal ComponentPrice { get; set; }

    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
}
