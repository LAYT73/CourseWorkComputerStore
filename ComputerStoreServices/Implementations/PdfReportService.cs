using ComputerStoreModels.Reports;
using iText.Html2pdf;
using System.Text;

namespace ComputerStoreServices.Implementations;

public class PdfReportService
{
    public async Task<byte[]> GenerateComponentDetailReportAsync(IEnumerable<ComponentDetailReport> data)
    {
        var html = BuildComponentDetailReportHtml(data);
        return await GeneratePdfFromHtmlAsync(html);
    }

    private string BuildComponentDetailReportHtml(IEnumerable<ComponentDetailReport> data)
    {
        var sb = new StringBuilder();

        sb.Append("<h1 style='text-align:center;'>Отчет по комплектующим</h1>");
        sb.Append("<table border='1' width='100%'>");
        sb.Append("<tr><th>Название комплектующего</th><th>Покупка</th><th>Заказ</th><th>Дата отчета</th><th>Детали</th></tr>");

        foreach (var row in data)
        {
            sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>",
                row.ComponentName,
                row.PurchaseDescription,
                row.OrderDescription,
                row.ReportDate.ToShortDateString(),
                row.Details ?? "-"
            );
        }

        sb.Append("</table>");
        return sb.ToString();
    }

    private async Task<byte[]> GeneratePdfFromHtmlAsync(string htmlContent)
    {
        using var memoryStream = new MemoryStream();
        var converterProperties = new ConverterProperties();

        // Генерация PDF из HTML
        HtmlConverter.ConvertToPdf(htmlContent, memoryStream, converterProperties);

        return await Task.FromResult(memoryStream.ToArray());
    }
}
