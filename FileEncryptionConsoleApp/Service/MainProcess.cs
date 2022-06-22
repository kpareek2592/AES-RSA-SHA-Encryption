using FileEncryptionConsoleApp.SupportService;
using System;
using System.Security.Cryptography;

namespace FileEncryptionConsoleApp.Service
{
    public class MainProcess
    {
        private readonly int keySize;

        #region DefaultValues

        public const int DefaultRSAKeyLength = 2048;

        public const int DefaultSHA256Iterations = 100000;
        public MainProcess(int DefaultRSAKeyLength)
        {
            keySize = DefaultRSAKeyLength;
        }
        #endregion

        public byte[] GenerateAESKey()
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 256;
                aes.GenerateKey();
                return aes.Key;
            }
        }

        public RSAKeyPair GenerateRSAKeyPair(int keySize = DefaultRSAKeyLength)
        {
            RSAParameters origin = GenerateKeys();
            RSAKeyPair keys = new RSAKeyPair
            {
                PublicKey = ExportPublicKey(origin),
                PrivateKey = ExportPrivateKey(origin),
                KeyLength = keySize
            };

            return keys;
        }

        /// <summary>
        /// Export public key blob
        /// </summary>
        /// <param name="rsaKeyInfo"></param>
        /// <returns></returns>
        public static byte[] ExportPublicKey(RSAParameters rsaKeyInfo)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(DefaultRSAKeyLength))
            {
                rsa.ImportParameters(rsaKeyInfo);
                return rsa.ExportCspBlob(false);
            }
        }

        /// <summary>
        /// Export private and public key blob
        /// </summary>
        /// <param name="rsaKeyInfo"></param>
        /// <returns></returns>
        public static byte[] ExportPrivateKey(RSAParameters rsaKeyInfo)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(DefaultRSAKeyLength))
            {
                rsa.ImportParameters(rsaKeyInfo);
                return rsa.ExportCspBlob(true);
            }
        }

        /// <summary>
        /// Generate a private/public key pair
        /// </summary>
        /// <returns></returns>
        public static RSAParameters GenerateKeys()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(DefaultRSAKeyLength))
            {
                return rsa.ExportParameters(true);
            }
        }

        /// <summary>
        /// Convert private/public key pair  to XML-string format
        /// </summary>
        /// <returns></returns>
        public string ConvertToXmlFormat(byte[] keyBlob)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize))
            {
                rsa.ImportCspBlob(keyBlob);
                return rsa.ToXmlString(true);
            }
        }

        public static RSAParameters ConvertKeysFromXml(string xmlString)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlString);
                return rsa.ExportParameters(true);
            }
        }
    }
}
