using System.ComponentModel;
using System.Windows;
using Interfaces.Views;

namespace Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindowView
    {
        /// <summary>
        /// Класс для безопасного восстановления и сохранения размеров окон в файле конфигурации приложения.
        /// </summary>
        private readonly WindowSizeRestorer _windowSizeRestorer;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="mainWindowViewModel">Модель представления вида основного окна</param>
        /// <param name="windowSizeRestorer">Класс для безопасного восстановления и сохранения размеров окон в файле конфигурации приложения.</param>
        public MainWindow(
            MainWindowViewModel mainWindowViewModel, 
            WindowSizeRestorer windowSizeRestorer)
        {
            InitializeComponent();
            
            DataContext = mainWindowViewModel;
            _windowSizeRestorer = windowSizeRestorer;

            _windowSizeRestorer.TryRestore(this);
        }

        /// <summary>
        /// Обработчик закрытия окна.
        /// </summary>
        private void OnClosing(object sender, CancelEventArgs e)
        {
            _windowSizeRestorer.TryStore(this);
        }
    }
}
