using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Interfaces.Logic;
using Microsoft.Win32;
using Model;
using Views.Common;

namespace Views
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ICommand _newFileCommand;

        private ICommand _openFileCommand;

        private ICommand _saveFileCommand;

        private ICommand _createAccountCommand;

        private ICommand _editAccountCommand;

        private string _filePath;

        private ObservableCollection<Account> _accounts;

        private readonly IAccountsLogic _accountsLogic;

        public ICommand NewFileCommand => _newFileCommand ?? (_newFileCommand = new Command(NewFile));

        public ICommand OpenFileCommand => _openFileCommand ?? (_openFileCommand = new Command(OpenFile));

        public ICommand SaveFileCommand => _saveFileCommand ?? (_saveFileCommand = new Command(SaveFile, o => Accounts != null && Accounts.Any()));

        public ICommand CreateAccountCommand => _createAccountCommand ?? (_createAccountCommand = new Command(CreateAccount, o => Accounts != null));

        public ICommand EditAccountCommand => _editAccountCommand ?? (_editAccountCommand = new Command(EditAccount, o => Accounts != null && Accounts.Any()));

        public ObservableCollection<Account> Accounts
        {
            get => _accounts;
            private set
            {
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }

        public MainWindowViewModel(IAccountsLogic accountsLogic)
        {
            _accountsLogic = accountsLogic;
        }

        private void NewFile(object param)
        {
            Accounts = new ObservableCollection<Account>(new List<Account>());
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

        private async void SaveFile(object param)
        {
            var sfd = new SaveFileDialog { Filter = "All files (*.*)|*.*" };
            if (!string.IsNullOrWhiteSpace(_filePath))
                sfd.FileName = _filePath;

            if (sfd.ShowDialog() != true)
                return;

            _filePath = sfd.FileName;

            if (string.IsNullOrWhiteSpace(_filePath))
                return;

            await _accountsLogic.SaveAccounts(Accounts, _filePath);
        }

        private void CreateAccount(object param)
        {
            Accounts.Add(new Account { Id = Guid.NewGuid(), Login = "Login", Password = "password", WebSite = "website", Comment = "comment"});
        }

        private void EditAccount(object param)
        {

        }
    }
}