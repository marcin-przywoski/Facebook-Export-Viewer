using System;
using System.Collections.Generic;
using System.IO;
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
using ExportViewer.GUI.Interfaces;
using ExportViewer.GUI.ViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ExportViewer.GUI.Views
{
    /// <summary>
    /// Interaction logic for Utilities.xaml
    /// </summary>
    public partial class Utilities : UserControl, IUtilities
    {
        private Progress<string> _progress;

        public Utilities ()
        {
            InitializeComponent();

            var utilitiesViewModel = DataContext as UtilitiesViewModel;

            if (utilitiesViewModel != null)
                utilitiesViewModel.Utilities = this;

            _progress = new Progress<string>(x => { OutputLog.AppendText(x + "\n"); OutputLog.ScrollToEnd(); });

        }


        public Task ReportError (string message)
            {
            MessageBox.Show(message, "Alert" , MessageBoxButton.OK, MessageBoxImage.Error);
            return Task.CompletedTask;
            }

        public Task ReportMessage (string message)
        {
            MessageBox.Show(message);
            return Task.CompletedTask;
            }


        public async Task SaveLog (string path)
        {
            await File.WriteAllTextAsync(path , OutputLog.Text);
        }

       public IProgress<string> GetProgressObject ()
        {
            return _progress;
        }
    }
}
