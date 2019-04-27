using Model;

namespace Interfaces.Views
{
    /// <summary>
    /// Интерфейс редактора аккаунта.
    /// </summary>
    public interface IEditAccountWindowView : IView
    {
        /// <summary>
        /// Редактируемый аккаунт.
        /// </summary>
        Account EditingAccount { get; set; }
    }
}