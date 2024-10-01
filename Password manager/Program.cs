// See https://aka.ms/new-console-template for more information


using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using Password_manager;
using Password_manager.Entities;
using Password_manager.Repo;
using Password_manager.Services;

var username = "peter";
var password = "p@ssw0rd1!";

EncryptionService encryptionService = new EncryptionService();
var res = encryptionService.GenerateVaultKey(password, Encoding.UTF8.GetBytes(Constants.SaltInBase64));
Console.WriteLine(Convert.ToBase64String(res));

var url = "bob.com";
var passwordForUrl = "pass4bob";

var vaultItem = new VaultItem
{
    Url = En
}
/*using (var db = new VaultContext())
{
    db.Add(new PasswordHash() { HashedPassword = "password", Salt = "salt"});
    db.Add(new VaultItem { EncryptedPassword = "password", Salt = "salt", Url = "url" });
    db.SaveChanges();
}*/