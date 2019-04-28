using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;

namespace Model
{
    [DataContract]
    public class Account : INotifyPropertyChanged
    {
        private string _login;

        private string _website;

        private string _comment;

        public Account()
        {
            Id = Guid.NewGuid();
            Password = new SecureString();
        }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        public SecureString Password { get; set; }

        [DataMember]
        public string WebSite
        {
            get => _website;
            set
            {
                _website = value;
                OnPropertyChanged(nameof(WebSite));
            }
        }

        [DataMember]
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}