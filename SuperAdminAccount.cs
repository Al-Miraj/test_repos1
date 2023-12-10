﻿using Newtonsoft.Json;
using System.Security.Principal;


public sealed class SuperAdminAccount : Account
{
    public SuperAdminAccount(int id, string name, string email, string password) : base(id, name, email, password)
    {
    }

    public SuperAdminAccount() : base() { }

    public override List<Reservation> GetReservations()
    {
        return Restaurant.Reservations;
    }

    public List<Account>? GetAccounts()
    {
        return Restaurant.Accounts;
    }

    //public static string AccountsXmlFileName = "Accounts.xml";
    //public static void SuperAdminCanAddAdmin(int id, string name, string email, string password)
    //{
    //    id = (Restaurant.Accounts.Count > 1) ? LoginSystem.GetLatestId() + 1 : 2;
    //    name = LoginSystem.GetAccountName();
    //    email = LoginSystem.GetAccountEmail();
    //    password = LoginSystem.GetAccountPassword(true);


    //    List<Account> adminAccounts;
    //    AdminAccount adminAccount = new AdminAccount(id, name, email, password);
    //    adminAccounts = new List<Account>() { adminAccount };
    //    XmlFileHandler.WriteToFile(adminAccounts, AccountsXmlFileName);
    //}

    public static string GetAccountPassword()
    {
        string password;
        { LoginSystem.DisplayPasswordRequirements(); }
        Console.WriteLine("Please enter the password: ");
        while (true)
        {
            password = Console.ReadLine()!;
            if (!LoginSystem.IsValidPassword(password))
            {
                Console.WriteLine("Please enter a valid password.");
                continue;
            }
            break;
        }
        return password;
    }
    public static string AccountsXmlFileName = "Accounts.xml";

    public static void SuperAdminCanAddAdmin()
    {
        // Call existing methods to get name, email, and password
        string name = LoginSystem.GetAccountName();
        string email = LoginSystem.GetAccountEmail();
        string password = GetAccountPassword();


        // Retrieve existing admin accounts
        List<Account> adminAccounts = XmlFileHandler.ReadFromFile<Account>(AccountsXmlFileName);

        // Generate a new ID if not provided
        int id = (Restaurant.Accounts.Count > 1) ? LoginSystem.GetLatestId() + 1 : 2;
        // Create a new AdminAccount
        AdminAccount adminAccount = new AdminAccount(id, name, email, password);

        // Add the new admin to the list
        adminAccounts.Add(adminAccount);

        // Write the updated list to the XML file
        XmlFileHandler.WriteToFile(adminAccounts, AccountsXmlFileName);

        Console.WriteLine("Account creation successful!");
        //return adminAccounts;
    }

    public static void SuperAdminCanRemoveAdmin()
    {
        // Retrieve all accounts from the accounts file
        List<Account> allAccounts = XmlFileHandler.ReadFromFile<Account>(AccountsXmlFileName);

        // Filter so that only admin accounts remain
        List<AdminAccount> adminAccounts = allAccounts.OfType<AdminAccount>().ToList();

        // Display existing admin accounts
        Console.WriteLine("Existing Admin Accounts:");
        foreach (var adminAccount in adminAccounts)
        {
            Console.WriteLine($"ID: {adminAccount.ID}, Name: {adminAccount.Name}, Email: {adminAccount.Email}");
        }

        Console.Write("Enter the email of the admin account to be removed: ");
        string emailFromToBeDeletedAdmin = Console.ReadLine();

        // Find the admin account with the provided email
        AdminAccount toBeRemovedAdminAccount = adminAccounts.FirstOrDefault(mail => mail.Email == emailFromToBeDeletedAdmin);

        if (toBeRemovedAdminAccount != null)
        {
            // Remove the admin account from the list
            allAccounts.Remove(toBeRemovedAdminAccount);

            // Update the XML file with the modified list
            XmlFileHandler.WriteToFile(allAccounts, AccountsXmlFileName);

            Console.WriteLine($"Admin account with the email '{emailFromToBeDeletedAdmin}' has been removed.");
        }
        else
        {
            Console.WriteLine($"Admin account with the email '{emailFromToBeDeletedAdmin}' not found.");
        }
    }




    public static void AdminAccountsOverview()
    {

        //List<SuperAdminAccount> superAdminAccounts = Restaurant.GetSuperAdminAccount();


        // Read all accounts from the XML file
        List<Account> allAccounts = XmlFileHandler.ReadFromFile<Account>(AccountsXmlFileName);

        // Filter accounts by account type to ensure only admin coounts are retrieved
        List<SuperAdminAccount> superAdminAccounts = allAccounts.OfType<SuperAdminAccount>().ToList();
        List<AdminAccount> adminAccounts = allAccounts.OfType<AdminAccount>().ToList();



        foreach (var superAdminAccount in superAdminAccounts)
        {
            Console.WriteLine(superAdminAccount);
        }


        //List<AdminAccount> adminAccounts = Restaurant.GetAdminAccounts();

        foreach (var adminAccount in adminAccounts)
        {
            Console.WriteLine(adminAccount);
        }
    }
}

