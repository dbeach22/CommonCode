using FatHead.Crypto.Interfaces;
using FatHead.Enums;
using FatHead.Loggers;
using FatHead.Loggers.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FatHead.Crypto
{
    public class Cryptography : ICryptography
    {
        private string _encryptionKey;
        private ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="encryptionKey">System.String Encryption key</param>
        /// <param name="logger">Fathead.Loggers.Interfaces.ILogger</param>
        public Cryptography(string encryptionKey, ILogger logger)
        {
            _encryptionKey = encryptionKey;
            _logger = logger;
        }

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="clearText">System.String Clear text string</param>
        /// <returns>Encrypted string</returns>
        public string Encrypt(string clearText)
        {
            try
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(new Log(ErrorCode.Error, DateTime.Now, ex.Message));
            }

            return clearText;
        }

        /// <summary>
        /// Decrypts an encrypted string
        /// </summary>
        /// <param name="cipherText">System.StringEncrypted string</param>
        /// <returns>Clear text string</returns>
        public string Decrypt(string cipherText)
        {
            try
            {
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(new Log(ErrorCode.Error, DateTime.Now, ex.Message));
            }

            return cipherText;
        }
    }
}
