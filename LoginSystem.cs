using Newtonsoft.Json;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Data;


static class LoginSystem
{
    public static void Start()
    {
        Console.WriteLine("Welcome!\n\nWhat do you want to do:");
        List<string> LoginRegisterOptions = new List<string>() { "Log into existing account", "Create new account" };
        int selectedOption = MenuSelector.RunMenuNavigator(LoginRegisterOptions);
        switch (selectedOption)
        {
            case 0: Console.Clear(); Login(); break;
            case 1: Console.Clear(); Register(); break;
        }
    }

    public static void Login()
    {
        int attempts = 0;
        do
        {
            Console.Clear();
            Console.WriteLine("Log in screen\n");
            if (Restaurant.Accounts == null)
            {
                Console.WriteLine("There aren't any accounts registered yet. Please consider registering.");
                return;
            }
            (string email, string password) = ReadUserInfo(false);
            Account account = FindAccount(email, password);
            if (account != null)
            {
                Console.WriteLine("Logging in...");
                var dashboard = new Dashboard(account);
                ConnectUser(account, dashboard);
                Console.WriteLine("Login successful!");
                Console.WriteLine("\n[Press any key to continue to dashboard.]");
                Console.ReadKey();
                dashboard.RunDashboardMenu();
            }
            else
            {
                Console.WriteLine($"Login failed. You have {2 - attempts} left"); // todo give reason why login failed
                attempts++;
            }
        } while (attempts < 3);
        Console.WriteLine("Too many failed attempts. Please try again later.");
    }

    public static void Register()
    {
        Console.WriteLine("Create account\n");

        try
        {
            string name;
            do
            {
                Console.Write("Please enter you name: ");
                name = Console.ReadLine()!;
                if (name.Length < 1)
                {
                    Console.WriteLine("Invalid input. Your name must at least be 1 character.\n");
                    continue;
                }
                else if (name.Any(char.IsDigit))
                {
                    Console.WriteLine("Invalid input. Your name must not contain any digits.\n");
                    continue;
                }
                Console.WriteLine();
                break;
            }
            while (true);

            

            (string email, string password) = ReadUserInfo(true);

            int id = (Restaurant.Accounts != null) ? GetLatestId() + 1 : 1;

            CustomerAccount account = new CustomerAccount(id, name, email, password);
            Restaurant.Accounts!.Add(account);
            XmlFileHandler.WriteToFile(Restaurant.Accounts, Restaurant.AccountsXmlFileName);

            Dashboard dashboard = new Dashboard(account);
            ConnectUser(account, dashboard);
            Console.WriteLine("\n[Press any key to continue to dashboard.]");
            Console.ReadKey();
            dashboard.RunDashboardMenu();
        }
        catch (NullReferenceException ex)
        {
            Console.WriteLine($"An error occured: {ex.Message}");
        }
    }

    public static void Logout()
    {
        Console.WriteLine("Logging out...");
        ResetMenu();
        Console.WriteLine("You're now logged out");
        Console.ReadLine();
        Menu.RunMenu();
    }

    private static void ResetMenu()
    {
        Menu.IsUserLoggedIn = false;
        Menu.CurrentUser = null;
        Menu.UserDashboard = null;
    }

    public static int GetLatestId() => Restaurant.Accounts != null ? Restaurant.Accounts.Max(account => account.Id) : 0;
    
    private static Account? FindAccount(string email, string password) => Restaurant.Accounts?.FirstOrDefault(account => account.Email == email && account.Password == password);
    
    public static (string, string) ReadUserInfo(bool register)
    {
        do
        {
            Console.WriteLine("Enter email and password\n");
            Console.Write("Email: ");
            string? email = Console.ReadLine();

            if (email != null && IsEmail(email))
            {
                if (register)
                {
                    Console.WriteLine("\nA password must meet the following requirements:");
                    Console.WriteLine("- The length of the password should be between 7 and 16 characters.");
                    Console.WriteLine("- The password should contain at least one uppercase letter.");
                    Console.WriteLine("- The password should contain at least one lowercase letter.");
                    Console.WriteLine("- The password should contain at least one digit.");
                    Console.WriteLine("- The password should contain at least one special character (such as !, @, #, $, %, etc.).");
                    Console.WriteLine("- The password should not contain any characters that are not digits, letters, or special characters.");
                }
                string? password;
                do
                {
                    Console.Write("\nPassword: ");
                    password = Console.ReadLine();
                    bool isValidPassword = IsValidPassword(password);
                    if (!isValidPassword)
                    {
                        Console.WriteLine("Please enter a valid password.");
                        continue;
                    }
                    break;
                } while (true);
                return (email, password)!;
            }
            else
            {
                Console.WriteLine("Please enter a valid email. (i.e. john@email.com");
            }
        } while (true);
    }

    public static bool IsValidPassword(string? password)
    {
        if (password != null)
        {
            bool result = 
                password.Length >= 7 
                && password.Length <= 16
                && Regex.IsMatch(password, "[A-Z]")
                && Regex.IsMatch(password, "[a-z]")
                && Regex.IsMatch(password, @"\d")
                && Regex.IsMatch(password, @"[!-/:-@\[-_{-~]")
                && !Regex.IsMatch(password, @"[^\dA-Za-z!-/:-@\[-_{-~]");
            return result;
        }
        else
        {
            return false;
        }
    }

    public static bool IsEmail(string? text)
    {
        if (text =="")
        {
            return false;
        }
        try                                             // Met System.Net.Mail kijken of de tekst in de goede format is
        {
            var mailAddress = new MailAddress(text);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public static void UpdateJson()  // todo check what references this method
    {
        using StreamWriter writer = new("accounts.json");
        string json = JsonConvert.SerializeObject(Restaurant.Accounts);
        writer.Write(json);
    }

    private static void ConnectUser(Account account, Dashboard dashboard)
    {
        Menu.IsUserLoggedIn = true;
        Menu.CurrentUser = account;
        Menu.UserDashboard = dashboard;
    }

}