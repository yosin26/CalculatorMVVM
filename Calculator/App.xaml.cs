using System;
using System.Windows;
using Calculator.Model;
using Calculator.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Настроить службы
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Установить DataContext для MainWindow
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = ServiceProvider.GetRequiredService<CalculatorViewModel>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Регистрируем модель, ViewModel и главное окно
            services.AddSingleton<CalculatorModel>();
            services.AddSingleton<CalculatorViewModel>();
            services.AddSingleton<MainWindow>();
        }
    }
}
