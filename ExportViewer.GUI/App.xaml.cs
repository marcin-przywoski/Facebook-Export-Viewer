using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ExportViewer.Core.Services;
using ExportViewer.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExportViewer.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }


        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<IHtmlParsingService, HtmlParsingService>();
            services.AddSingleton<IJsonParsingService, JsonParsingService>();
            services.AddSingleton<IDataParsingService, DataParsingService>();
            services.AddSingleton<IDateEmbeddingService, DateEmbeddingService>();

        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
