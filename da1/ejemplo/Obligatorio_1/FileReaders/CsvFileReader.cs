using ILogicImport;
namespace FileReaders;


public class CsvFileReader : IFileReaderStrategy
{
    public async Task<List<string[]>> ReadFile(Stream fileStream)
    {
        List<string[]> records = new List<string[]>();
        using var reader = new StreamReader(fileStream);

        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            records.Add(line.Split(','));
        }

        return records;
    }
}