using System.Security.Cryptography;
using System.Text;
using Password_manager.Repo;

namespace Password_manager.Services;

public class EncryptionService
{
    private readonly VaultContext _db;

    public EncryptionService(VaultContext db)
    {
        this._db = db;
    }

    public (byte[] key, byte[] salt) GenerateVaultKey(string password, string salt, int keySize = 32, int saltSize = 16, int iterations = 10000)  
    {  
        // Generate a unique salt  
        var encodedSalt = Convert.FromBase64String(salt);

        // Derive the key using PBKDF2  
        using var pbkdf2 = new Rfc2898DeriveBytes(password, encodedSalt, iterations, HashAlgorithmName.SHA512);
        var key = pbkdf2.GetBytes(keySize);  
        return (key, encodedSalt);
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
    
    public string Decrypt(byte[] ciphertext, byte[] nonce, byte[] tag, byte[] key)
    {
        using var aes = new AesGcm(key, 16);
        var plaintextBytes = new byte[ciphertext.Length];

        aes.Decrypt(nonce, ciphertext, tag, plaintextBytes);

        return Encoding.UTF8.GetString(plaintextBytes);
    }

    public string SerializePayload(string url, string password)
    {
        return $"{url}|{password}";
    }
    
    public (string url, string password) ExtractPartsFromData(string data)  
    {  
        // Split the string using the delimiter  
        var parts = data.Split('|');  
        if (parts.Length != 2)  
        {  
            throw new ArgumentException("The data string is not properly formatted.");  
        } 

        return (parts[0], parts[1]);  
    }
}