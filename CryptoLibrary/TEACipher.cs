using System;

namespace CryptoLibrary
{
    public class TEACipher :ICipher
    {
        private enum Action
        {
            Encrypt = 0,
            Decrypt
        }

        private const uint _delta=0x9E3779B9;
        private const uint _decryptSum = 0xC6EF3720;
        private const short _numberOfRounds = 32;

        public TEACipher()
        {
            Key = null;
        }

        public TEACipher(byte[] CipherKey)
        {
            Key = CipherKey;
        }

        public byte[] Key
        {
            set;
            internal get;
        }

        public byte[] Encrypt(byte[] DataToEncript) => TEAAlgorithm(DataToEncript, Action.Encrypt);

        public byte[] Decrypt(byte[] EncrytedData) => TEAAlgorithm(EncrytedData, Action.Decrypt);

        private byte[] TEAAlgorithm(byte[] Data, Action ActionToCommit)
        {
            CheckKeyAndData(Data.Length);

            uint[] KeyParts = GetKeyParts();
            byte[] ProcessedData = new byte[Data.Length];

            for (int indexer = 0; indexer < Data.Length; indexer += 8)
            {
                uint FirstValue = BitConverter.ToUInt32(Data, indexer);
                uint SecondValue = BitConverter.ToUInt32(Data, indexer + 4);
                if (ActionToCommit == Action.Encrypt)
                    Encrypt(ref FirstValue, ref SecondValue, KeyParts);
                else
                    Decrypt(ref FirstValue, ref SecondValue, KeyParts);
                byte[] FirstValueBytes = BitConverter.GetBytes(FirstValue);
                byte[] SecondValueBytes = BitConverter.GetBytes(SecondValue);
                for (int i = 0; i < 4; i++)
                {
                    ProcessedData[indexer + i] = FirstValueBytes[i];
                    ProcessedData[indexer + i + 4] = SecondValueBytes[i];
                }
            }
            return ProcessedData;
        }

        private void Encrypt(ref uint FirstValue,ref uint SecondValue,uint[] Key)
        {
            uint sum = 0;
            uint FirstValueContainter=FirstValue, SecondValueContainter=SecondValue;
            for (int indexer = 0; indexer < _numberOfRounds; indexer++)
            {                         
                sum += _delta;
                FirstValueContainter += ((SecondValueContainter << 4) + Key[0]) ^ (SecondValueContainter + sum) ^ ((SecondValueContainter >> 5) + Key[1]);
                SecondValueContainter += ((FirstValueContainter << 4) + Key[2]) ^ (FirstValueContainter + sum) ^ ((FirstValueContainter >> 5) + Key[3]);
            }
            FirstValue = FirstValueContainter;
            SecondValue = SecondValueContainter;
        }

        private void Decrypt(ref uint FirstValue, ref uint SecondValue, uint[] Key)
        {
            uint sum = _decryptSum;
            uint FirstValueContainter = FirstValue, SecondValueContainter = SecondValue;
            for (int indexer = 0; indexer < _numberOfRounds; indexer++)
            {
                SecondValueContainter -= ((FirstValueContainter << 4) + Key[2]) ^ (FirstValueContainter + sum) ^ ((FirstValueContainter >> 5) + Key[3]);
                FirstValueContainter -= ((SecondValueContainter << 4) + Key[0]) ^ (SecondValueContainter + sum) ^ ((SecondValueContainter >> 5) + Key[1]);
                sum -= _delta;
            }
            FirstValue = FirstValueContainter;
            SecondValue = SecondValueContainter;
        }

        private uint[] GetKeyParts()
        {
            uint[] KeyParts = new uint[4];
            for (int indexer = 0; indexer < Key.Length; indexer += 4)
            {
                KeyParts[indexer / 4] = BitConverter.ToUInt32(Key, indexer);
            }
            return KeyParts;
        }

        private void CheckKeyAndData(int DataLength)
        {
            if (Key == null || Key.Length != 16)
                throw new Exception("Cipher key is not set or not compatibale with TEA algorithm.");
            if (DataLength % 8 != 0)
                throw new Exception("Lenght of data is not compatibale with TEA algorithm.");
        }
    }
}
