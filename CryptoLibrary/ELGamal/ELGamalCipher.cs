using BigIntegerExt;
using System;
using System.Numerics;
using System.Security.Cryptography;

namespace CryptoLibrary
{
    public class ELGamalCipher : AsymmetricAlgorithm, ICipher
    {
        private ELGamalKeyStruct _key;
        public ELGamalKeyStruct Key
        {
            set=> _key = value;
            internal get
            {
                if (NeedToGenerateKey())
                {
                    CreateKeyPair(KeySizeValue);
                }
                return _key;
            }
        }

        private RandomNumberGenerator NumberGenerator;

        #region Constructors

        public ELGamalCipher(ELGamalKeyStruct Key)
        {
            this.Key = Key;
            NumberGenerator = RandomNumberGenerator.Create();
        }

        #endregion

        #region ICipher

        public byte[] Decrypt(byte[] EncriptedData)
        {
            throw new System.NotImplementedException();
        }

        public byte[] Encrypt(byte[] DataToEncript)
        {
            BigInteger Message = new BigInteger(DataToEncript);
            byte[] EncriptedMessage  = ProcessBigInteger(Message);
            return EncriptedMessage;
        }
        #endregion

        #region Additional methodes

        public byte[] ProcessBigInteger(BigInteger Message)
        {
            if (BigInteger.Abs(Message) > Key.MaxRawPlainText)
                throw new Exception($"Message to encrypt is too large. Message should be |m| < 2^{Key.MaxRawPlainTextBits() - 1}");
            BigInteger K;
            do
            {
                K = new BigInteger();
                K = K.GenRandomBits(Key.PublicKey[0].BitCount() - 1, NumberGenerator);
            } while (BigInteger.GreatestCommonDivisor(K,Key.PublicKey[0]-1)!=1);

            BigInteger P = BigInteger.ModPow(Key.PublicKey[1], K, Key.P); //G mi je public key[1]
            BigInteger S = BigInteger.ModPow(Key.PublicKey[2], K, Key.P) * Encode(Message) % Key.P; //H mi je public key 2

            byte[] p_bytes = P.ToByteArray();
            byte[] s_bytes = S.ToByteArray();

            var res = new byte[Key.CiphertextBlocksize()];

            Array.Copy(p_bytes, 0, res, 0, p_bytes.Length);
            Array.Copy(s_bytes, 0, res, res.Length / 2, s_bytes.Length);

            return res;
        }

        private BigInteger Encode(BigInteger origin)
        {
            if (origin < 0)
                return Key.MaxRawPlainText + origin + 1;
            return origin;
        }

        public BigInteger ProcessByteBlock(byte[] block)
        {
            var byteLength = Key.CiphertextBlocksize() / 2;
            var p_bytes = new byte[byteLength];
            Array.Copy(block, 0, p_bytes, 0, p_bytes.Length);
            var s_bytes = new byte[byteLength];
            Array.Copy(block, block.Length - s_bytes.Length, s_bytes, 0, s_bytes.Length);

            var P = new BigInteger(p_bytes);
            var S = new BigInteger(s_bytes);

            P = BigInteger.ModPow(P, Key.PrivateKey[0], Key.PublicKey[0]); //A mi je privateKey0
            P = P.ModInverse(Key.PublicKey[0]);
            var M = S * P % Key.PublicKey[0];

            return Decode(M);
        }

        private BigInteger Decode(BigInteger origin)
        {
            origin = origin % (Key.MaxRawPlainText + 1);
            if (origin > Key.MaxRawPlainText / 2)
                return origin - Key.MaxRawPlainText - 1;
            return origin;
        }

        private bool NeedToGenerateKey() => (Key.P == 0) && (Key.G==0) && (Key.Y==0);
        
        private void CreateKeyPair(int KeyStrength)
        {
            do
            {
                Key.PublicKey[0] = Key.PublicKey[0].GenPseudoPrime(KeyStrength, 16, NumberGenerator);
            } while (Key.PLength() != KeyStrength / 8);

            Key.PrivateKey[0] = new BigInteger();
            Key.PrivateKey[0] = Key.PrivateKey[0].GenRandomBits(KeyStrength - 1, NumberGenerator);
            Key.PublicKey[1] = new BigInteger();
            Key.PublicKey[1] = Key.PublicKey[1].GenRandomBits(KeyStrength - 1, NumberGenerator);
            
            Key.PublicKey[2] = BigInteger.ModPow(Key.PublicKey[1], Key.PrivateKey[0], Key.PublicKey[0]);
        }


        
        #endregion

    }
}
