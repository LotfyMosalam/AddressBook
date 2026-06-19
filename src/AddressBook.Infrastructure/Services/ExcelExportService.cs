using AddressBook.Application.AddressEntries.DTOs;
using AddressBook.Application.Common.Interfaces;
using ClosedXML.Excel;

namespace AddressBook.Infrastructure.Services;

public class ExcelExportService : IExcelExportService
{
    public byte[] ExportAddressEntries(IReadOnlyList<AddressEntryListItemDto> entries)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Address Book");

        // Header row
        var headers = new[] { "Full Name", "Job", "Department", "Mobile Number", "Email", "Age", "Photo URL" };
        for (var i = 0; i < headers.Length; i++)
        {
            var cell = worksheet.Cell(1, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
        }

        // Data rows
        for (var row = 0; row < entries.Count; row++)
        {
            var entry = entries[row];
            var excelRow = row + 2;

            worksheet.Cell(excelRow, 1).Value = entry.FullName;
            worksheet.Cell(excelRow, 2).Value = entry.JobName;
            worksheet.Cell(excelRow, 3).Value = entry.DepartmentName;
            worksheet.Cell(excelRow, 4).Value = entry.MobileNumber;
            worksheet.Cell(excelRow, 5).Value = entry.Email;
            worksheet.Cell(excelRow, 6).Value = entry.Age;
            worksheet.Cell(excelRow, 7).Value = entry.PhotoUrl ?? string.Empty;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
