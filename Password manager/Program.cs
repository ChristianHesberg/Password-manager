// See https://aka.ms/new-console-template for more information


using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Password_manager;
using Password_manager.Entities;
using Password_manager.Repo;
using Password_manager.Services;
using Password_manager.Utils;

//var dbContextOptions = new DbContextOptionsBuilder<VaultContext>();
//dbContextOptions.UseSqlite("Data Source=passwordmanager.db");
using var db = new VaultContext();

var repo = new Repository(db);
var utils = new EncryptionDecryptionUtils();
var vaultService = new VaultService(repo, utils);
var loginService = new LoginService(repo, utils);
var appUtils = new ConsoleAppUtils(loginService, vaultService);

Console.WriteLine("Welcome to Password Manager!");

string password;
byte[] key;

if (!loginService.CheckForAccount())
{
    Console.WriteLine("No user found, please enter password for new user");
    password = appUtils.ReadPassword();
    
    loginService.CreateAccount(password);
}

password = appUtils.EnterValidPassword();
key = loginService.GenerateVaultKey(password);

string[] options = { "Get Passwords", "Enter New Password/URL Pair" };
while (true)
{
    var selected = appUtils.DisplayMenu(options);
    if (selected == 0)
    {
        var list = vaultService.GetEntries(key);
        foreach (var entry in list)
        {
            Console.WriteLine($"{entry.Url} - {entry.Password}");
            Console.WriteLine("-----------------------------");
        }
    }
    if (selected == 1)
    {
        appUtils.EnterNewPasswordUrlPair(key);
    }

    Console.WriteLine("Press any key to continue...");  
    Console.ReadKey(); 
    Console.Clear();
}
 


              
// Here you would call your authentication service to verify the password  
// For now, we'll just display the password entered for demonstration purposes  

              
// You can replace the above line with your login service call  
// bool isAuthenticated = AuthenticationService.Login(password);  
// if (isAuthenticated) { ... } else { ... }  
  



/*var username = "peter";
var password = "p@ssw0rd1!";

EncryptionDecryptionUtils encryptionService = new EncryptionDecryptionUtils();
var res = encryptionService.Pbkdf2KeyDerivation(password, Constants.SaltInBase64);
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