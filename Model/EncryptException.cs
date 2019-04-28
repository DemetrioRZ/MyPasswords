using System;

namespace Model
{
    public class EncryptException : Exception
    {
        public EncryptException()
        {
        }

        public EncryptException(string message)
            : base(message)
        {
        }

        public EncryptException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}