public class SuperAdminAccount : AdminAccount
{
    public SuperAdminAccount(int id, string name, string email, string password)
        : base(id, name, email, password) { }

    private int GenerateNewId()
    {
        // This is a placeholder. You should replace it with your actual ID generation logic.
        // For instance, it could be the next available integer or a more complex unique identifier.
        return Restaurant.AdminAccounts.Max(a => a.ID) + 1;
    }

    public static SuperAdminAccount GetDefaultSuperAdmin()
    {
        // Use the ID and name you want for your default superadmin account
        int defaultId = 1;
        string defaultName = "Super Admin";
        string defaultEmail = "superadmin@hotmail.com";
        string defaultPassword = "Admin-1234"; // In a real system, this password would need to be stored securely, not hardcoded

        return new SuperAdminAccount(defaultId, defaultName, defaultEmail, defaultPassword);
    }

    public void AddAdmin(string email, string password)
    {
        // Check if admin with this email already exists
        if (LoginSystem.Accounts.Any(a => a.Email == email))
        {
            Console.WriteLine("An admin with this email already exists.");
            return;
        }

        // Add the new admin
        int newId = GenerateNewId();
        string adminName = "New Admin"; // You need to decide how to set the name for new admins
        var newAdmin = new AdminAccount(newId, adminName, email, password);
        AdminAccount newAdmin = new AdminAccount(newId, email, password);
        LoginSystem.Accounts.Add(newAdmin);

        // Save the updated list of accounts
        XmlFileHandler.WriteToFile(LoginSystem.Accounts, "Accounts.xml");
        // Or for JSON:
        // Utensils.WriteJson("Accounts.json", LoginSystem.Accounts);

        Console.WriteLine("New admin added.");
    }


    public void RemoveAdmin(string email)
    {
        var adminToRemove = Restaurant.AdminAccounts.FirstOrDefault(a => a.Email == email);
        if (adminToRemove != null)
        {
            Restaurant.AdminAccounts.Remove(adminToRemove);
            Console.WriteLine("Admin removed successfully.");

            // Save the updated list to the file
            XmlFileHandler.WriteToFile(Restaurant.AdminAccounts, "AdminAccounts.xml");
            // If you're using JSON, use the Utensils class to write to a JSON file instead
            // Utensils.WriteJson("AdminAccounts.json", Restaurant.AdminAccounts);
        }
        else
        {
            Console.WriteLine("Admin account not found.");
        }
    }

    public void ListAdmins()
    {
        Console.WriteLine("Listing all admins:");
        foreach (var admin in Restaurant.AdminAccounts)
        {
            Console.WriteLine($"Email: {admin.Email}");
        }
    }







    public void ViewAdminOverview()
    {
        foreach (var admin in Restaurant.AdminAccounts)
        {
            string maskedPassword = new string('*', admin.Password.Length); // Mask the password
            Console.WriteLine($"ID: {admin.ID}, Name: {admin.Name}, Email: {admin.Email}, Password: {maskedPassword}");
        }
    }

}
