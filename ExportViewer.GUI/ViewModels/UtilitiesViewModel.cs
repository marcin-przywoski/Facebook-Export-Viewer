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

namespace ExportViewer.GUI.ViewModels
{
    public partial class UtilitiesViewModel
    {
        private readonly IHtmlParsingService htmlParsingService = App.Current.serviceProvider.GetRequiredService<IHtmlParsingService>();
        private readonly IJsonParsingService jsonParsingService = App.Current.serviceProvider.GetRequiredService<IJsonParsingService>();
        private readonly IDataParsingService dataParsingService = App.Current.serviceProvider.GetRequiredService<IDataParsingService>();
        private readonly IDateEmbeddingService dateEmbeddingService = App.Current.serviceProvider.GetRequiredService<IDateEmbeddingService>();



        public UtilitiesViewModel()
        {

        }

        public IUtilities Utilities { get; set; }

        public UtilitiesModel _utilitiesModel { get; set; } = new UtilitiesModel ();

        [RelayCommand]
       async void OnStart()
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



                        var exportType = await dataParsingService.GetExportType(_utilitiesModel.SourcePath, progress);

                        var exportLanguage = await dataParsingService.GetExportLanguage(_utilitiesModel.DestinationPath , exportType, progress);

                        var exportFiles = await dataParsingService.GetExportFiles(_utilitiesModel.SourcePath , exportType, progress);

                        var messages = new List<IMessage>();

                foreach (var file in exportFiles)
                {
                    messages.AddRange(await Task.Run(() => dataParsingService.GetExportFileMessages(_utilitiesModel.SourcePath , file , exportType , exportLanguage , progress)));
                }

                await Task.WhenAll(messages.Select(x => Task.Run(() => dateEmbeddingService.EmbeddDate(x , _utilitiesModel.SourcePath , _utilitiesModel.DestinationPath , progress))));

                stopwatch.Stop();
                progress.Report($"Time elapsed total: {stopwatch.Elapsed:g}." + "\n" + $"Log saved to {_utilitiesModel.DestinationPath} log.txt");
               await Utilities.SaveLog(_utilitiesModel.DestinationPath + "log.txt");
            }
        }
    }
}
