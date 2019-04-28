using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Model
{
    /// <summary>
    /// Методы расширения для уровня моделей.
    /// </summary>
    public static class ModelExtensions
    {
        /// <summary>
        /// Защищённую строку приводит к виду незащищённой строки
        /// </summary>
        /// <param name="secureString">защищённая строка</param>
        /// <returns>незащищённая строка</returns>
        [SecurityCritical]
        public static string ToUnsecure(this SecureString secureString)
        {
            if (secureString == null)
                throw new ArgumentNullException(nameof(secureString));

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}