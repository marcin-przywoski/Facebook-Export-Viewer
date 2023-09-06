using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ExportViewer.Core.Enums;
using ExportViewer.Core.Models.Interfaces;
using ExportViewer.Core.Services.Interfaces;

namespace ExportViewer.Core.Services
{
    public class DataParsingService : IDataParsingService
    {
        public Task<IEnumerable<IMessage>> GetExportFileMessages (string exportLocation , string exportFileLocation , ExportType type , CultureInfo locale , IProgress<string> progress)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetExportFiles (string exportLocation , ExportType type , IProgress<string> progress)
        {
            throw new NotImplementedException();
        }

        public Task<CultureInfo> GetExportLanguage (string exportLocation , ExportType type , IProgress<string> progress)
        {
            throw new NotImplementedException();
        }

        public Task<ExportType> GetExportType (string exportLocation , IProgress<string> progress)
        {
            throw new NotImplementedException();
        }
    }
}
