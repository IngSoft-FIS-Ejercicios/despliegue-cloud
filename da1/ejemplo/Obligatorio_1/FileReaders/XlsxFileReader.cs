using ILogicImport;
namespace FileReaders;
using OfficeOpenXml;
using System.IO;

public class XlsxFileReader : IFileReaderStrategy
{
    public async Task<List<string[]>> ReadFile(Stream fileStream)
    {
        using var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var records = new List<string[]>();
        
        using var package = new ExcelPackage(memoryStream);
        var worksheet = package.Workbook.Worksheets[0];

        for (int row = 1; row <= worksheet.Dimension.Rows; row++)
        {
            var record = new string[5];
            record[0] = worksheet.Cells[row, 1].Text.Trim();
            record[1] = worksheet.Cells[row, 2].Text.Trim();
            record[2] = worksheet.Cells[row, 3].Text.Trim();
            record[3] = worksheet.Cells[row, 4].Text.Trim();
            record[4] = worksheet.Cells[row, 5].Text.Trim();

            records.Add(record);
        }

        return records;
    }
}