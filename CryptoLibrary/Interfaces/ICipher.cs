namespace CryptoLibrary
{
    public interface ICipher
    {
        byte[] Encrypt(byte[] DataToEncript);

        byte[] Decrypt(byte[] EncriptedData);

        byte[] Key
        {
            set;
        }

    }
}
