namespace CryptoLibrary
{
    public interface ICipherMode:ICipher,ISymmetricKey
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
