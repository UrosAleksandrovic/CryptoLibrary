using System;
using System.Collections;

namespace CryptoLibrary
{
    public class Converter 
    {
        public static byte[] BitToByteArray(BitArray BitArrayToConvert)
        {
            byte[] ConvertedResault = new byte[(BitArrayToConvert.Length - 1) / 8 + 1];
            BitArrayToConvert.CopyTo(ConvertedResault, 0);
            return ConvertedResault;
        }


    }
}
