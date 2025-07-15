using ClosedXML.Excel;
using ComputerStoreModels.Reports;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ComputerStoreServices.Implementations;

public class WordExcelReportService
{
    // Word: Список сборок по товарам (Кладовщик)
    public async Task<byte[]> GenerateAssemblyListByProductsWordAsync(IEnumerable<AssemblyByProductReportView> data)
    {
        var reportData = data;

        using var memoryStream = new MemoryStream();
        using var wordDoc = WordprocessingDocument.Create(memoryStream, WordprocessingDocumentType.Document);
        var mainPart = wordDoc.AddMainDocumentPart();
        mainPart.Document = new Document();
        var body = mainPart.Document.AppendChild(new Body());

        body.AppendChild(new Paragraph(new Run(new Text("Список сборок по товарам"))));

        var table = new Table();

        var headerRow = new TableRow();
        headerRow.Append(
            new TableCell(new Paragraph(new Run(new Text("Сборка")))),
            new TableCell(new Paragraph(new Run(new Text("Описание")))),
            new TableCell(new Paragraph(new Run(new Text("Дата создания")))),
            new TableCell(new Paragraph(new Run(new Text("Товар")))),
            new TableCell(new Paragraph(new Run(new Text("Компонент"))))
        );
        table.AppendChild(headerRow);

        foreach (var item in reportData)
        {
            var tr = new TableRow();
            tr.Append(
                new TableCell(new Paragraph(new Run(new Text(item.AssemblyName)))),
                new TableCell(new Paragraph(new Run(new Text(item.AssemblyDescription)))),
                new TableCell(new Paragraph(new Run(new Text(item.AssemblyCreatedAt.ToString())))),
                new TableCell(new Paragraph(new Run(new Text(item.ProductName)))),
                new TableCell(new Paragraph(new Run(new Text(item.ComponentName))))
            );
            table.AppendChild(tr);
        }

        body.AppendChild(table);
        wordDoc.Save();

        return await Task.FromResult(memoryStream.ToArray());
    }

    // Excel: Список сборок по товарам (Кладовщик)
    public async Task<byte[]> GenerateAssemblyListByProductsExcelAsync(IEnumerable<AssemblyByProductReportView> data)
    {
        var reportData = data;

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Список сборок по товарам");

        worksheet.Cell(1, 1).Value = "Сборка";
        worksheet.Cell(1, 2).Value = "Описание";
        worksheet.Cell(1, 3).Value = "Дата создания";
        worksheet.Cell(1, 4).Value = "Товар";
        worksheet.Cell(1, 5).Value = "Компонент";

        int row = 2;
        foreach (var item in reportData)
        {
            worksheet.Cell(row, 1).Value = item.AssemblyName;
            worksheet.Cell(row, 2).Value = item.AssemblyDescription;
            worksheet.Cell(row, 3).Value = item.AssemblyCreatedAt.ToString();
            worksheet.Cell(row, 4).Value = item.ProductName;
            worksheet.Cell(row, 5).Value = item.ComponentName;
            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return await Task.FromResult(stream.ToArray());
    }
}
