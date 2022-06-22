using System;

namespace FileEncryptionConsoleApp.SupportService
{
    public class EncryptMetadata
    {
        private readonly byte[] key;

        private EncryptMetadata(byte[] key)
        {
            this.key = key ?? throw new ArgumentNullException("Key must be non null");
        }

        public byte[] ExportKey()
        {
            byte[] copy = new byte[key.Length];
            key.CopyTo(copy, 0);
            return copy;
        }

        public static EncryptMetadata UseKey(byte[] encryptionKey)
        {
            return new EncryptMetadata(encryptionKey);
        }
    }
}
