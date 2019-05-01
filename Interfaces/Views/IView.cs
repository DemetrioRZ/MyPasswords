namespace Interfaces.Views
{
    /// <summary>
    /// Общий интерфейс для окон.
    /// </summary>
    public interface IView
    {
        object DataContext { get; }

        void Show();

        bool? ShowDialog();

        void Close();

        bool? DialogResult { get; set; }
    }
}