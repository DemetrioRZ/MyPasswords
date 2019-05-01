using System.ComponentModel;
using System.Windows;
using Interfaces.Views;
using Views.Common;

namespace Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView
    {
        /// <summary>
        /// Класс для безопасного восстановления и сохранения размеров окон в файле конфигурации приложения.
        /// </summary>
        private readonly WindowSizeRestorer _windowSizeRestorer;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="mainViewModel">Модель представления вида основного окна</param>
        /// <param name="windowSizeRestorer">Класс для безопасного восстановления и сохранения размеров окон в файле конфигурации приложения.</param>
        public MainWindow(
            MainViewModel mainViewModel, 
            WindowSizeRestorer windowSizeRestorer)
        {
            InitializeComponent();
            
            DataContext = mainViewModel;
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
