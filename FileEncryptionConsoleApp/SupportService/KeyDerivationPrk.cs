using System;

namespace FileEncryptionConsoleApp.SupportService
{
    public class KeyDerivationPrk
    {
        private readonly byte[] prk;

        private KeyDerivationPrk(byte[] prk)
        {
            if (prk == null)
            {
                throw new ArgumentNullException(nameof(prk));
            }
            if (prk.Length != 32)
            {
                throw new ArgumentException($"Expected Pseudo random key to be of length 32, but was {prk.Length}", nameof(prk));
            }
            this.prk = prk;
        }

        public byte[] ExtractPseudoRandomKey()
        {
            byte[] buffer = new byte[prk.Length];
            Array.Copy(prk, 0, buffer, 0, prk.Length);
            return buffer;
        }

        public static KeyDerivationPrk UseBase64EncodedPseudoRandomKey(string serializedValue)
        {
            if (serializedValue == null)
            {
                throw new ArgumentNullException(nameof(serializedValue));
            }
            var key = Convert.FromBase64String(serializedValue);
            return new KeyDerivationPrk(key);
        }
    }
}
