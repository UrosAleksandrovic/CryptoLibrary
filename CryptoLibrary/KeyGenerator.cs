using System.Security.Cryptography;

namespace CryptoLibrary
{
    public class KeyGenerator
    {

        public static byte[] Generate128BitKey()
        {
            return KeyGenerator.GenerateKey(16);
        }

        public static byte[] Generate64BitKey()
        {
            return KeyGenerator.GenerateKey(8);
        }

        public static byte[] GenerateKey(int KeyLength)
        {
            RNGCryptoServiceProvider Generator = new RNGCryptoServiceProvider();
            byte[] GeneratedKey = new byte[KeyLength];
            Generator.GetBytes(GeneratedKey);
            return GeneratedKey;
        }
    }
}
