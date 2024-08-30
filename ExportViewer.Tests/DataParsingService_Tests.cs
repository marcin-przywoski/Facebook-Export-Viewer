using System;
using System.IO;
using System.Threading.Tasks;
using ExportViewer.Core.Enums;
using ExportViewer.Core.Services;
using Xunit;
using Xunit.Abstractions;

namespace ExportViewer.Tests
{
    public class DataParsingService_Tests
    {
        private readonly DataParsingService _dataParsingService;

        private readonly ITestOutputHelper _output;

        private readonly IProgress<string> _progress = new Progress<string>(s => Console.WriteLine(s));

        public DataParsingService_Tests(ITestOutputHelper output)
        {
            _dataParsingService = new DataParsingService();
            _output = output;
        }

        [Fact]
        public async Task GetExportType_ReturnsJson()
        {
            // Arrange
            string tempDir = Path.Combine(Path.GetTempPath() , Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            string jsonFilePath = Path.Combine(tempDir , "test.json");
            if (!File.Exists(jsonFilePath))
            {
                await File.Create(jsonFilePath).DisposeAsync();
            }

            // Act
            ExportType result = await _dataParsingService.GetExportType(tempDir , _progress);

            // Assert
            Assert.Equal(ExportType.Json , result);
        }

        [Fact]
        public async Task GetExportType_ReturnsHtml()
        {
            // Arrange
            string tempDir = Path.Combine(Path.GetTempPath() , Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            string htmlFilePath = Path.Combine(tempDir , "test.html");
            if (!File.Exists(htmlFilePath))
            {
                await File.Create(htmlFilePath).DisposeAsync();
            }

            // Act
            ExportType result = await _dataParsingService.GetExportType(tempDir , _progress);

            // Assert
            Assert.Equal(ExportType.HTML , result);
        }

        [Theory]
        [InlineData("./TestData/GetExportLanguageXPath1")]
        [InlineData("./TestData/GetExportLanguageXPath2")]
        public async Task GetExportLanguage_Html_ReturnsCultureInfo(string exportLocation)
        {
            string fullPath = Path.Combine(Path.GetFullPath(exportLocation));
            var locale = await _dataParsingService.GetExportLanguage(fullPath , ExportType.HTML , _progress);

            Assert.Equal("en-US" , locale.Name);
        }

        [Fact]
        public async Task GetExportLanguage_Json_ReturnsCultureInfo()
        {
            string exportLocation = "./TestData/GetExportLanguageJson";
            string fullPath = Path.Combine(Path.GetFullPath(exportLocation));
            var locale = await _dataParsingService.GetExportLanguage(fullPath , ExportType.Json , _progress);

            Assert.Equal("en-US" , locale.Name);
        }
    }
}
