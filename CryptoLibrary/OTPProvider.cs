using System.Collections;

namespace CryptoLibrary
{
    public class OTPProvider : IOTPProvider
    {
        private static OTPProvider _instance;
        private static readonly object _locker = new object();

        public static OTPProvider Instance
        {
            get
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        _instance = new OTPProvider();
                    }
                    return _instance;
                }
            }
        }

        private OTPProvider() { }

        public byte[] Decrypt(byte[] Encrypted, byte[] Pad)
        {
            return this.Encrypt(Encrypted, Pad);
        }

        public byte[] Encrypt(byte[] Data, byte[] Pad)
        {
            if (Data.Length != Pad.Length)
                throw new System.Exception("Data length must be of same length as Pad used in OTP algorithm.");
            BitArray DataAsBitArray = new BitArray(Data);
            BitArray PadAsBitArray = new BitArray(Pad);

            BitArray EncryptedData = DataAsBitArray.Xor(PadAsBitArray);

            return Converter.BitToByteArray(EncryptedData);
        }
    }
}
