﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Input;
using Interfaces.Logic;
using Interfaces.Views;
using Microsoft.Win32;
using Model;
using Views.Common;

namespace Views
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Логика работы с аккаунтами
        /// </summary>
        private readonly IAccountsLogic _accountsLogic;

        /// <summary>
        /// Функтор получения окна редактора аккаунта
        /// </summary>
        private readonly Func<IEditAccountView> _getEditAccountWindowView;

        /// <summary>
        /// Функтор получения окна ввода мастер пароля.
        /// </summary>
        private readonly Func<IEnterMasterPasswordView> _getEnterMasterPasswordWindowView;

        /// <summary>
        /// Путь к сериализованному файлу с аккаунтами.
        /// </summary>
        private string _serializedAccountsFilePath;

        /// <summary>
        /// Мастер пароль.
        /// </summary>
        [SecurityCritical]
        private SecureString _masterPassword;

        /// <summary>
        /// Коллекция отображаемых аккаунтов
        /// </summary>
        private ObservableCollection<AccountViewModel> _accounts;

        /// <summary>
        /// Выбранная модель аккаунта.
        /// </summary>
        private AccountViewModel _selectedAccount;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="accountsLogic">Логика работы с аккаунтами</param>
        /// <param name="getEditAccountWindowView">Функтор получения окна редактора аккаунта</param>
        /// <param name="getEnterMasterPasswordWindowView">Функтор получения окна ввода мастер пароля</param>
        public MainViewModel(
            IAccountsLogic accountsLogic, 
            Func<IEditAccountView> getEditAccountWindowView, 
            Func<IEnterMasterPasswordView> getEnterMasterPasswordWindowView)
        {
            _accountsLogic = accountsLogic;
            _getEditAccountWindowView = getEditAccountWindowView;
            _getEnterMasterPasswordWindowView = getEnterMasterPasswordWindowView;

            InitializeCommands();
        }

        /// <summary>
        /// Команда создания нового файла с аккаунтами.
        /// </summary>
        public ICommand NewFileCommand { get; private set; }

        /// <summary>
        /// Команда открытия файла с аккаунтами.
        /// </summary>
        public ICommand OpenFileCommand { get; private set; }

        /// <summary>
        /// Команда сохранения файла с аккаунтами.
        /// </summary>
        public ICommand SaveFileCommand { get; private set; }

        /// <summary>
        /// Команда создания нового аккаунта для хранения.
        /// </summary>
        public ICommand CreateAccountCommand { get; private set; }

        /// <summary>
        /// Команда редактирования выбранного аккаунта.
        /// </summary>
        public ICommand EditAccountCommand { get; private set; }

        /// <summary>
        /// Команда удаления выбранного аккаунта.
        /// </summary>
        public ICommand DeleteAccountCommand { get; private set; }

        /// <summary>
        /// Коллекция отображаемых аккаунтов
        /// </summary>
        public ObservableCollection<AccountViewModel> Accounts
        {
            get => _accounts;
            private set
            {
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
                OnPropertyChanged(nameof(CanShowGrid));
                OnPropertyChanged(nameof(CanShowHintLabel));
            }
        }

        /// <summary>
        /// Проверка наличия открытого или созданного списка аккаунтов.
        /// </summary>
        public bool CanShowGrid => _accounts != null;

        /// <summary>
        /// Проверка показывать ли подсказку с чего начать работу.
        /// </summary>
        public bool CanShowHintLabel => _accounts == null;

        /// <summary>
        /// Выбранный аккаунт.
        /// </summary>
        public AccountViewModel SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
            }
        }

        /// <summary>
        /// Инициализация команд.
        /// </summary>
        private void InitializeCommands()
        {
            NewFileCommand = new Command(NewFile);
            OpenFileCommand = new Command(OpenFileAsync);
            SaveFileCommand = new Command(SaveFileAsync, x => Accounts != null);
            CreateAccountCommand = new Command(CreateAccount, x => Accounts != null);
            EditAccountCommand = new Command(EditAccount, x => Accounts != null && Accounts.Any() && SelectedAccount != null);
            DeleteAccountCommand = new Command(DeleteAccount, x => Accounts != null && Accounts.Any() && SelectedAccount != null);
        }

        /// <summary>
        /// Создаёт новый для хранения аккаунтов.
        /// </summary>
        private void NewFile(object param)
        {
            if (Accounts != null && MessageBox.Show("Create new file?", "New file", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                return;

            _serializedAccountsFilePath = null;
            _masterPassword = null;

            Accounts = new ObservableCollection<AccountViewModel>(new List<AccountViewModel>());
        }

        /// <summary>
        /// Команда открытия файла с аккаунтами.
        /// </summary>
        private async void OpenFileAsync(object param)
        {
            if (Accounts != null && MessageBox.Show("Open existing file?", "Open file", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                return;

            var ofd = new OpenFileDialog {Multiselect = false, Filter = "All files (*.*)|*.*"};
            
            if (ofd.ShowDialog() != true)
                return;

            _serializedAccountsFilePath = ofd.FileName;

            if (string.IsNullOrWhiteSpace(_serializedAccountsFilePath))
                return;

            var enterMasterPasswordView = _getEnterMasterPasswordWindowView();

            if (enterMasterPasswordView.ShowDialog() != true)
            {
                _serializedAccountsFilePath = null;
                return;
            }

            if (!(enterMasterPasswordView.DataContext is EnterMasterPasswordViewModel enterMasterPasswordViewModel))
            {
                _serializedAccountsFilePath = null;
                return;
            }

            _masterPassword = enterMasterPasswordViewModel.MasterPassword.Copy();

            try
            {
                var accounts = await _accountsLogic.GetAccounts(_serializedAccountsFilePath, _masterPassword.Copy());
                var accountViewModels = accounts.Select(x => new AccountViewModel().For(x)).ToList();

                Accounts = new ObservableCollection<AccountViewModel>(accountViewModels);
            }
            catch (DecryptException)
            {
                MessageBox.Show("Decrypt file error, wrong password", "Decryption error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Сохраняет файл с аккаунтами на диск.
        /// </summary>
        private async void SaveFileAsync(object param)
        {
            if (string.IsNullOrWhiteSpace(_serializedAccountsFilePath))
            {
                var sfd = new SaveFileDialog { Filter = "Gzip files (*.gz)|*.gz" };
                
                if (sfd.ShowDialog() != true)
                    return;

                _serializedAccountsFilePath = sfd.FileName;

                if (string.IsNullOrWhiteSpace(_serializedAccountsFilePath))
                    return;
            }
            
            if (_masterPassword == null)
            {
                var enterMasterPasswordView = _getEnterMasterPasswordWindowView();

                if (enterMasterPasswordView.ShowDialog() != true)
                {
                    _serializedAccountsFilePath = null;
                    return;
                }

                if (!(enterMasterPasswordView.DataContext is EnterMasterPasswordViewModel enterMasterPasswordViewModel))
                {
                    _serializedAccountsFilePath = null;
                    return;
                }

                _masterPassword = enterMasterPasswordViewModel.MasterPassword.Copy();
            }

            try
            {
                var accounts = Accounts.Select(x => x.GetModel()).ToList();
                await _accountsLogic.SaveAccounts(accounts, _serializedAccountsFilePath, _masterPassword.Copy());
            }
            catch (EncryptException)
            {
                MessageBox.Show("Encrypt file error, please contact developer", "Encryption error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Создаёт новый аккаунт.
        /// </summary>
        private void CreateAccount(object param)
        {
            var account = new AccountViewModel().For(new Account());
            var editWindow = _getEditAccountWindowView();
            if (!(editWindow.DataContext is EditAccountViewModel editWindowViewModel))
                return;

            editWindowViewModel.EditingAccount = account;

            if (editWindow.ShowDialog() != true)
                return;

            Accounts.Add(editWindowViewModel.EditingAccount);
        }

        /// <summary>
        /// Редактирует выбранный аккаунт.
        /// </summary>
        /// <param name="param">модель представления вида выбранного аккаунта</param>
        private void EditAccount(object param)
        {
            if (!(param is AccountViewModel account))
                return;

            var editWindow = _getEditAccountWindowView();
            if (!(editWindow.DataContext is EditAccountViewModel editWindowViewModel))
                return;

            editWindowViewModel.EditingAccount = account;

            editWindow.ShowDialog();
        }

        /// <summary>
        /// Удаляет выбранный аккаунт.
        /// </summary>
        /// <param name="param">модель представления вида выбранного аккаунта</param>
        private void DeleteAccount(object param)
        {
            if (!(param is AccountViewModel account))
                return;

            if (MessageBox.Show($"Are you sure you want to delete account {account.Login}?", "Delete account", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                return;

            Accounts.Remove(account);
            account.Password.Dispose();
        }
    }
}