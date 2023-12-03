using System;
using System.Collections.Generic;
using System.Text;
using ExportViewer.Core.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace ExportViewer.Core.Data
{
    public class ExportDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }



        public ExportDbContext (DbContextOptions<ExportDbContext> options)
            : base(options)
        {
        }

    }
}
