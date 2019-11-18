namespace CryptoLibrary
{
    public interface IKeyGenerator
    {
        
        byte[] Generate64BitKey();

        byte[] GenerateKey(int KeyLength);

        byte[] Generate128BitKey();
 
    }
}
