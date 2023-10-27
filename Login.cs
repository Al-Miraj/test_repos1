using Newtonsoft.Json;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ComponentModel;

/*static class LoginSystem
{
    public static List<Account>? Accounts;
    public static List<Reservation>? Reservations;

    static LoginSystem()
    {
        using StreamReader reader = new("accounts.json");
        string json = reader.ReadToEnd();
        Accounts = JsonConvert.DeserializeObject<List<Account>>(json);
        using StreamReader reader2 = new("reservations.json");
        json = reader2.ReadToEnd();
        Reservations = JsonConvert.DeserializeObject<List<Reservation>>(json);
    }

    public static void Login()
    {
        try
        {
            Console.WriteLine("Sign In");
            Console.WriteLine("For register menu, enter 'R'. Otherwise just enter.");
            string input = Console.ReadLine().ToLower();
            if (input == "r")
            {
                Register();
            }
            while (true)
            {
                Console.WriteLine("Log in screen");
                Console.WriteLine();
                var (email, password) = ReadUserInfo();
                if (Accounts.Any(account => account.Email == email && account.Password == password))
                {
                    Console.WriteLine("Login successful!");
                    return;
                }
                Console.WriteLine("Login failed. Try again.");
            }
        }
        catch (Exception)
        {
            Console.WriteLine($"An error occurred: ");
        }
    }


    private static int GetLatestId()
    {
        List<int> ids = new();
        foreach (var account in Accounts)
        {
            ids.Add(account.Id);
        }
        return ids.Max();
    }

    private static (string, string) ReadUserInfo()
    {
        while (true)
        {
            Console.Write("\nEmail: ");
            string email = Console.ReadLine();
            if (IsEmail(email))
            {
                Console.Write("\nPassword: ");
                string password = Console.ReadLine();
                return (email, password);
            }
            Console.WriteLine("Please enter a valid email. (i.e. john@email.com");
        }
    }

    private static bool IsEmail(string text)
    {
        try // Met System.Net.Mail kijken of de tekst in de goede format is
        {
            var mailAddress = new MailAddress(text);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static void UpdateJson()
    {
        using StreamWriter writer = new("accounts.json");
        string json = JsonConvert.SerializeObject(Accounts);
        writer.Write(json);
    }

    public static void Register()
    {
        try
        {
            var (email, password) = ReadUserInfo();
            int id = (Accounts != null) ? GetLatestId() + 1 : 1;
            var account = new Account(id, email, password);
            Accounts.Add(account);
            UpdateJson();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occured: ");
        }
        
    }
}*/
