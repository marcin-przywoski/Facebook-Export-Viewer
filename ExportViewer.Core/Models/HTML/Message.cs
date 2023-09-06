using System;
using System.Collections.Generic;
using System.Text;
using ExportViewer.Core.Models.Interfaces;

namespace ExportViewer.Core.Models.HTML
{
    public class Message : IMessage
    {
        public DateTime Date { get; set; }

        public string Link { get; set; }
    }
}
