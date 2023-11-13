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
using static System.Net.WebRequestMethods;
using System.Diagnostics;

static class LoginSystem
{
    public static List<Account> Accounts = Utensils.ReadJson<List<Account>>("Accounts.json") ?? new List<Account>(); //  
    public static List<AdminAccount> AdminAccounts = Utensils.ReadJson<List<AdminAccount>>("Admins.json") ?? new List<AdminAccount>(); // 

    public static void Start()
    {
        Console.WriteLine("Sign In");
        Console.WriteLine("For register menu, enter 'R'. Otherwise just enter.");
        string input = Console.ReadLine()!.ToLower();
        if (input == "r")
        {
            Register();
        }
        else
        {
            Login();
        }
    }

    public static void Login()
    {
        Console.WriteLine("Log in screen");
        Console.WriteLine();
        int attempts = 0;
        do
        {
            var (email, password) = ReadUserInfo(false);
            if (Accounts == null)
            {
                Console.WriteLine("There aren't any accounts registered yet. Please consider registering.");
                return;
            }
            if (AdminAccounts == null)
            {
                Console.WriteLine("There aren't any Admins yet.");
                return;
            }
            if (Accounts.Any(account => account.Email.ToLower() == email.ToLower() && account.Password == password))
            {
                Console.WriteLine("Logging in...");
                var account = FindAccount(email, password);
                if (account != null)
                {
                    var admins = Utensils.ReadJson<List<AdminAccount>>("Admins.json");
                    if (admins!.Any(admin => admin.Email.ToLower().Equals(account.Email.ToLower()) && admin.Password.Equals(account.Password)))
                    {
                        Console.WriteLine("Admin found...");
                        account = (account as AdminAccount)!;
                    }
                    var dashboard = new Dashboard(account);
                    ConnectUser(account, dashboard);
                    Console.WriteLine("Login successful!");
                    Console.ReadLine();
                    dashboard.Display();
                }
            }
            else
            {
                Console.WriteLine("Login failed. You have  " + (3 - attempts) + " attempts left");
                attempts++;
            }
        } while (attempts < 3);
        Console.WriteLine("Too many failed attempts. Please try again later.");
    }

    public static void Register()
    {
        Console.WriteLine("Register");
        Console.WriteLine();
        try
        {
            string? name;
            do
            {
                Console.Write("Please enter your name: ");
                name = Console.ReadLine();
            } while (name == null);
            var (email, password) = ReadUserInfo(true);
            int id = (Accounts != null) ? GetLatestId() + 1 : 1;
            var account = new Account(id, name, email, password);
            UpdateJson();
            var dashboard = new Dashboard(account);
            ConnectUser(account, dashboard);
            Console.ReadLine();
            dashboard.Display();
        }
        catch (NullReferenceException ex)
        {
            Console.WriteLine($"An error occured: {ex.Message}");
        }
    }

    public static void Logout()
    {
        Console.WriteLine("Logging out...");
        Utensils.ResetMenu();
        Console.WriteLine("You're now logged out");
        Console.ReadLine();
        Menu.RunMenu();
    }

    public static int GetLatestId() => Accounts != null ? Accounts.Max(account => account.Id) : 0;
    private static Account? FindAccount(string email, string password) => Accounts?.FirstOrDefault(account => account.Email == email && account.Password == password);

    private static void ConnectUser(Account account, Dashboard dashboard)
    {
        Menu.IsUserLoggedIn = true; 
        Menu.CurrentUser = account; 
        Menu.UserDashboard = dashboard;
    }

    public static (string, string) ReadUserInfo(bool register)
    {
        do
        {
            Console.WriteLine("Enter email and password");
            Console.WriteLine();
            Console.Write("\nEmail: ");
            string? email = Console.ReadLine();

            if (email != null && IsEmail(email))
            {
                if (register == true)
                {
                    Console.WriteLine("A password must meet the following requirements:");
                    Console.WriteLine("- The length of the password should be between 7 and 16 characters.");
                    Console.WriteLine("- The password should contain at least one uppercase letter.");
                    Console.WriteLine("- The password should contain at least one lowercase letter.");
                    Console.WriteLine("- The password should contain at least one digit.");
                    Console.WriteLine("- The password should contain at least one special character (such as !, @, #, $, %, etc.).");
                    Console.WriteLine("- The password should not contain any characters that are not digits, letters, or special characters.");
                }
                do
                {
                    Console.Write("\nPassword: ");
                    string? password = Console.ReadLine();
                    if (password != null && IsValidPassword(password))
                    {
                        return (email, password);
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid password.");
                    }
                } while (true);
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
            bool result = password.Length >= 7 && password.Length <= 16
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
        if (text == null)
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

    public static void UpdateJson()
    {
        using StreamWriter writer = new("Accounts.json");
        string json = JsonConvert.SerializeObject(Accounts);
        writer.Write(json);
        using StreamWriter writer2 = new("Admins.json");
        json = JsonConvert.SerializeObject(AdminAccounts);
        writer2.Write(json);
    }


}