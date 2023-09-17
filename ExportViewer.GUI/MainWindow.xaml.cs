using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExportViewer.Core.Models.Interfaces;
using ExportViewer.Core.Services.Interfaces;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ExportViewer.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string exportLocation;
        string destination;

        private readonly IHtmlParsingService htmlParsingService;
        private readonly IJsonParsingService jsonParsingService;
        private readonly IDataParsingService dataParsingService;
        private readonly IDateEmbeddingService dateEmbeddingService;

        public MainWindow(IDateEmbeddingService dateEmbeddingService, IDataParsingService dataParsingService, IHtmlParsingService htmlParsingService, IJsonParsingService jsonParsingService)
        {
            InitializeComponent();
            this.dateEmbeddingService = dateEmbeddingService;
            this.dataParsingService = dataParsingService;
            this.htmlParsingService = htmlParsingService;
            this.jsonParsingService = jsonParsingService;
        }

        private void DestinationLocation_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok && dialog.FileName != null)
            {
                DestinationLocationLabel.Content = "Destination location : ";
                destination = dialog.FileName + "/";
                DestinationLocationLabel.Content += destination;
                DestinationLocationLabel.ToolTip = DestinationLocationLabel.Content;
            }

        }

        private void SourceLocation_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok && dialog.FileName != null)
            {
                SourceLocationLabel.Content = "Source location : ";
                exportLocation = dialog.FileName + "/";
                SourceLocationLabel.Content += exportLocation;
                SourceLocationLabel.ToolTip = SourceLocationLabel.Content;
            }

        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var progress = new Progress<string>(x => { OutputLog.AppendText(x + "\n"); OutputLog.ScrollToEnd(); });


            if (String.IsNullOrEmpty(destination) || String.IsNullOrEmpty(exportLocation))
            {
                MessageBox.Show("One of the paths have not been selected!");
        }
            else
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();


                var exportType = await dataParsingService.GetExportType(exportLocation, progress);

                var exportLanguage = await dataParsingService.GetExportLanguage(exportLocation, exportType, progress);

                var exportFiles = await dataParsingService.GetExportFiles(exportLocation, exportType, progress);

                var messages = new List<IMessage>();

                foreach (var file in exportFiles)
                {
                    messages.AddRange(await Task.Run(() => dataParsingService.GetExportFileMessages(exportLocation, file, exportType, exportLanguage, progress)));
                }

                await Task.WhenAll(messages.Select(x => Task.Run(() => dateEmbeddingService.EmbeddDate(x, exportLocation, destination, progress))));

                stopwatch.Stop();
                OutputLog.AppendText($"Time elapsed total: {stopwatch.Elapsed:g}." + "\n" + $"Log saved to {destination} log.txt");
                await File.WriteAllTextAsync(destination + "log.txt", OutputLog.Text);
            }
        }
    }
}
