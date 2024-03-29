﻿namespace CryptoLibrary
{
    public interface ICipher
    {
        byte[] Encrypt(byte[] DataToEncript);

        byte[] Decrypt(byte[] EncriptedData);

        void SetKey(byte[] Key);
    }
}
