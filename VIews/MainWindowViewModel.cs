using System.Collections.ObjectModel;
using System.Windows.Input;
using Interfaces.Logic;
using Microsoft.Win32;
using Model;
using Views.Common;

namespace Views
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand OpenFileCommand => _openFileCommand ?? (_openFileCommand = new Command(OpenFile));

        public ObservableCollection<Account> Accounts { get; private set; }

        public MainWindowViewModel(IAccountsLogic accountsLogic)
        {
            _accountsLogic = accountsLogic;
        }

        private async void OpenFile(object param)
        {
            var ofd = new OpenFileDialog {Multiselect = false, Filter = "All files (*.*)|*.*"};
            
            if (ofd.ShowDialog() != true)
                return;

            _filePath = ofd.FileName;

            if (string.IsNullOrWhiteSpace(_filePath))
                return;

            var accounts = await _accountsLogic.GetAccounts(_filePath);

            Accounts = new ObservableCollection<Account>(accounts);
        }

        private ICommand _openFileCommand;

        private string _filePath;

        private IAccountsLogic _accountsLogic;
    }
}