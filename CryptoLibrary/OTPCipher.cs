using System.Collections;

namespace CryptoLibrary
{
    public class OTPCipher : ICipher, ISymmetricKey
    {

        public OTPCipher() 
        {
            Key = null;
        }

        public OTPCipher(byte[] CipherKey)
        {
            Key = CipherKey;
        }

        #region ICipher

        public byte[] Key { set; internal get; }
        
        public byte[] Encrypt(byte[] DataToEncript)
        {
            if ( Key== null  || DataToEncript.Length != Key.Length)
                throw new System.Exception("Data length must be of same length as Pad used in OTP algorithm. Key may not be set!");
            BitArray DataAsBitArray = new BitArray(DataToEncript);
            BitArray PadAsBitArray = new BitArray(Key);

            BitArray EncryptedData = DataAsBitArray.Xor(PadAsBitArray);

            return Converter.BitToByteArray(EncryptedData);
        }

        public byte[] Decrypt(byte[] EncriptedData) => this.Encrypt(EncriptedData);

        public void SetKey(byte[] Key)
        {
            this.Key = Key;
        }

        #endregion
    }
}
