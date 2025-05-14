namespace ILogicImport;

public interface IFileReaderStrategy
{
    Task<List<string[]>> ReadFile(Stream fileStream);
}