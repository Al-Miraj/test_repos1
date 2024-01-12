public static class AdminManagement
{
    public static void Display()
    {
        Console.Clear();
        List<string> options = new()
        {
            "Add Admin",
            "Remove Admin",
            "Admin Overview"
        };
        int option = MenuSelector.RunMenuNavigator(options);
        switch (option)
        {
            case 0: AddAdmin(); break;
            case 1: RemoveAdmin(); break;
            case 2: AdminAccountsOverview(); break;
        }
    }
    private static void AddAdmin()
    {
        string name = LoginSystem.GetAccountName();
        string email = LoginSystem.GetAccountEmail();
        string password = LoginSystem.GetAccountPassword(false);
        int id = (Restaurant.Accounts.Count > 1) ? LoginSystem.GetLatestId() + 1 : 2;
        AdminAccount adminAccount = new AdminAccount(id, name, email, password);
        Restaurant.Accounts.Add(adminAccount);
        Restaurant.UpdateRestaurantFiles();
        Console.WriteLine("Account creation successful!");
    }

    private static void RemoveAdmin()
    {
        List<AdminAccount> adminAccounts = Restaurant.GetAdminAccounts();
        Console.WriteLine("Existing Admin Accounts:");
        foreach (var adminAccount in adminAccounts)
        {
            Console.WriteLine(adminAccount.ToString());
        }
        Console.Write("Enter the id of the admin account to be removed: ");
        AdminAccount toBeRemovedAdminAccount;
        try
        {
            int id = int.Parse(Console.ReadLine());
            toBeRemovedAdminAccount = adminAccounts.FirstOrDefault(admin => admin.ID == id);
            if (toBeRemovedAdminAccount != default)
            {
                Restaurant.Accounts.Remove(toBeRemovedAdminAccount);
                Restaurant.UpdateRestaurantFiles();

                Console.WriteLine($"Admin account with the id '{id}' has been removed.");
            }
            else
            {
                Console.WriteLine($"Admin account with the id '{id}' not found.");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }

    private static void AdminAccountsOverview()
    {
        Console.Clear();
        List<SuperAdminAccount> superAdminAccounts = Restaurant.GetSuperAdminAccounts();
        List<AdminAccount> adminAccounts = Restaurant.GetAdminAccounts();
        superAdminAccounts.ForEach(Console.WriteLine);
        adminAccounts.ForEach(Console.WriteLine);
        Console.WriteLine("\n[Press any key to return to the User Dashboard.]");
        Console.ReadKey();

    }

    
}