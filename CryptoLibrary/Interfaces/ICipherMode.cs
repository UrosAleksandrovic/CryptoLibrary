namespace CryptoLibrary
{
    public interface ICipherMode:ICipher
    {
        ICipher Cipher
        {
            set;
        }
    
        uint DataBlockSize
        {
            set;
        }

        byte[] InitializationVector
        {
            set;
        }

        void CheckVector();
    }
}
