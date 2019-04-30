using System.Windows;
using System.Windows.Threading;
using Autofac;
using Interfaces.Logic;
using Interfaces.Views;
using Logic;
using Views;

namespace MyPasswords
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Контейнер Autofac.
        /// </summary>
        private IContainer _container;

        /// <summary>
        /// Точка входа, вместо бутстраппера
        /// </summary>
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            SetupIoC();

            var mainWindow = _container.Resolve<IMainWindowView>();
            mainWindow.Show();
        }

        /// <summary>
        /// Обработка необработанных исключений в UI потоке.
        /// </summary>
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            var exceptionHelper = _container.Resolve<IExceptionHelper>();
            // todo: добавить запись в лог вместо окна с ошибкой
            MessageBox.Show(exceptionHelper.GetExceptionInfo(e.Exception), "UnhandledException", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Настройка DI IoC
        /// </summary>
        private void SetupIoC()
        {
            var builder = new ContainerBuilder();
            
            RegisterViews(builder);
            RegisterViewModels(builder);
            RegisterLogic(builder);
            
            _container = builder.Build();
        }

        /// <summary>
        /// Регистрация представлений.
        /// </summary>
        /// <param name="builder">ContainerBuilder Autofac</param>
        private void RegisterViews(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindow>().As<IMainWindowView>().InstancePerDependency();
            builder.RegisterType<EditAccountWindow>().As<IEditAccountWindowView>().InstancePerDependency();
            builder.RegisterType<WindowSizeRestorer>().SingleInstance();
        }

        /// <summary>
        /// Регистрация моделей представления.
        /// </summary>
        /// <param name="builder">ContainerBuilder Autofac</param>
        private void RegisterViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindowViewModel>().InstancePerDependency();
            builder.RegisterType<EditAccountWindowViewModel>().InstancePerDependency();
        }

        /// <summary>
        /// Регистрация классов бизнес логики.
        /// </summary>
        /// <param name="builder">ContainerBuilder Autofac</param>
        private void RegisterLogic(ContainerBuilder builder)
        {
            builder.RegisterType<ExceptionHelper>().As<IExceptionHelper>().InstancePerDependency();
            builder.RegisterType<EncryptDecryptLogic>().As<IEncryptDecryptLogic>().InstancePerDependency();
            builder.RegisterType<AccountsLogic>().As<IAccountsLogic>().InstancePerDependency();
            builder.RegisterType<AccountsSerializer>().As<IAccountsSerializer>().InstancePerDependency();
            builder.RegisterType<GzipArchiver>().As<IGzipArchiver>().InstancePerDependency();
        }

    }
}
