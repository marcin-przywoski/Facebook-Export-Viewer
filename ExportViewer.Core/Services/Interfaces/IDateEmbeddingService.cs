﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ExportViewer.Core.Models.Common;
using ExportViewer.Core.Models.Interfaces;

namespace ExportViewer.Core.Services.Interfaces
{
    public interface IDateEmbeddingService
    {
        public Task EmbeddDate(Message message, string exportLocation, string destinationPath, IProgress<string> progress);
    }
}
