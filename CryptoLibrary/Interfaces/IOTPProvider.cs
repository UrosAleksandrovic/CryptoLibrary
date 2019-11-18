namespace CryptoLibrary
{
    public interface IOTPProvider
    {
        byte[] Encrypt(byte[] Data, byte[] Pad);

        byte[] Decrypt(byte[] Encrypted, byte[] Pad);
    }
}
