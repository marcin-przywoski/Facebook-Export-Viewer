using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using ExportViewer.Core.Models.Common;
using ExportViewer.Core.Models.Interfaces;

namespace ExportViewer.Core.Services.Interfaces
{
    public interface IHtmlParsingService
    {
        public Task<IEnumerable<Message>> GetMessages(string filePath, CultureInfo locale, string exportLocation);
    }
}
