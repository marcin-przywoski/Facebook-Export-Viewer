

using CommunityToolkit.Mvvm.ComponentModel;

namespace ExportViewer.GUI.Models
{
   public class UtilitiesModel : ObservableObject
    {
        public string SourcePath { get; set; }

        public string SourcePathLabel { get; set; }

        public string DestinationPath { get; set; }

        public string DestinationPathLabel { get; set; }

        public string OutputLog { get; set; }
    }
}