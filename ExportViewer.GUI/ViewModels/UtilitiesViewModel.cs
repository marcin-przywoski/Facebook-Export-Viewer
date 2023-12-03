using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.Input;
using ExportViewer.Core.Models.Interfaces;
using ExportViewer.Core.Services;
using ExportViewer.Core.Services.Interfaces;
using ExportViewer.GUI.Interfaces;
using ExportViewer.GUI.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAPICodePack.Dialogs;
using Message = ExportViewer.Core.Models.Common.Message;

namespace ExportViewer.GUI.ViewModels
{
    public partial class UtilitiesViewModel
    {
        private readonly IHtmlParsingService htmlParsingService = App.Current.serviceProvider.GetRequiredService<IHtmlParsingService>();
        private readonly IJsonParsingService jsonParsingService = App.Current.serviceProvider.GetRequiredService<IJsonParsingService>();
        private readonly IDataParsingService dataParsingService = App.Current.serviceProvider.GetRequiredService<IDataParsingService>();
        private readonly IDateEmbeddingService dateEmbeddingService = App.Current.serviceProvider.GetRequiredService<IDateEmbeddingService>();



        public UtilitiesViewModel ()
        {

        }

        public IUtilities Utilities { get; set; }

        public UtilitiesModel _utilitiesModel { get; set; } = new UtilitiesModel();

        [RelayCommand]
        async Task OnStart ()
        {
            if (String.IsNullOrEmpty(_utilitiesModel.SourcePath) || String.IsNullOrEmpty(_utilitiesModel.DestinationPath))
            {
                Utilities.ReportError("One of the paths have not been selected!");
            }
            else
            {
                var progress = Utilities.GetProgressObject();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                var sourceFolderPath = Path.Combine(_utilitiesModel.SourcePath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar);
                var destinationFolderPath = Path.Combine(_utilitiesModel.DestinationPath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar);

                var exportType = await dataParsingService.GetExportType(sourceFolderPath , progress);

                var exportLanguage = await dataParsingService.GetExportLanguage(destinationFolderPath , exportType , progress);

                var exportFiles = await dataParsingService.GetExportFiles(sourceFolderPath , exportType , progress);

                var messages = new List<Message>();

                foreach (var file in exportFiles)
                {
                    messages.AddRange(await Task.Run(() => dataParsingService.GetExportFileMessages(sourceFolderPath , file , exportType , exportLanguage , progress)));
                }

                await Task.WhenAll(messages.Select(x => Task.Run(() => dateEmbeddingService.EmbeddDate(x , sourceFolderPath , destinationFolderPath , progress))));

                stopwatch.Stop();

                var logPath = Path.Combine(destinationFolderPath , "log.txt");

                progress.Report($"Time elapsed total: {stopwatch.Elapsed:g}." + "\n" + $"Log saved to {logPath}");
                await Utilities.SaveLog(logPath);
            }
        }
    }
}

