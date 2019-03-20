using Interfaces.ViewModels;

namespace Interfaces.Views
{
    public interface IView
    {
        IViewModel ViewModel { get; }

        void Show();

        bool? ShowDialog();
    }
}