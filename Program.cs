using MiniProject1_V1;

class Program
{
    public static void Main()
    {
        // Initialize a SuperAdminAccount for testing
        InitializeSuperAdmin();

        // Existing code
        OptionMenu.RunMenu();
    }

    private static void InitializeSuperAdmin()
    {
        // Add a SuperAdminAccount for testing
        // Replace the email and password with your desired credentials
        var superAdmin = new SuperAdminAccount(1, "Super Admin", "superadmin@email.com", "securepassword");
        Restaurant.SuperAdminAccounts.Add(superAdmin);

        // Any other initialization if needed
    }
}
