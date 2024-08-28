using System;
using System.IO;
using System.Threading.Tasks;
using ExportViewer.Core.Enums;
using ExportViewer.Core.Services;
using Xunit;

namespace ExportViewer.Tests
{
    public class DataParsingService_Tests
    {
        private readonly DataParsingService _dataParsingService;

        private readonly IProgress<string> _progress = new Progress<string>(s => Console.WriteLine(s));

        public DataParsingService_Tests ()
        {
            _dataParsingService = new DataParsingService();
        }

        [Fact]
        public async Task GetExportType_ReturnsJson ()
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
        public async Task GetExportType_ReturnsHtml ()
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
    }
}
