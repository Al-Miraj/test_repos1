using System.ComponentModel.DataAnnotations;

public static class CustomerManagement
{
    public static void Display()
    {
        List<string> options = new List<string>()
        {
            "Customer Overview",
            "View Customer Details",
            "Add Customer",
            "Update Customer",
            "Delete Customer",
            "Back to Admin Dashboard"
        };
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Customer Account Management");
            int selectedOption = MenuSelector.RunMenuNavigator(options);
            List<Action> actions = new() { CustomerOverview, ViewCustomer, AddCustomer, UpdateCustomer, DeleteCustomer };
            Console.Clear();
            if (selectedOption == 5) { return; }
            actions[selectedOption]();
            Restaurant.UpdateRestaurantFiles();
            Console.WriteLine("\n[Press any key to return to the Customer Account Management menu.]");
            Console.ReadKey();
        }
    }


    private static void AddCustomer()
    {
        Console.WriteLine("Add Customer\n");
        Console.WriteLine("Enter customer details: ");
        CustomerAccount customer = LoginSystem.CreateCustomerAccount();
        Console.WriteLine("Customer added!");
        Console.WriteLine(customer.ToString());
    }

    private static void ViewCustomer()
    {
        CustomerAccount customer = GetCustomer();
        Console.WriteLine("\nCustomer Info:\n");
        List<string> options = new()
        {
            "Update",
            "Delete",
            "Back"
        };
        List<Action> actions = new() { UpdateCustomer, DeleteCustomer };
        while (true)
        {
            int selectedOption = MenuSelector.RunMenuNavigator(options);
            if (selectedOption == 2) { return; }
            actions[selectedOption]();
        }
    }
    private static void CustomerOverview()
    {
        List<CustomerAccount> customers = Restaurant.CustomerAccounts;
        Console.WriteLine("Customer Overview");
        Console.WriteLine();
        if (customers.Count == 0)
        {
            Console.WriteLine("There are no customers yet");
            return;
        }
        foreach (CustomerAccount customer in customers)
        {
            Console.WriteLine(customer.ToString());
            Console.WriteLine();
        }
    }

    private static void UpdateCustomer()
    {
        Console.WriteLine("Update Customer details\n");
        if (Restaurant.CustomerAccounts.Count == 0) { Console.WriteLine("There are not customer accounts yet."); return; }
        CustomerAccount customer = GetCustomer();
        List<string> updateOptions = new List<string>() { "Update name", "Update email", "Update password", "Done"};
        string ogCustomerDetails = customer.ToString();
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"\nCurrent customer details:\n");
            Console.WriteLine(customer.ToString());
            Console.WriteLine("\nUpdate:");
            int selectedOption = MenuSelector.RunMenuNavigator(updateOptions);
            switch (selectedOption)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("Update your name");
                    customer.Name = LoginSystem.GetAccountName();
                    break;
                case 1:
                    Console.Clear();
                    Console.WriteLine("Update your email");
                    customer.Email = LoginSystem.GetAccountEmail();
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("Update your password");
                    customer.Password = LoginSystem.GetAccountPassword(true);
                    break;
                case 3:
                    Console.WriteLine("Customer updated!");
                    Console.WriteLine("Before\n" + ogCustomerDetails + "\nAfter\n" + customer.ToString());
                    return;
                default:
                    break;
            }
        }
    }

    private static void DeleteCustomer()
    {
        CustomerAccount customer = GetCustomer();
        Console.WriteLine("Are you sure you want to delete this customer account?");
        int selectedOption = MenuSelector.RunMenuNavigator(new List<string>() { "Yes", "No" });
        if (selectedOption == 0)
        {
            bool removed = Restaurant.Accounts.Remove(customer);  // todo: reduce this body to just this line if removing always goes succesfull.
            if (removed)
            { Console.WriteLine("Customer account deleted succesfully!"); }
            else
            { Console.WriteLine("Something went wrong. Customer account was not deleted. Contact us for more information."); }//todo: figure out why it may not go well
        }
        else
        { Console.WriteLine("This customer account will not be deleted."); }
    }

    private static CustomerAccount GetCustomer()
    {
        List<string> options = Restaurant.CustomerAccounts.Select(c => c.ToString()).ToList();
        options.Add("Exit");
        int selectedOption = MenuSelector.RunMenuNavigator(options);
        if (selectedOption == options.Count - 1) { OptionMenu.UserDashboard!.RunDashboardMenu(); }
        return Restaurant.CustomerAccounts[selectedOption];
    }
}