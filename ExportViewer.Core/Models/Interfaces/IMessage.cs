using System;
using System.Collections.Generic;
using System.Text;

namespace ExportViewer.Core.Models.Interfaces
{
    public interface IMessage
    {
        public DateTime Date { get; set; }

        public string Link { get; set; }
    }
}
