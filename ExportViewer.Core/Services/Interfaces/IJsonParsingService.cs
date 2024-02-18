using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using ExportViewer.Core.Models.Interfaces;
using ExportViewer.Core.Models.Common;

namespace ExportViewer.Core.Services.Interfaces
{
    public interface IJsonParsingService
    {
        public Task<IEnumerable<Message>> GetMessages(string filePath, CultureInfo locale, string exportLocation);
    }
}
