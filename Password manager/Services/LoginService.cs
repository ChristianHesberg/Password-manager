﻿using System.Text;
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
        var verification = _encryptionDecryptionUtils.Pbkdf2KeyDerivation(password, passwordHash.PasswordSalt);

        if (passwordHash.HashedPassword != Convert.ToBase64String(verification.key)) return false;
        return true;
    }

    public byte[] GenerateVaultKey(string password)
    {
        var passwordHash = _repo.GetPasswordHash();
        var key = _encryptionDecryptionUtils.Pbkdf2KeyDerivation(password, passwordHash.KeySalt);
        return key.key;
    }

    public bool CheckForAccount()
    {
        try
        {
            var passwordHash = _repo.GetPasswordHash();
        }
        catch(InvalidOperationException exception)
        {
            return false;
        }
        return true;
    }

    public void CreateAccount(string password)
    {
        var passwordSalt = _encryptionDecryptionUtils.GenerateSalt(16);
        var keySalt = _encryptionDecryptionUtils.GenerateSalt(16);
        var passwordHash = _encryptionDecryptionUtils.Pbkdf2KeyDerivation(password, Convert.ToBase64String(passwordSalt));

        var passwordHashEntity = new PasswordHash()
        {
            HashedPassword = Convert.ToBase64String(passwordHash.key),
            KeySalt = Convert.ToBase64String(keySalt),
            PasswordSalt = Convert.ToBase64String(passwordSalt)
        };
        
        _repo.AddPasswordHash(passwordHashEntity);
    }
}