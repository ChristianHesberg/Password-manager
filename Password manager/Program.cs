// See https://aka.ms/new-console-template for more information


using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using Password_manager;
using Password_manager.Entities;
using Password_manager.Repo;

var username = "peter";
var password = "p@ssw0rd1!";

Client client = new Client();
var res = client.GenerateVaultKey(username, password);
Console.WriteLine(Convert.ToBase64String(res));
using (var db = new VaultContext())
{
    db.Add(new PasswordHash() { HashedPassword = "password", Salt = "salt"});
    db.Add(new VaultItem { EncryptedPassword = "password", Salt = "salt", Url = "url" });
    db.SaveChanges();
}