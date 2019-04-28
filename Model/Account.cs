using System;
using System.Runtime.Serialization;
using System.Security;

namespace Model
{
    [DataContract]
    public class Account
    {
        public Account()
        {
            Id = Guid.NewGuid();
            Password = new SecureString();
        }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public SecureString Password { get; set; }

        [DataMember]
        public string WebSite { get; set; }

        [DataMember]
        public string Comment { get; set; }
    }
}