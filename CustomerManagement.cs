using System.ComponentModel.DataAnnotations;

public static class CustomerManagement
{
    public static AdminAccount CurrentAdmin { private get; set; }

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
            switch (selectedOption)
            {
                case 0:
                    CustomerOverview();
                    break;
                case 1:
                    ViewCustomer();
                    break;
                case 2:
                    AddCustomer();
                    break;
                case 3:
                    UpdateCustomer();
                    break;
                case 4:
                    DeleteCustomer();
                    break;
                case 5:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    Console.ReadLine();
                    break;
            }

            Restaurant.UpdateRestaurantFiles();
            Console.WriteLine("\n[Press any key to return to the Customer Account Management menu.]");
            Console.ReadKey();
        }
    }


    private static void AddCustomer()
    {
        // call LoginSystem.Register()
        Console.WriteLine("Add Customer");
        Console.WriteLine();
        Console.WriteLine("Enter customer details: ");
        string? name;
        do
        {
            Console.Write("Name: ");
            name = Console.ReadLine();
        } while (name == null);
        var (email, password) = LoginSystem.ReadUserInfo(true);
        int id = (Restaurant.Accounts != null) ? LoginSystem.GetLatestId() + 1 : 1;
        var customer = new CustomerAccount(id, name, email, password);
        LoginSystem.UpdateJson();
        Console.WriteLine("Customer added!");
        Console.WriteLine(customer.ToString());
        Console.ReadLine();
    }

    private static void ViewCustomer()
    {
        Console.WriteLine("See Customer Overview for list of customers");
        Console.WriteLine("Enter customer ID: ");
        int id;
        while (true)
        {
            string input = Console.ReadLine()!;
            if (!input.All(char.IsDigit))
            { Console.WriteLine("Customer IDs must only contain digits."); continue; }
            id = Convert.ToInt32(input);
            break;
        }
        CustomerAccount customer = Restaurant.CustomerAccounts.FirstOrDefault(c => c.Id == id);
        if (customer == null)
        {
            Console.WriteLine("Customer not found");
            return;
        }
        Console.WriteLine("Customer Info:");
        Console.WriteLine(customer.ToString());
        Console.ReadLine();
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
        Console.ReadLine();
    }

    private static void UpdateCustomer()
    {
        Console.WriteLine("Enter customer id: ");
        int id = Convert.ToInt32(Console.ReadLine());
        var customer = CurrentAdmin.GetAccounts()?.FirstOrDefault(c => c.Id == id);
        if (customer == null)
        {
            Console.WriteLine("Customer not found");
            return;
        }
        string custStr = customer.ToString();
        Console.WriteLine(custStr);
        (string email, string password) = LoginSystem.ReadUserInfo(false);
        customer.Email = email;
        customer.Password = password;
        LoginSystem.UpdateJson();
        Console.WriteLine("Customer updated!");
        Console.WriteLine("Before\n" + custStr + "After\n" + customer.ToString());
        Console.ReadLine();
    }

    private static void DeleteCustomer()
    {
        Console.WriteLine("See Customer Overview for list of customers");
        Console.WriteLine("Enter customer id: ");
        int id = Convert.ToInt32(Console.ReadLine());
        var customer = CurrentAdmin.GetAccounts()?.FirstOrDefault(c => c.Id == id);
        if (customer == null)
        {
            Console.WriteLine("Customer not found");
            return;
        }
        CurrentAdmin.GetAccounts()?.Remove(customer);
        LoginSystem.UpdateJson();
        Console.WriteLine("Customer deleted!");
        Console.ReadLine();
    }
}