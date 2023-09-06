using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using ExportViewer.Core.Enums;
using ExportViewer.Core.Models.Interfaces;

namespace ExportViewer.Core.Services.Interfaces
{
    public interface IDataParsingService
    {
        public Task<CultureInfo> GetExportLanguage(string exportLocation, ExportType type, IProgress<string> progress);

        public Task<IEnumerable<IMessage>> GetExportFileMessages(string exportLocation, string exportFileLocation, ExportType type, CultureInfo locale, IProgress<string> progress);

        public Task<ExportType> GetExportType(string exportLocation, IProgress<string> progress);

        public Task<IEnumerable<string>> GetExportFiles(string exportLocation, ExportType type, IProgress<string> progress);

    }
}
