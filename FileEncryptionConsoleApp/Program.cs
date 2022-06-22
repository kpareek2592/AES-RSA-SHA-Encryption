using FileEncryptionConsoleApp.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FileEncryptionConsoleApp
{
    class Program
    {
        public const int DefaultRSAKeyLength = 2048;

        public const int DefaultSHA256Iterations = 100000;
        static void Main(string[] args)
        {
            MainProcess mainProcess = new MainProcess(DefaultRSAKeyLength);
            byte[] aesKey = mainProcess.GenerateAESKey();

            // Generate RSA Key-Pair
            SupportService.RSAKeyPair rsaKeys = mainProcess.GenerateRSAKeyPair(DefaultRSAKeyLength);
            byte[] publicKey = rsaKeys.PublicKey;   //byte[276]
            byte[] privateKey = rsaKeys.PrivateKey; //byte[1172]
            int keyLength = rsaKeys.KeyLength;      //2048

            // Get audio stream
            #region AudioStream

            string path = AssetPath(@"Data\AudioFile1.wav");
            string inputFile = AssetPath(@"Data\AudioFile1.wav.aes");
            string outputFile = AssetPath(@"Data\AudioFileTest_Encrypted.wav");
            
            byte[] data = System.IO.File.ReadAllBytes(path);
            MemoryStream ms = new MemoryStream(data);

            #endregion #AudioStream

            // encrypt file using AES and encrypt key with rsa public key

            EncryptProcessorOptions options = new EncryptProcessorOptions(publicKey, DefaultRSAKeyLength, "metadata-key");
            EncryptProcessor encryptProcessor = new EncryptProcessor(options);
            var result = encryptProcessor.EncryptFile(ms, options);

            byte[] encryptedData = result.EncryptedData;
            byte[] encryptedAesKey = result.EncryptedKey;

            File.WriteAllBytes(inputFile, encryptedData);

            // Get Hash - SHA256
            string checksum = DecryptProcessor.GetChecksum(path);
            byte[] checksumBytes = Encoding.ASCII.GetBytes(checksum);
            byte[] encryptedChecksum;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(DefaultRSAKeyLength))
            {
                encryptedChecksum = rsa.Encrypt(checksumBytes, RSAEncryptionPadding.OaepSHA1);
            }

            // Decrypt aes key through private key
            byte[] decrecptedAesKey = DecryptProcessor.DecryptRsaKey(encryptedAesKey, privateKey);

            // Decrypt file
            byte[] decryptedData = DecryptProcessor.Decrypt(encryptedData, decrecptedAesKey);

            bool isDataValid = data.SequenceEqual(decryptedData);
            
            File.WriteAllBytes(outputFile, decryptedData);
            string finalchecksum = DecryptProcessor.GetChecksum(outputFile);
            var checsumCompre = String.Equals(checksum, finalchecksum);

        }

        public static string AssetPath(string asset)
        {
            string testDir = Environment.CurrentDirectory;
            int binPos = testDir.IndexOf("bin", StringComparison.Ordinal);
            testDir = testDir.Substring(0, binPos);

            return testDir + asset;
        }

    }
}
