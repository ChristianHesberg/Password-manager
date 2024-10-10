using System.Security.Cryptography;
using Password_manager.Entities;
using Password_manager.Services;

namespace Password_manager.Utils;

public class ConsoleAppUtils
{
    private readonly LoginService _loginService;
    private readonly VaultService _vaultService;

    public ConsoleAppUtils(LoginService loginService, VaultService vaultService)
    {
        _loginService = loginService;
        _vaultService = vaultService;
    }

    public string ReadPassword()  
    {  
        string password = string.Empty;  
        ConsoleKeyInfo keyInfo;  
              
        do  
        {  
            keyInfo = Console.ReadKey(intercept: true);  
            if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)  
            {  
                password += keyInfo.KeyChar;  
                Console.Write("*"); // Display asterisk for each character entered  
            }  
            else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)  
            {  
                password = password.Substring(0, password.Length - 1);  
                Console.Write("\b \b"); // Handle backspace  
            }  
        } while (keyInfo.Key != ConsoleKey.Enter);  
              
        Console.WriteLine();  
        return password;  
    }
    
    public int DisplayMenu(string [] options)  
    {  
        
        int selectedIndex = 0;  
  
        ConsoleKeyInfo keyInfo;  
        do  
        {  
            Console.Clear();  
            Console.WriteLine("Use the arrow keys to select an option, then press Enter:");  
  
            for (int i = 0; i < options.Length; i++)  
            {  
                if (i == selectedIndex)  
                {  
                    Console.BackgroundColor = ConsoleColor.Gray;  
                    Console.ForegroundColor = ConsoleColor.Black;  
                }  
                Console.WriteLine(options[i]);  
                Console.ResetColor();  
            }  
  
            keyInfo = Console.ReadKey(intercept: true);  
            if (keyInfo.Key == ConsoleKey.UpArrow)  
            {  
                selectedIndex = (selectedIndex == 0) ? options.Length - 1 : selectedIndex - 1;  
            }  
            else if (keyInfo.Key == ConsoleKey.DownArrow)  
            {  
                selectedIndex = (selectedIndex == options.Length - 1) ? 0 : selectedIndex + 1;  
            }  
  
        } while (keyInfo.Key != ConsoleKey.Enter);  
  
        return selectedIndex;  
    }  
    
    public void EnterNewPasswordUrlPair(byte[] key)  
    {  
        Console.Clear();  
        Console.WriteLine("Enter the URL:");  
        string url = Console.ReadLine();
        
        string[] options = { "Generate password", "Enter password" };
        var result = DisplayMenu(options);

        string password = "";
        if (result == 0)
        {
            var passBytes = new byte[32];
            RandomNumberGenerator.Fill(passBytes);
            password = Convert.ToBase64String(passBytes);
            Console.WriteLine(password);
        } else if (result == 1)
        {
            Console.WriteLine("Enter the password:");
            password = Console.ReadLine();  
        }
  
        var passwordEntry = new Entry()  
        {  
            Url = url,  
            Password = password
        };
        
        _vaultService.AddEntry(passwordEntry, key);
  
        Console.WriteLine("Password/URL pair saved.");  
    } 

    public string EnterValidPassword()
    {
        Console.Write("Please enter your password: ");  
        var password = ReadPassword();
        if (!_loginService.Login(password))
        {
            Console.WriteLine("password invalid");
            EnterValidPassword();
        }
        return password;
    }
}