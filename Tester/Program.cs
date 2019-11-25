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
            string Test = "Ovo je test!";
            byte[] StringAsBytes = Encoding.ASCII.GetBytes(Test);
            string IV = "123456789012";
            byte[] IVASBYTES = Encoding.ASCII.GetBytes(IV);

            byte[] Key = KeyGenerator.GenerateKey(StringAsBytes.Length);
            ICipherMode myCipher = new CipherFeedbackMode(new OTPCipher());
            myCipher.Key = Key;
            myCipher.DataBlockSize = (uint)StringAsBytes.Length;
            myCipher.InitializationVector = IVASBYTES;

            byte[] EncryptedData = myCipher.Encrypt(StringAsBytes);

            Console.WriteLine(Encoding.ASCII.GetString(EncryptedData));

            byte[] DecryptedData = myCipher.Decrypt(EncryptedData);

            Console.WriteLine(Encoding.ASCII.GetString(DecryptedData));
            
            Console.ReadKey();

           /* string s = "0SAZEEsERWdV2bkVYtUU3PhTSJYWwwDr8fDhsRvPat4z1M40k2HLULHicx0paeUu";
            byte[] StringAsBytes = Encoding.UTF8.GetBytes(s);
            SHA1 provider = new SHA1CryptoServiceProvider();

            byte[] HashResault = provider.ComputeHash(StringAsBytes);
            byte[] HashData = SHA1Provider.Hash(StringAsBytes);

            Console.WriteLine(Encoding.UTF8.GetString(HashData));
            Console.WriteLine(Encoding.UTF8.GetString(HashResault));
            
            Console.ReadKey();*/
        }
    }
}
