using System;

namespace CryptoLibrary
{
    public class TEAProvider
    {

        private const uint _delta=0x9E3779B9;
        private const uint _decryptSum = 0xC6EF3720;
        private const short _numberOfRounds = 32;


        //Ovde se ponavlja kod u pelji, vidi kako to da sredis
        public static byte[] Encrypt(byte[] Data, byte[] Key)
        {
            if (Data.Length % 8 != 0 || Key.Length != 16)
                throw new Exception("Length of Data or Key are not compatibale with TEA algorithm.");

            uint[] KeyParts = GetKeyParts(Key);
            byte[] EncrytedData = new byte[Data.Length];

            for(int indexer = 0; indexer < Data.Length; indexer += 8)
            {
                uint FirstValue = BitConverter.ToUInt32(Data, indexer);
                uint SecondValue = BitConverter.ToUInt32(Data, indexer + 4);
                Encrypt(ref FirstValue,ref SecondValue, KeyParts);
                byte[] FirstValueBytes = BitConverter.GetBytes(FirstValue);
                byte[] SecondValueBytes = BitConverter.GetBytes(SecondValue);
                for(int i = 0; i < 4; i++)
                {
                    EncrytedData[indexer + i] = FirstValueBytes[i];
                    EncrytedData[indexer + i + 4] = SecondValueBytes[i];
                }
            }
            return EncrytedData;
        }

        public static byte[] Decrypt(byte[] EncrytedData, byte[] Key)
        {
            if (EncrytedData.Length % 8 != 0 || Key.Length != 16)
                throw new Exception("Length of Data or Key are not compatibale with TEA algorithm.");

            uint[] KeyParts = GetKeyParts(Key);
            byte[] DecryptedData = new byte[EncrytedData.Length];

            for (int indexer = 0; indexer < EncrytedData.Length; indexer += 8)
            {
                uint FirstValue = BitConverter.ToUInt32(EncrytedData, indexer);
                uint SecondValue = BitConverter.ToUInt32(EncrytedData, indexer + 4);
                Decrypt(ref FirstValue, ref SecondValue, KeyParts);
                byte[] FirstValueBytes = BitConverter.GetBytes(FirstValue);
                byte[] SecondValueBytes = BitConverter.GetBytes(SecondValue);
                for (int i = 0; i < 4; i++)
                {
                    DecryptedData[indexer + i] = FirstValueBytes[i];
                    DecryptedData[indexer + i + 4] = SecondValueBytes[i];
                }
            }
            return DecryptedData;
        }

        private static void Encrypt(ref uint FirstValue,ref uint SecondValue,uint[] Key)
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
        private static void Decrypt(ref uint FirstValue, ref uint SecondValue, uint[] Key)
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

        private static uint[] GetKeyParts(byte[] Key)
        {
            uint[] KeyParts = new uint[4];
            for (int indexer = 0; indexer < Key.Length; indexer += 4)
            {
                KeyParts[indexer / 4] = BitConverter.ToUInt32(Key, indexer);
            }
            return KeyParts;
        }
    }
}
