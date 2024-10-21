using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using ExportViewer.Core.Models.Common;
using ExportViewer.Core.Services;
using Moq;
using Xunit;

namespace ExportViewer.Tests
{
    [ExcludeFromCodeCoverage]
    public class DateEmbeddingServiceTests
    {
        [Fact]
        public async Task EmbeddDate_FileExists_CopySuccessful ()
        {
            // Arrange
            var message = new Message { Link = $"{Guid.NewGuid()}.txt" , Date = DateTime.Now.AddDays(new Random().Next(1 , 100)) };
            var exportLocation = Path.GetTempPath();
            var destinationPath = Path.GetTempPath();
            var progressMock = new Mock<IProgress<string>>();

            // Ensure the file exists for the test
            var sourceFilePath = Path.Combine(exportLocation , message.Link);
            var destinationFilePath = Path.Combine(destinationPath , message.Link);
            Directory.CreateDirectory(exportLocation);
            Directory.CreateDirectory(destinationPath);
            File.WriteAllText(sourceFilePath , "Test content");

            var service = new DateEmbeddingService();

            // Act
            await service.EmbeddDate(message , exportLocation , destinationPath , progressMock.Object);

            // Assert
            Assert.True(File.Exists(destinationFilePath));
            Assert.Equal(File.GetCreationTime(destinationFilePath) , message.Date);
            Assert.Equal(File.GetLastAccessTime(destinationFilePath) , message.Date);
            Assert.Equal(File.GetLastWriteTime(destinationFilePath) , message.Date);

            // Cleanup
            File.Delete(sourceFilePath);
            File.Delete(destinationFilePath);
        }

        [Fact]
        public async Task EmbeddDate_FileDoesNotExist_ThrowsException ()
        {
            // Arrange
            var message = new Message { Link = $"{Guid.NewGuid()}.txt" , Date = DateTime.Now.AddDays(new Random().Next(1 , 100)) };
            var exportLocation = Path.GetTempPath();
            var destinationPath = Path.GetTempPath();
            var progressMock = new Mock<IProgress<string>>();

            var service = new DateEmbeddingService();

            // Act & Assert
            await Assert.ThrowsAsync<FileNotFoundException>(() => service.EmbeddDate(message , exportLocation , destinationPath , progressMock.Object));
        }

        [Fact]
        public async Task EmbeddDate_ProgressReported ()
        {
            // Arrange
            var message = new Message { Link = $"{Guid.NewGuid()}.txt" , Date = DateTime.Now.AddDays(new Random().Next(1 , 100)) };
            var exportLocation = Path.GetTempPath();
            var destinationPath = Path.GetTempPath();
            var progressMock = new Mock<IProgress<string>>();
            progressMock.Setup(p => p.Report(It.IsAny<string>())).Verifiable();

            // Ensure the file exists for the test
            var sourceFilePath = Path.Combine(exportLocation , message.Link);
            Directory.CreateDirectory(exportLocation);
            File.WriteAllText(sourceFilePath , "Test content");

            var service = new DateEmbeddingService();

            // Act
            await service.EmbeddDate(message , exportLocation , destinationPath , progressMock.Object);

            // Assert
            progressMock.Verify(p => p.Report(It.IsAny<string>()) , Times.AtLeastOnce);

            // Cleanup
            File.Delete(sourceFilePath);
        }

        [Fact]
        public async Task EmbeddDate_DestinationPathIsNull_ThrowsException ()
        {
            // Arrange
            var message = new Message { Link = $"{Guid.NewGuid()}.txt" , Date = DateTime.Now.AddDays(new Random().Next(1 , 100)) };
            var exportLocation = Path.GetTempPath();
            var progressMock = new Mock<IProgress<string>>();

            // Ensure the file exists for the test
            var sourceFilePath = Path.Combine(exportLocation , message.Link);
            Directory.CreateDirectory(exportLocation);
            File.WriteAllText(sourceFilePath , "Test content");

            var service = new DateEmbeddingService();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.EmbeddDate(message , exportLocation , null , progressMock.Object));
            File.Delete(sourceFilePath);
        }

        [Fact]
        public async Task EmbeddDate_DestinationPathIsEmpty_ThrowsException ()
        {
            // Arrange
            var message = new Message { Link = $"{Guid.NewGuid()}.txt" , Date = DateTime.Now.AddDays(new Random().Next(1 , 100)) };
            var exportLocation = Path.GetTempPath();
            var progressMock = new Mock<IProgress<string>>();

            // Ensure the file exists for the test
            var sourceFilePath = Path.Combine(exportLocation , message.Link);
            Directory.CreateDirectory(exportLocation);
            File.WriteAllText(sourceFilePath , "Test content");

            var service = new DateEmbeddingService();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.EmbeddDate(message , exportLocation , string.Empty , progressMock.Object));
            File.Delete(sourceFilePath);
        }
    }
}

