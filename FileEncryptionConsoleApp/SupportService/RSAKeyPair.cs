namespace FileEncryptionConsoleApp.SupportService
{
    public class RSAKeyPair
    {
        public byte[] PublicKey { get; set; }

        public byte[] PrivateKey { get; set; }

        public int KeyLength { get; set; }
    }
}
