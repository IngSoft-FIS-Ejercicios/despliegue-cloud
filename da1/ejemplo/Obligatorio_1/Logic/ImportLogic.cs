using System.Globalization;
using Domain;
using FileReaders;
using ILogicImport;


namespace Logic;

public class ImportResult
{
    public string? Message { get; set; }
    public string? Error { get; set; }
}
public class ImportLogic
{
    private PanelLogic _panelLogic;
    private EpicLogic _epicLogic;
    
    public ImportLogic(PanelLogic panelLogic, EpicLogic epicLogic)
    {
        _panelLogic = panelLogic;
        _epicLogic = epicLogic;
    }

    public bool IsAValidFormat(string fileName)
    {
        try
        {
            ResolveStrategy(fileName);
            return true;
        }
        catch 
        {
            return false;
        }
    }
    public async Task<ImportResult> ProcessFile(User user, Stream fileStream, string fileName)
    {
        IFileReaderStrategy fileReader = ResolveStrategy(fileName);
        List<string[]> records = await fileReader.ReadFile(fileStream);
        int errorCounter = 0;

        foreach (var record in records)
        {
            if (!ProcessRecord(record, user))
            {
                errorCounter++;
            }
        }
        
        ImportResult result = GenerateImportResultMessage(errorCounter);
        return result;
    }

    private ImportResult GenerateImportResultMessage(int errorCounter)
    {
        ImportResult result = new ImportResult();
        if (errorCounter == 0)
        {
            result.Message = "The file was processed successfully!";
        }
        else if (errorCounter == 1)
        {
            result.Error = "There was 1 line that could not be processed due to errors.";
        }
        else
        {
            result.Error = $"There was {errorCounter} line that could not be processed due to errors.";
        }

        return result;
    }
    
    private IFileReaderStrategy ResolveStrategy(string fileName)
    {
        if (IsAFileType("csv",fileName))
        {
            return new CsvFileReader();
        }
        else if (IsAFileType("xlsx",fileName))
        {
            return new XlsxFileReader();
        }
        else
        {
            throw new NotSupportedException($"Unsupported file format: {fileName}");
        }
    }

    private bool IsAFileType(string type,string fileName)
    {
        return fileName.EndsWith("." + type, StringComparison.OrdinalIgnoreCase);
    }
    
    private bool ProcessRecord(string[] record, User user)
    {
        if (record.Length != 4 && record.Length != 5)
        {
            LogError($"Invalid input: {record.Length} parts instead of 4 or 5.", string.Join(",", record), user);
            return false;
        }

        try
        {
            if (IsHeader(record)) return true;
            
            int panelId = int.Parse(record[3].Trim());
            CheckPanelIdIsNotReserved(panelId);
            
            _panelLogic.CreateTask(_panelLogic.findPanelById(panelId), CreatePanelTaskFromRecord(record));
            
            return true;
        }
        catch (Exception ex)
        {
            LogError(ex.Message, string.Join(",", record), user);
            return false;
        }
    }

    private static bool IsHeader(string[] record)
    {
        return record[0].Trim() == "Título" && record[1].Trim() == "Descripción" && record[2].Trim() == "Fecha de vencimiento";
    }
    public void CheckPanelIdIsNotReserved(int panelId)
    {
        const int oudatedTasksPanelId = 1;
        if (panelId == oudatedTasksPanelId)
        {
            throw new InvalidOperationException("Id cannot be 1, that's the Id for OutdatedTasksPanel");
        }
        
    }
    
    // Using SemaphoreSlim to prevent tasks from overlapping when writing to the log file.
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private async Task LogError(string errorMessage, string csvLineWithError, User user)
    {
        await _semaphore.WaitAsync();

        string logMessage = GenerateErrorMessageForImport(errorMessage, csvLineWithError);
        await File.AppendAllTextAsync(GetErrorImportPath(user), logMessage);

        _semaphore.Release();

    }
    private PanelTask CreatePanelTaskFromRecord(string[] record)
    {
        PanelTask task = new PanelTask(new DateTimeProvider())
        {
            Title = record[0].Trim(),
            Description = record[1].Trim(),
            DueDate = DateTime.ParseExact(record[2].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture) 
        };
            
        if (record.Length == 5)
        {
            task.Epic = _epicLogic.FindEpicById(int.Parse(record[4].Trim()));
        }

        return task;
    }
    
    private static string GetProjectDirectory()
    {
        string currentPath = Directory.GetCurrentDirectory();
        string projectPath = Directory.GetParent(currentPath).FullName;
        return projectPath;
    }

    public string GetErrorImportPath(User user)
    {
        return Path.Combine(GetProjectDirectory(), $"ErroresImport-{user.Name}-{user.Id}.txt");
    }
    
    private static string GenerateErrorMessageForImport(string errorMessage, string csvLineWithError)
    {
        DateTime utcMinus3 = DateTime.UtcNow.AddHours(-3);
        return $"Date: {utcMinus3:O} | Line: {csvLineWithError} | Error: {errorMessage}{Environment.NewLine}";
    }

}