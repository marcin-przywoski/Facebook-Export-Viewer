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
        [InlineData("/html/body/div/div/div/div[2]/div[2]/div/div[3]/div/div[2]/div[1]/div[2]/div/div/div/div[1]/div[3]" , "about_you/preferences.html")]
        [InlineData("/html/body/div/div/div/div[2]/div[2]/div/div[1]/div/div[2]/div[1]/div[2]/div/div/div/div[1]/div[3]" , "preferences/language_and_locale.html")]
        public async Task GetExportLanguage_HtmlFromXPath_ReturnsCultureInfo(string xPath , string folderScheme)
        {
            string htmlString = HelperMethods.Html.GenerateLanguageHtml(xPath);
            string exportPath = Path.Combine(Path.GetTempPath() , Guid.NewGuid().ToString());
            string fullPath = Path.Combine(exportPath , folderScheme);
            string? dir = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.WriteAllText(fullPath , htmlString);

            var locale = await _dataParsingService.GetExportLanguage(exportPath , ExportType.HTML , _progress);

            Assert.Equal("en-US" , locale.Name);
            _output.WriteLine($"Export language: {locale.Name} \n Export HTML string: {htmlString}");

            //Cleanup
            Directory.Delete(exportPath , true);
        }

        [Theory]
        [InlineData("language_and_locale_v2[0].children[0].entries[0].data.value","preferences/language_and_locale.json")]
        public async Task GetExportLanguage_Json_ReturnsCultureInfo(string jsonPath, string folderScheme)
        {
            string jsonString = HelperMethods.Json.GenerateLanguageJson(jsonPath);
                        string exportPath = Path.Combine(Path.GetTempPath() , Guid.NewGuid().ToString());
            string fullPath = Path.Combine(exportPath , folderScheme);
            string? dir = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.WriteAllText(fullPath , jsonString);

            var locale = await _dataParsingService.GetExportLanguage(exportPath , ExportType.Json , _progress);

            Assert.Equal("en-US" , locale.Name);
            _output.WriteLine($"Export language: {locale.Name} \n Export JSON string: {jsonString}");

            //Cleanup
            Directory.Delete(exportPath , true);
        }
    }
}
