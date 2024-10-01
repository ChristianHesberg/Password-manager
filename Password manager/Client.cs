using System.Security.Cryptography;
using System.Text;

namespace Password_manager; 

public class Client
{
    public byte[] GenerateVaultKey(string username, string password)
    {
        var userPassString = $"{username}{password}";
        return Pbkdf2Hash(userPassString, Encoding.UTF8.GetBytes(Constants.SaltInBase64));
    }
    
    public static byte[] Pbkdf2Hash(string input, byte[] salt)
    {
        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(input, salt, 5000, HashAlgorithmName.SHA512);
        return pbkdf2.GetBytes(32);
    }
}