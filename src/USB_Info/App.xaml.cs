using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using USB_Info.Core;
using USB_Info.Core.Abstractions;
using USB_Info.MVVM.ViewModels;
using USB_Info.Services;
using USB_Info.Services.Abstractions;

namespace USB_Info
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        readonly ServiceProvider? _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider!.GetService<MainWindow>();
            mainWindow!.DataContext = GetMainViewModel();

            mainWindow.Show();
            base.OnStartup(e);
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<ILogicalHIDService, LogicalHIService>();
            services.AddSingleton<IPhysicalHIDService, PhysicalHIDService>();
            services.AddTransient<IUsbInfo, UsbInfo>();
            services.AddSingleton<IDeviceService, DeviceService>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>();
        }

        private MainViewModel GetMainViewModel()
        {
            var mainViewModel = _serviceProvider!.GetService<MainViewModel>();
            mainViewModel!.IsChecked = false;
            mainViewModel.CurrentViewModel = new LogicalHIDViewModel(_serviceProvider!.GetService<ILogicalHIDService>()!);

            return mainViewModel!;
        }
    }
}
