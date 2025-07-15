namespace ComputerStoreModels.Reports;

public class ComponentDetailReport
{
    public string ComponentName { get; set; } = string.Empty;
    public string PurchaseDescription { get; set; } = string.Empty;
    public string OrderDescription { get; set; } = string.Empty;
    public DateTime ReportDate { get; set; }
    public string? Details { get; set; }
}
