using System;
using System.Runtime.Serialization;
using System.Security;

namespace Model
{
    /// <summary>
    /// Модель аккаунта
    /// </summary>
    [DataContract]
    public class Account
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Account()
        {
            Id = Guid.NewGuid();
            Password = new SecureString();
        }

        /// <summary>
        /// Идентификатор модели
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        [DataMember]
        public string Login { get; set; }
        
        /// <summary>
        /// Пароль
        /// </summary>
        public SecureString Password { get; set; }

        /// <summary>
        /// Сайт ресурса
        /// </summary>
        [DataMember]
        public string WebSite { get; set; }
        
        /// <summary>
        /// Комментарий к аккаунту
        /// </summary>
        [DataMember] 
        public string Comment { get; set; }

        /// <summary>
        /// Незащищённый пароль для сериализации.
        /// </summary>
        [DataMember]
        private string UnsafePassword
        {
            [SecurityCritical] get => Password.ToUnsecure();
            [SecurityCritical] set
            {
                if (Password == null)
                    Password = new SecureString();

                Password.Clear();
                foreach (var ch in value)
                    Password.AppendChar(ch);
            }
        }
    }
}