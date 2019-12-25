using System.Numerics;

namespace CryptoLibrary
{
    public interface IAsymmetricKey
    {
        BigInteger[] PublicKey
        {
            set;
        }

        BigInteger[] PrivateKey
        {
            set;
        }
    }
}
