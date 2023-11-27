using System;
using System.Threading.Tasks;

namespace ExportViewer.GUI.Interfaces
{
    public interface IUtilities
    {
        public Task ReportError(string message);

        public Task ReportMessage(string message);

        public IProgress<string> GetProgressObject();

        public Task SaveLog (string path);
    }
}