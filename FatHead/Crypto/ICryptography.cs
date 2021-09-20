
namespace FatHead.Crypto.Interfaces
{
    public interface ICryptography
    {
        string Decrypt(string cipherText);

        string Encrypt(string clearText);
    }
}
