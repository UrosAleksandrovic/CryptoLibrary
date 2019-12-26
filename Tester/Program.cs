using System;
using System.Text;
using CryptoLibrary;
using System.Security.Cryptography;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            string Test = "__Ovo je string test!!__";
            byte[] TestStringAsBytes = Encoding.ASCII.GetBytes(Test);
            byte[] Key = KeyGenerator.GenerateKey(TestStringAsBytes.Length);
            byte[] EncryptedData, DecryptedData;

            ICipher OtpCipher = new OTPCipher(Key);
            EncryptedData = OtpCipher.Encrypt(TestStringAsBytes);
            Console.WriteLine(Encoding.ASCII.GetString(EncryptedData));
            DecryptedData = OtpCipher.Decrypt(EncryptedData);
            Console.WriteLine(Encoding.ASCII.GetString(DecryptedData));

            Console.WriteLine();
            Key = KeyGenerator.Generate128BitKey();
            ICipher TeaCipher = new TEACipher(Key);
            EncryptedData = TeaCipher.Encrypt(TestStringAsBytes);
            Console.WriteLine(Encoding.ASCII.GetString(EncryptedData));
            DecryptedData = TeaCipher.Decrypt(EncryptedData);
            Console.WriteLine(Encoding.ASCII.GetString(DecryptedData));

            Console.WriteLine();
            ICipherMode ModeTester = new CipherFeedbackMode(TeaCipher);
            ModeTester.Key = Key;
            ModeTester.DataBlockSize = (uint)TestStringAsBytes.Length;
            string IV = "012345678901234567891234";
            byte[] IVAsBytes = Encoding.ASCII.GetBytes(IV);
            ModeTester.InitializationVector = IVAsBytes;
            EncryptedData = ModeTester.Encrypt(TestStringAsBytes);
            Console.WriteLine(Encoding.ASCII.GetString(EncryptedData));
            DecryptedData = ModeTester.Decrypt(EncryptedData);
            Console.WriteLine(Encoding.ASCII.GetString(DecryptedData));

            Console.WriteLine();
            string s = "_Test string za testiranje hash!";
            byte[] StringAsBytes = Encoding.Unicode.GetBytes(s);
            for (int i = 0; i < StringAsBytes.Length; i++)
                Console.Write("  {0}", StringAsBytes[i]);
            Console.WriteLine("\n\n");
            byte[] HashData = SHA1Provider.Hash(StringAsBytes);
            for (int i = 0; i < HashData.Length; i++)
                Console.Write("  {0}", HashData[i]);

            Console.ReadKey();

        }
    }
}
