using System;

namespace CryptoLibrary
{
    public class CipherFeedbackMode : ICipherMode, ISymmetricKey
    {
        private byte[] _initializationVector;
        


        #region Constructors
        public CipherFeedbackMode()
        {
            Cipher = null;
        }

        public CipherFeedbackMode(ICipher Cipher)
        {
            this.Cipher = Cipher;
        }

        #endregion

        #region ICipherMode
        public ICipher Cipher
        {
            set;
            internal get;
        }

        public uint DataBlockSize
        {
            set;
            internal get;
        }

        public byte[] InitializationVector
        {
            set
            {
                if (DataBlockSize == default(uint))
                    throw new Exception("Data block size must be set first!");
                if (value.Length != DataBlockSize)
                    throw new Exception("Initialization vector is not the right size!");

                _initializationVector = value;
            }
            internal get => _initializationVector;
        }

        public void CheckVector()
        {
            if(InitializationVector == null)
                throw new Exception("Initialization vector is null!");
        }

        #endregion

        #region ICipher

        public byte[] Key { set; internal get; }

        public byte[] Encrypt(byte[] DataToEncript)
        {
            CheckVector();
            if (Cipher == null)
                throw new Exception("Cipher is not set!");

            Cipher.SetKey(Key);
            byte[] EncriptedData = new byte[DataToEncript.Length];
            byte[] Vector = InitializationVector;
            for(int i = 0; i < (DataToEncript.Length) / DataBlockSize; i++)
            {
                byte[] SingleBlock = new byte[DataBlockSize];
                for(int j = 0; j < DataBlockSize; j++)
                {
                    SingleBlock[j] = DataToEncript[(i * DataBlockSize) + j];
                }
                Vector = EncryptSingleBlock(SingleBlock,ref Vector);
                for (int j = 0; j < DataBlockSize; j++)
                {
                    EncriptedData[(i * DataBlockSize) + j] = Vector[j];
                }
            }
            return EncriptedData;
        }

        public byte[] Decrypt(byte[] EncriptedData)
        {
            CheckVector();
            if (Cipher == null)
                throw new Exception("Cipher is not set!");

            Cipher.SetKey(Key);
            byte[] DecriptedData = new byte[EncriptedData.Length];
            byte[] Vector = InitializationVector;
            for (int i = 0; i < (EncriptedData.Length) / DataBlockSize; i++)
            {
                byte[] SingleBlock = new byte[DataBlockSize];
                for (int j = 0; j < DataBlockSize; j++)
                {
                    SingleBlock[j] = EncriptedData[(i * DataBlockSize) + j];
                }
                byte[] DecriptedBlock = DecriptSingleBlock(SingleBlock, Vector);
                Vector = SingleBlock;
                for (int j = 0; j < DataBlockSize; j++)
                {
                    DecriptedData[(i * DataBlockSize) + j] = DecriptedBlock[j];
                }
            }
            return DecriptedData;
        }
        
        #endregion
    
    
        private byte[] EncryptSingleBlock(byte[] SingleBlock,ref byte[] Vector)
        {
            byte[] Temp = Cipher.Encrypt(Vector);

            ICipher OTP = new OTPCipher(SingleBlock);
            Vector = OTP.Encrypt(Temp);
            return Vector;
        }

        private byte[] DecriptSingleBlock(byte[] SingleBlock,byte[] Vector)
        {
            byte[] Temp = Cipher.Encrypt(Vector);

            ICipher OTP = new OTPCipher(SingleBlock);
            return OTP.Encrypt(Temp);
        }

        public void SetKey(byte[] Key)
        {
            this.Key = Key;
            if (Cipher != null)
                this.Cipher.SetKey(Key);
        }
    }
}
