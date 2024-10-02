// See https://aka.ms/new-console-template for more information


using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using Password_manager;
using Password_manager.Entities;
using Password_manager.Repo;
using Password_manager.Services;
using Password_manager.Utils;

var username = "peter";
var password = "p@ssw0rd1!";

EncryptionDecryptionUtils encryptionService = new EncryptionDecryptionUtils();
var res = encryptionService.GenerateVaultKey(password, Constants.SaltInBase64);
Console.WriteLine(Convert.ToBase64String(res.key));
Console.WriteLine(Convert.ToBase64String(res.salt));

var url = "bob.com";
var passwordForUrl = "pass4bob";

var serializedPayload = encryptionService.SerializePayload(url, passwordForUrl);
var encryptedData = encryptionService.Encrypt(serializedPayload, res.key);

Console.WriteLine($"cipherText: {Convert.ToBase64String(encryptedData.ciphertext)}");
Console.WriteLine($"nonce: {Convert.ToBase64String(encryptedData.nonce)}");
Console.WriteLine($"tag: {Convert.ToBase64String(encryptedData.tag)}");

var decryptedData =
    encryptionService.Decrypt(encryptedData.ciphertext, encryptedData.nonce, encryptedData.tag, res.key);

Console.WriteLine($"plainText: {decryptedData}");

var parsedData = encryptionService.ExtractPartsFromData(decryptedData);

Console.WriteLine($"url: {parsedData.url}");
Console.WriteLine($"password: {parsedData.password}");
/*using (var db = new VaultContext())
{
    db.Add(new PasswordHash() { HashedPassword = "password", Salt = "salt"});
    db.Add(new VaultItem { EncryptedPassword = "password", Salt = "salt", Url = "url" });
    db.SaveChanges();
}*/