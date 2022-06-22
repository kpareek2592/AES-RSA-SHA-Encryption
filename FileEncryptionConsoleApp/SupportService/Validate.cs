using System;

namespace FileEncryptionConsoleApp.SupportService
{
    public static class Validate
    {
        public static byte[] NotNullOrEmpty(byte[] value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Byte array must not be null", name);
            }
            if (value.Length == 0)
            {
                throw new ArgumentException("Byte array must not be empty", name);
            }
            return value;
        }
    }
}
