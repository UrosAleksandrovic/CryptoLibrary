using System.Numerics;
using BigIntegerExt;

namespace CryptoLibrary
{
    public class ELGamalKeyStruct:IAsymmetricKey
    {
        //P je na netu Q
        //K je privatni deo
        //G je G
        //X je A - Ovo je privatni kljuc Sve je ostalno javni?
        //Y je H 
        public BigInteger[] PublicKey { set; get; }
        public BigInteger[] PrivateKey { set; get; }

        public BigInteger P;
        public BigInteger G;
        public BigInteger Y;
        public BigInteger X;

        private BigInteger _maxRawPlainText;

        public BigInteger MaxRawPlainText
        {
            get
            {
                if (_maxRawPlainText == BigInteger.Zero)
                    _maxRawPlainText = BigInteger.Pow(2, MaxRawPlainTextBits()) - BigInteger.One;
                return _maxRawPlainText;
            }
        }


        public BigInteger MaxRawPlainTextHalf() => MaxRawPlainText / 2;

        public int MaxRawPlainTextBits() => 128;

        public int CiphertextBlocksize() => PublicKey[0].BitCount() * 2 + 2;

        public int CiphertextLength() => CiphertextBlocksize() * 2;

        public int PLength() => (PublicKey[0].BitCount() + 7) / 8;

    }
}
