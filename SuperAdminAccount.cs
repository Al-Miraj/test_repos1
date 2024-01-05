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
        var existingAdmin = Restaurant.AdminAccounts.Any(a => a.Email == email);
        if (existingAdmin)
        {
            Console.WriteLine("An admin with this email already exists.");
            return;
        }

        int newId = GenerateNewId();
        var newAdmin = new AdminAccount(newId, "New Admin", email, password);
        // Assuming you hash passwords
        Restaurant.AdminAccounts.Add(newAdmin);
        // You may want to persist this change to a database or file
        Console.WriteLine("Admin added successfully.");
    }


    public void RemoveAdmin(string email)
    {
        var adminToRemove = Restaurant.AdminAccounts.FirstOrDefault(a => a.Email == email);
        if (adminToRemove != null)
        {
            Restaurant.AdminAccounts.Remove(adminToRemove);
            // Update your data storage to reflect this change
            Console.WriteLine("Admin removed successfully.");
        }
        else
        {
            Console.WriteLine("Admin account not found.");
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
