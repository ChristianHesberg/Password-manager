using System.Text;
using Password_manager.Entities;
using Password_manager.Repo;
using Password_manager.Utils;

namespace Password_manager.Services;

public class LoginService
{
    
    private readonly Repository _repo;
    private readonly EncryptionDecryptionUtils _encryptionDecryptionUtils;

    public LoginService(Repository repo, EncryptionDecryptionUtils utils)
    {
        _repo = repo;
        _encryptionDecryptionUtils = utils;
    }
    
    public bool Login(string password)
    {
        var passwordHash = _repo.GetPasswordHash();
        var verification = _encryptionDecryptionUtils.GenerateVaultKey(password, passwordHash.PasswordSalt);

        if (Encoding.UTF8.GetBytes(passwordHash.HashedPassword) != verification.key) return false;
        return true;
    }

    public byte[] GenerateVaultKey(string password)
    {
        var passwordHash = _repo.GetPasswordHash();
        var key = _encryptionDecryptionUtils.GenerateVaultKey(password, passwordHash.KeySalt);
        return key.key;
    }

    public void CreateAccount(string password)
    {
        var passwordSalt = _encryptionDecryptionUtils.GenerateSalt(16);
        var keySalt = _encryptionDecryptionUtils.GenerateSalt(16);
        var passwordHash = _encryptionDecryptionUtils.GenerateVaultKey(password, Convert.ToBase64String(passwordSalt));

        var passwordHashEntity = new PasswordHash()
        {
            HashedPassword = Convert.ToBase64String(passwordHash.key),
            KeySalt = Convert.ToBase64String(keySalt),
            PasswordSalt = Convert.ToBase64String(passwordSalt)
        };
        
        _repo.AddPasswordHash(passwordHashEntity);
    }
}