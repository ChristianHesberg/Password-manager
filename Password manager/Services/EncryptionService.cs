using System.Security.Cryptography;
using System.Text;

namespace Password_manager.Services;

public class EncryptionService
{
    public byte[] GenerateVaultKey(string password, byte[] salt)
    {
        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, 5000, HashAlgorithmName.SHA512);
        return pbkdf2.GetBytes(16);
    }
    
    public (byte[] ciphertext, byte[] nonce, byte[] tag) Encrypt(string plaintext, byte[] key)
    {
        using var aes = new AesGcm(key, 16);
        var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
        RandomNumberGenerator.Fill(nonce);

        var tag = new byte[AesGcm.TagByteSizes.MaxSize];

        var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
        var ciphertext = new byte[plaintextBytes.Length];

        aes.Encrypt(nonce, plaintextBytes, ciphertext, tag);

        return (ciphertext, nonce, tag);
    }
    
    public static string Decrypt(byte[] ciphertext, byte[] nonce, byte[] tag, byte[] key)
    {
        using var aes = new AesGcm(key, 16);
        var plaintextBytes = new byte[ciphertext.Length];

        aes.Decrypt(nonce, ciphertext, tag, plaintextBytes);

        return Encoding.UTF8.GetString(plaintextBytes);
    }
}