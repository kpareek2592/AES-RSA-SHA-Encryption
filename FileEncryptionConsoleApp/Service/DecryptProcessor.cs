using FileEncryptionConsoleApp.SupportService;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FileEncryptionConsoleApp.Service
{
    public class DecryptProcessor
    {
        public static string GetChecksum(string filepath)
        {
            using (var hasher = System.Security.Cryptography.HashAlgorithm.Create("SHA256"))
            {
                using (var stream = System.IO.File.OpenRead(filepath))
                {
                    var hash = hasher.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "");
                }
            }
        }

        public static byte[] DecryptRsaKey(byte[] encryptedKey, byte[] privateKey)
        {
            const int keySize = 2048;
            using (var rsa = new RSACryptoServiceProvider(keySize))
            {
                rsa.ImportCspBlob(privateKey);
                return rsa.Decrypt(encryptedKey, RSAEncryptionPadding.OaepSHA1);
            }
        }

        public static byte[] Decrypt(byte[] data, byte[] privateKey)
        {
            using (var aes = new AesCryptoServiceProvider
            {
                Key = privateKey,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
            })
            {
                var iv = new byte[16];
                Buffer.BlockCopy(data, 0, iv, 0, iv.Length);

                using (MemoryStream stream = new MemoryStream(data) { Position = 16 }) // the first 16 bytes are the iv, ignore them
                using (var cryptoStream = new CryptoStream(stream, aes.CreateDecryptor(aes.Key, iv), CryptoStreamMode.Read))
                {
                    MemoryStream output = new MemoryStream();
                    var buffer = new byte[1024];
                    int read;
                    do
                    {
                        read = cryptoStream.Read(buffer, 0, buffer.Length);
                        output.Write(buffer, 0, read);
                    } while (read > 0);

                    return output.ToArray();
                }
            }
        }
    }
}
