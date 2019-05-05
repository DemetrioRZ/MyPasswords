using System;
using System.IO;
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
        /// <summary>
        /// Длина соли в байтах.
        /// </summary>
        private const int SaltLength = 8;

        /// <summary>
        /// Длина соли в символах Base64.
        /// </summary>
        private const int SaltBase64Length = 12;

        /// <summary>
        /// Размер ключа в битах.
        /// </summary>
        private const int KeySize = 256;

        /// <summary>
        /// Размер блока в битах.
        /// </summary>
        private const int BlockSize = 128;

        /// <summary>
        /// :)
        /// </summary>
        private const int BitsInByte = 8;

        /// <summary>
        /// Количество итераций для получения ключа.
        /// </summary>
        private const int Iterations = 1000;

        /// <summary>
        /// Асинхронно расшифрует json с паролями.
        /// </summary>
        /// <param name="encryptedJson">зашифрованный json</param>
        /// <param name="masterPassword">мастер пароль</param>
        /// <returns>расшифрованный текст</returns>
        public async Task<string> DecryptAsync(string encryptedJson, SecureString masterPassword)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var decryptedText = AesDecrypt(encryptedJson, masterPassword);

                    return decryptedText;
                }
                catch (CryptographicException ex)
                {
                    // todo: добавить логи
                    throw new DecryptException("DecryptError", ex);
                }
            });
        }

        /// <summary>
        /// Асинхронно шифрует json c паролями.
        /// </summary>
        /// <param name="json">json c паролями</param>
        /// <param name="masterPassword">мастер пароль</param>
        /// <returns>зашифрованный json</returns>
        public async Task<string> EncryptAsync(string json, SecureString masterPassword)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var encryptedText = AesEncrypt(json, masterPassword);

                    return encryptedText;
                }
                catch (CryptographicException ex)
                {
                    // todo: добавить логи
                    throw new EncryptException("Encrypt error", ex);
                }
            });
        }

        /// <summary>
        /// Шифрует текст UTF8 указанным паролем.
        /// </summary>
        /// <param name="text">текст UTF8</param>
        /// <param name="password">пароль</param>
        /// <returns>зашифрованный текст в BASE64</returns>
        [SecurityCritical]
        private string AesEncrypt(string text, SecureString password)
        {
            using (var ms = new MemoryStream())
            {
                using (var aes = new RijndaelManaged())
                {
                    aes.KeySize = KeySize;
                    aes.BlockSize = BlockSize;

                    var salt = GetRandomBytes();

                    var key = new Rfc2898DeriveBytes(password.ToUnsecure(), salt, Iterations, HashAlgorithmName.SHA256);
                    aes.Key = key.GetBytes(aes.KeySize / BitsInByte);
                    aes.IV = key.GetBytes(aes.BlockSize / BitsInByte);

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

        /// <summary>
        /// Расшифровывает текст указанным паролем.
        /// </summary>
        /// <param name="text">текст BASE64</param>
        /// <param name="password"></param>
        /// <returns>расшифрованный текст</returns>
        [SecurityCritical]
        private string AesDecrypt(string text, SecureString password)
        {
            using (var ms = new MemoryStream())
            {
                using (var aes = new RijndaelManaged())
                {
                    aes.KeySize = KeySize;
                    aes.BlockSize = BlockSize;

                    var salt = Convert.FromBase64String(text.Substring(text.Length - SaltBase64Length, SaltBase64Length));

                    var key = new Rfc2898DeriveBytes(password.ToUnsecure(), salt, Iterations, HashAlgorithmName.SHA256);
                    aes.Key = key.GetBytes(aes.KeySize / BitsInByte);
                    aes.IV = key.GetBytes(aes.BlockSize / BitsInByte);

                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        var bytesToBeDecrypted = Convert.FromBase64String(text.Substring(0, text.Length - SaltBase64Length));
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