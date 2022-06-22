using System;

namespace FileEncryptionConsoleApp.Service
{
    public class EncryptProcessorOptions
    {
        public EncryptProcessorOptions(byte[] publicKey, int keySize, string metadataKey = null)
        {
            //MetadataKey = metadataKey ?? throw new ArgumentNullException(nameof(metadataKey));
            PublicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey));
            KeySize = keySize >= 2048 ? keySize : throw new ArgumentException(nameof(keySize));
        }

        public string MetadataKey { get; }

        public byte[] PublicKey { get; }
        public int KeySize { get; }
    }
}
