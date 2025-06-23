using Domain;
using Logic;

using Repository;
using RepositoryTests.Context;

namespace LogicTest
{
    [TestClass]
    public class ImportLogicTest
    {
        private ImportLogic _importLogic;
        private PanelLogic _panelLogic;
        private User _user;
        private Panel _panel;
        private Panel _panel2;
        private Epic _epic;
        private EpicLogic _epicLogic; 
        private string _testCsvFilePath;
        private string _xlsxFilePath;
        string errorImportPath;
        private RepositoryDBPanel _repositoryDbPanel;
        private RepositoryDBPanelTask _repositoryDbPanelTask;
        private RepositoryDBEpic _repositoryDbEpic;
        private SqlContext _context;
        
        
        [TestInitialize]
        public void Setup()
        {
            _context = SqlContextFactory.CreateMemoryContext();
            _context.Database.EnsureDeleted();
            _repositoryDbEpic = new RepositoryDBEpic(_context);
            _repositoryDbPanel = new RepositoryDBPanel(_context);
            _repositoryDbPanelTask = new RepositoryDBPanelTask(_context);
            
            _panelLogic = new PanelLogic(_repositoryDbPanel,_repositoryDbPanelTask);
            _epicLogic = new EpicLogic(_repositoryDbEpic);
            
            _importLogic = new ImportLogic(_panelLogic, _epicLogic);
            
            _testCsvFilePath = Path.Combine(Path.GetTempPath(), "test_obligatorio1_data.csv");
            _xlsxFilePath = Path.Combine(Path.GetTempPath(), "test_obligatorio1_data.xlsx");
            
            _user = new User()
            {
                Id = 0, 
                Name = "Tester",
                Surname = "Lopez",
                Email = "tester@gmail.com",
                Type = TypeUser.Admin,
                BirthDate = new DateTime(2024, 10, 20),
                Password = "Admin20#"
            };
            _panel = new Panel()
            {
                Name = "Outdated Tasks",
                Creator = new User(),
                Team = new Team(),
                Description = "Panel that contains outdated tasks",
                PanelTaskList = new List<PanelTask> { }
            };
            _panel2 = new Panel()
            {
                Name = "Outdated Tasks",
                Creator = new User(),
                Team = new Team(),
                Description = "Panel that contains outdated tasks",
                PanelTaskList = new List<PanelTask> { }
            };
            _epic = new Epic(new DateTimeProvider())
            {
                Title = "Epic 1",
                Description = "This is an epic",
                Priority = "Medium",
                DueDate = DateTime.Now.AddDays(7)
            };
            
            _epicLogic.CreateEpic(_epic);
            _panelLogic.CreatePanel(_panel);
            _panelLogic.CreatePanel(_panel2);
            errorImportPath = _importLogic.GetErrorImportPath(_user);
        }

        [TestCleanup]
        public void cleanup()
        {
            DeleteFile(_testCsvFilePath);
            DeleteFile(errorImportPath);
            DeleteFile(_xlsxFilePath);
        }
        
        public void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        private async Task AddLineToTestCsv(string line)
        {
            await using (var writer = new StreamWriter(_testCsvFilePath, append: true))
            {
                await writer.WriteLineAsync(line);
            }
        }
        private void CreateXlsxFile(string filePath, List<string[]> rows)
        {
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sheet1");

                for (int i = 0; i < rows.Count; i++)
                {
                    for (int j = 0; j < rows[i].Length; j++)
                    {
                        worksheet.Cell(i + 1, j + 1).Value = rows[i][j];
                    }
                }

                workbook.SaveAs(filePath);
            }
        }

        [TestMethod]
        public void IsValidFormatOk()
        {
            Assert.IsTrue(_importLogic.IsAValidFormat("valid.csv"));
        }
        
        [TestMethod]
        public void IsValidFormatError()
        {
            Assert.IsFalse(_importLogic.IsAValidFormat("valid.psd"));
        }
        
        [TestMethod]
        public async Task ProcessXlsxFileOk()
        {
            var rows = new List<string[]>
            {
                new[] { "Título", "Descripción", "Fecha de vencimiento", "ID de panel" },
                new[] { "Archivo Importado Titulo1", "Descripcion 1", "15/10/2060", "2", "1" },
                new[] { "Archivo Importado Titulo2", "Descripcion 2", "15/10/2060", "2"},
                new[] { "Archivo Importado Titulo3", "Descripcion 3", "15/10/2060", "2"}
            };
            CreateXlsxFile(_xlsxFilePath, rows);

            Stream testFile = null;
            try
            {
                testFile = File.OpenRead(_xlsxFilePath);
                await _importLogic.ProcessFile(_user, testFile, _xlsxFilePath);
            }
            finally
            {
                testFile?.Dispose();
            }

            Assert.AreEqual(1, _panelLogic.GetTasks().Count);
        }
        [TestMethod]
        public async Task ProsseceFileOk()
        {
            
            await AddLineToTestCsv("Archivo Importado Titulo1, Descripcion 1, 15/10/2060, 2");
            await AddLineToTestCsv("Archivo Importado Titulo2, Descripcion 2, 15/10/2060, 2,1");
            await AddLineToTestCsv("Archivo Importado Titulo3, Descripcion 3, 15/10/2060, 2");
            await using Stream testFile = File.OpenRead(_testCsvFilePath);
            await _importLogic.ProcessFile(_user, testFile, _testCsvFilePath);
            
            Assert.AreEqual(3,_panelLogic.GetTasks().Count);
            
        }
        
        [TestMethod]
        public async Task ProsseceFileInvalidId()
        {
            
            await AddLineToTestCsv("Archivo Importado Titulo1, Descripcion 1, 15/10/2060, 1");
            await using Stream testFile = File.OpenRead(_testCsvFilePath);
            await _importLogic.ProcessFile(_user, testFile, _testCsvFilePath);
            
            Assert.IsTrue(File.Exists(errorImportPath));
            
        }
        [TestMethod]
        public async Task ProsseceFileInvalidDate()
        {
            
            await AddLineToTestCsv("Archivo Importado Titulo1, Descripcion 1, 15/10/20, 1");
            await using Stream testFile = File.OpenRead(_testCsvFilePath);
            await _importLogic.ProcessFile(_user, testFile, _testCsvFilePath);
            
            Assert.IsTrue(File.Exists(errorImportPath));
            
        }
        [TestMethod]
        public async Task ProsseceFileInvalidTitle()
        {
            
            await AddLineToTestCsv(", Descripcion 1, 15/10/2060, 1");
            await using Stream testFile = File.OpenRead(_testCsvFilePath);
            await _importLogic.ProcessFile(_user, testFile, _testCsvFilePath);
            
            Assert.IsTrue(File.Exists(errorImportPath));
            
        }
        [TestMethod]
        public async Task ProsseceFileInvalidParameters()
        {
            
            await AddLineToTestCsv("Archivo Importado Titulo1,,, Descripcion 1, 15/10/2060, 1");
            await using Stream testFile = File.OpenRead(_testCsvFilePath);
            await _importLogic.ProcessFile(_user, testFile, _testCsvFilePath);
            
            Assert.IsTrue(File.Exists(errorImportPath));
            
        }
        [TestMethod]
        public async Task ProsseceFileIdNotFound()
        {
            
            await AddLineToTestCsv("Archivo Importado Titulo1, Descripcion 1, 15/10/2060, 50");
            await using Stream testFile = File.OpenRead(_testCsvFilePath);
            await _importLogic.ProcessFile(_user, testFile, _testCsvFilePath);
            
            Assert.IsTrue(File.Exists(errorImportPath));
            
        }
        
        [TestMethod]
        public async Task ProsseceFileMultipleErrors()
        {
            
            await AddLineToTestCsv("Archivo Importado Titulo1, Descripcion 1, 15/10/2060, 50");
            await AddLineToTestCsv("Archivo Importado Titulo1, Descripcion 1,, 50");
            await AddLineToTestCsv(", Descripcion 1, 15/10/2060, 50");
            await using Stream testFile = File.OpenRead(_testCsvFilePath);
            await _importLogic.ProcessFile(_user, testFile, _testCsvFilePath);
            Thread.Sleep(20);
            Assert.IsTrue(File.Exists(errorImportPath));
            
        }
        
        [TestMethod]
        public async Task ProsseceFileMessageOk()
        {
            
            await AddLineToTestCsv("Archivo Importado Titulo1, Descripcion 1, 15/10/2060, 2");
            await using Stream testFile = File.OpenRead(_testCsvFilePath);
            var result = await _importLogic.ProcessFile(_user, testFile, _testCsvFilePath);
            
            Assert.AreEqual("The file was processed successfully!",result.Message);
            
        }
        [TestMethod]
        public async Task ProsseceFileMessageErrorOk()
        {
            
            await AddLineToTestCsv("Archivo Importado Titulo1, Descripcion 1, 15/10/2060, 5");
            await using Stream testFile = File.OpenRead(_testCsvFilePath);
            var result = await _importLogic.ProcessFile(_user, testFile, _testCsvFilePath);
            
            Assert.AreEqual("There was 1 line that could not be processed due to errors.",result.Error);
            
        }
    }
}
