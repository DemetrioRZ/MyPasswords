using System.Windows;
using Interfaces.ViewModels;
using Interfaces.Views;

namespace Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindowView
    {
        /// <summary>
        /// Модель представления вида основного окна.
        /// </summary>
        private readonly IMainWindowViewModel _mainWindowViewModel;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="mainWindowViewModel">Модель представления вида основного окна</param>
        public MainWindow(IMainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            
            _mainWindowViewModel = mainWindowViewModel;
            DataContext = _mainWindowViewModel;
        }

        /// <summary>
        /// Модель представления вида.
        /// </summary>
        public IViewModel ViewModel => _mainWindowViewModel;
    }
}
