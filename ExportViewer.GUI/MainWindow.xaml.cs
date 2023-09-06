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

namespace ExportViewer.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

        private void StartButton_Click (object sender , RoutedEventArgs e)
        {

        }

        private void DestinationLocation_Click (object sender , RoutedEventArgs e)
        {

        }

        private void SourceLocation_Click (object sender , RoutedEventArgs e)
        {

        }
    }
}
