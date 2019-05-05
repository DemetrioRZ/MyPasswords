using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Logic;
using Model;

namespace Logic
{
    public class EncryptDecryptLogic : IEncryptDecryptLogic
    {
        private const int SaltLength = 8;

        public async Task<string> DecryptAsync(string encryptedJson, SecureString masterPassword)
        {
            return await Task.Run(() =>
            {
                var decryptedText = AesDecrypt(encryptedJson, masterPassword.ToUnsecure());

                return decryptedText;
            });
        }

        public async Task<string> EncryptAsync(string json, SecureString key)
        {
            return await Task.Run(() =>
            {
                var encryptedText = AesEncrypt(json, key.ToUnsecure());

                return encryptedText;
            });
        }

        private string AesEncrypt(string text, string password)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (var aes = new RijndaelManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;

                    var salt = GetRandomBytes();

                    var key = new Rfc2898DeriveBytes(password, salt, 1000);
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);

                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        var bytesToBeEncrypted = Encoding.UTF8.GetBytes(text);
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    var encryptedBytes = ms.ToArray();

                    return Convert.ToBase64String(encryptedBytes) + Convert.ToBase64String(salt);
                }
            }
        }

        private string AesDecrypt(string text, string password)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;

                    var salt = Convert.FromBase64String(text.Substring(text.Length - 12, 12));

                    var key = new Rfc2898DeriveBytes(password, salt, 1000);
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);

                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        var bytesToBeDecrypted = Convert.FromBase64String(text.Substring(0, text.Length - 12));
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    var decryptedBytes = ms.ToArray();

                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }

        /// <summary>
        /// RNGCryptoServiceProvider
        /// </summary>
        /// <returns></returns>
        private byte[] GetRandomBytes()
        {
            var ba = new byte[SaltLength];
            RandomNumberGenerator.Create().GetBytes(ba);
            return ba;
        }
    }
}