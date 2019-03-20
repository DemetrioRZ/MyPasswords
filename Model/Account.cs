using System;
using System.Runtime.Serialization;

namespace Model
{
    [DataContract]
    public class Account
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string WebSite { get; set; }

        [DataMember]
        public string Comment { get; set; }
    }
}