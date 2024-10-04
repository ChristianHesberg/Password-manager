using Password_manager.Services;

namespace Password_manager.Utils;

public class ConsoleAppUtils
{
    private readonly LoginService _loginService;

    public ConsoleAppUtils(LoginService loginService)
    {
        _loginService = loginService;
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
    
    public int DisplayMenu()  
    {  
        string[] options = { "Get Passwords", "Enter New Password/URL Pair" };  
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