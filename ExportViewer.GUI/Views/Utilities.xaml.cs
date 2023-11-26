using System;
using System.Collections.Generic;
using System.Text;
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
        public Utilities ()
        {
            InitializeComponent();

            var utilitiesViewModel = DataContext as UtilitiesViewModel;

            if (utilitiesViewModel != null)
                utilitiesViewModel.Utilities = this;

        }

        public void SelectDestination()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok && dialog.FileName != null)
            {
                DestinationLocation.Text = dialog.FileName + "/";

            }
        }

        public void SelectSource()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok && dialog.FileName != null)
            {
                SourceLocation.Text = dialog.FileName + "/";
            }
        }

        public void ShowSourceLocation(string sourceLocation)
        {
            MessageBox.Show($"Source location is: {sourceLocation}");
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        private void StartButton_Click (object sender , RoutedEventArgs e)
        {

        }
    }
}
