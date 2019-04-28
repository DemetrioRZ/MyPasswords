using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Logic;
using Model;

namespace Logic
{
    public class EncryptDecryptLogic : IEncryptDecryptLogic
    {
        public async Task<string> DecryptAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                var fileText = File.ReadAllText(filePath);

                var decryptedText = Decrypt("testkey", fileText);

                return decryptedText;
            });
        }

        public async Task EncryptAsync(string filePath, string json)
        {
            await Task.Run(() =>
            {
                var encryptedText = Encrypt("testkey", json);

                File.WriteAllText(filePath, encryptedText);
            });
        }

        public string Encrypt( string key, string data )
        {
            try
            {
                var keys = GetHashKeys(key);

                var encData = EncryptStringToBytes_Aes(data, keys[0], keys[1]);
            
                return encData;
            }
            catch (CryptographicException ex)
            {
                // todo: добавить логирование
                throw new EncryptException("Encrypt error", ex);
            }
            
        }

        public string Decrypt(string key, string data)
        {
            try
            {
                var keys = GetHashKeys(key);
            
                var decData = DecryptStringFromBytes_Aes( data, keys[0], keys[1] );
            
                return decData;
            }
            catch (Exception ex)
            {
                // todo: добавить логирование
                throw new DecryptException("Decrypt error", ex);
            }
        }

        private byte[][] GetHashKeys(string key)
        {
            var result = new byte[2][];
            var enc = Encoding.UTF8;

            var sha256 = new SHA256CryptoServiceProvider();

            var rawKey = enc.GetBytes(key);
            var rawIV = enc.GetBytes(key);

            var hashKey = sha256.ComputeHash(rawKey);
            var hashIV = sha256.ComputeHash(rawIV);

            Array.Resize(ref hashIV, 16);

            result[0] = hashKey;
            result[1] = hashIV;

            return result;
        }

        private static string EncryptStringToBytes_Aes( string plainText, byte[] key, byte[] iv )
        {
            if ( string.IsNullOrWhiteSpace(plainText) )
                throw new ArgumentNullException( nameof(plainText) );
            if ( key == null || !key.Any() )
                throw new ArgumentNullException( nameof(key) );
            if ( iv == null || !iv.Any() )
                throw new ArgumentNullException( nameof(iv) );

            byte[] encrypted;

            using ( AesManaged aesAlg = new AesManaged() )
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using ( MemoryStream msEncrypt = new MemoryStream() )
                {
                    using ( CryptoStream csEncrypt = new CryptoStream( msEncrypt, encryptor, CryptoStreamMode.Write ) )
                    {
                        using ( StreamWriter swEncrypt = new StreamWriter( csEncrypt ) )
                        {
                            swEncrypt.Write( plainText );
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String( encrypted );
        }

        private static string DecryptStringFromBytes_Aes( string cipherTextString, byte[] key, byte[] iv )
        {
            if ( string.IsNullOrWhiteSpace(cipherTextString) )
                throw new ArgumentNullException( nameof(cipherTextString) );
            if ( key == null || !key.Any() )
                throw new ArgumentNullException( nameof(key) );
            if ( iv == null || !iv.Any() )
                throw new ArgumentNullException( nameof(iv) );

            var cipherText = Convert.FromBase64String(cipherTextString);
            
            string plaintext = null;

            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read) )
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt) )
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}