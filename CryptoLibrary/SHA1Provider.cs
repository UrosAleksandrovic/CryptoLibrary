using System;

namespace CryptoLibrary
{
    public class SHA1Provider
    {
        private static uint[] H = { 0x67452301, 0xEFCDAB89, 0x98BADCFE, 0x10325476, 0xC3D2E1F0 };
       

        private static uint LeftRotate(uint NumToRotate,short NumOfBits)
        {
            return (NumToRotate << NumOfBits) | (NumToRotate >> (32 - NumOfBits));
        }

        private static uint RightRotate(uint NumToRotate, short NumOfBits)
        {
            return (NumToRotate >> NumOfBits) | (NumToRotate << (32 - NumOfBits));
        }

        private static void RestartConstants()
        {
            H[0] = 0x67452301;
            H[1] = 0xEFCDAB89;
            H[2] = 0x98BADCFE;
            H[3] = 0x10325476;
            H[4] = 0xC3D2E1F0;
        }

        private static byte[] AppendH()
        {
            byte[] ResaultBlock = new byte[20];
            for (int indexer = 0; indexer < 5; indexer++)
            {
                byte[] CurrentH = BitConverter.GetBytes(H[indexer]);
                for (int i = 0; i < 4; i++)
                {
                    ResaultBlock[indexer * 4 + i] = CurrentH[i];
                } 
            }
            return ResaultBlock;
        }

        public static byte[] Hash(byte[] Data)
        {
            if(Data.Length%64 != 0)
            {
                throw new Exception("Data is not of valid size.");
            }
            RestartConstants();
            byte[] TempBlock = new byte[64];
            for (int indexer = 0; indexer < Data.Length/64; indexer++)
            {
                for (int i = 0; i < 64; i++)
                    TempBlock[i] = Data[indexer * 64 + i];
                HashSingleBlock(PreprocessingData(TempBlock));
            }
            return AppendH();
        }

        private static void HashSingleBlock(byte[] SingleBlock)
        {
            if (SingleBlock.Length != 64)
                throw new Exception("Single block does not match right size!");
            uint[] W = new uint[80];
            byte[] ResaultBlock = new byte[20];
            if (BitConverter.IsLittleEndian)
                Array.Reverse(SingleBlock);
            for (int indexer = 0; indexer < SingleBlock.Length; indexer += 4)
            {
                W[indexer / 4] = BitConverter.ToUInt32(SingleBlock, indexer);
            }
            for(int indexer = 16; indexer < W.Length; indexer += 1)
            {
                W[indexer] = W[indexer - 3] ^ W[indexer - 8] ^ W[indexer - 14] ^ W[indexer - 16];
                W[indexer] = LeftRotate(W[indexer],1);
            }
            uint A = H[0], B = H[1], C = H[2], D = H[3], E = H[4];
            for (int indexer = 0; indexer < 80; indexer++)
            {
                Round(W[indexer],indexer,ref A,ref B,ref C,ref D,ref E);
            }
            H[0] += A; H[1] += B; H[2] += C; H[3] += D; H[4] += E;
        }

        private static void Round(uint W,int RoundNumber,ref uint A, ref uint B,ref uint C,ref uint D,ref uint E)
        {
            uint F=0, K=0;
            if(RoundNumber>=0 && RoundNumber <= 19)
            {
                F = (B & C) | ((~B) & D);
                K = 0x5A827999;
            }
            else if(RoundNumber>=20 && RoundNumber <= 39)
            {
                F = B ^ C ^ D;
                K = 0x6ED9EBA1;
            }
            else if(RoundNumber>=40 && RoundNumber<= 59)
            {
                F = (B & C) | (B & D) | (C & D);
                K = 0x8F1BBCDC;
            }
            else if(RoundNumber>=60 && RoundNumber<=79)
            {
                F = B ^ C ^ D;
                K = 0xCA62C1D6;
            }
            uint Temp = LeftRotate(A, 5) + F + E + K + W;
            E = D;
            D = C;
            C = LeftRotate(B, 30);
            B = A;
            A = Temp;
        }

        private static byte[] PreprocessingData(byte[] DataBlock)
        {
            uint BytesToAdd = Convert.ToUInt32((64 - (DataBlock.Length % 64)) % 64);
            byte[] PreprocessedData = new byte[DataBlock.Length + BytesToAdd];
            if (BytesToAdd == 0)
            {
                Array.Copy(DataBlock, PreprocessedData, PreprocessedData.Length);
            }
            else
            {
                for (int i = 0; i < DataBlock.Length; i++)
                {
                    PreprocessedData[i] = DataBlock[i];
                }
                PreprocessedData[DataBlock.Length] = 0x80;
                for (int i = 0; i < BytesToAdd - 2; i++)
                {
                    PreprocessedData[DataBlock.Length + i] = 0;
                }
                PreprocessedData[PreprocessedData.Length - 2] = getByte((64 - BytesToAdd) * 8,1);
                PreprocessedData[PreprocessedData.Length - 1] = getByte((64 - BytesToAdd) * 8,0);
            }
            return PreprocessedData;
        }

        private static byte getByte(uint x, int n)
        {
            return (byte)((x >> 8 * n) & 0xFF);
        }


    }  
}
