using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.ComponentModel.DataAnnotations;

public static class CustomerManagement
{
    public static void Display()
    {
        List<string> options = new List<string>()
        {
            "Customers",
            "New customer",
            "Back"
        };
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Customer Account Management");
            int selectedOption = MenuSelector.RunMenuNavigator(options);
            Console.Clear();
            if (selectedOption == 1)
                AddCustomer();
            else if (selectedOption == 2)
                return;
            else
                ViewCustomers();
            Restaurant.UpdateRestaurantFiles();
        }
    }


    private static void AddCustomer()
    {
        Console.WriteLine("Add Customer\n");
        Console.WriteLine("Enter customer details: ");
        CustomerAccount customer = LoginSystem.CreateCustomerAccount();
        Console.WriteLine("Customer added!");
        Console.WriteLine(customer.ToString());
        Console.WriteLine("\n[Press any key to return to the Customer Account Management menu.]");
        Console.ReadKey();
    }

    private static void ViewCustomers()
    {
        while (true)
        {
            Console.Clear();
            CustomerAccount? customer = GetCustomer();
            if (customer == null)
                return;
            Console.WriteLine("\nAction:\n");
            List<string> options = new()
        {
            "Update",
            "Delete",
            "Back"
        };
            while (true)
            {
                int selectedOption = MenuSelector.RunMenuNavigator(options);
                switch (selectedOption)
                {
                    case 0:
                        UpdateCustomer(customer);
                        return;
                    case 1:
                        DeleteCustomer(customer);
                        return;
                    default:
                        return;
                }
            }
        }
        
    }
    private static void UpdateCustomer(CustomerAccount customer)
    {
        Console.Clear();
        Console.WriteLine("Update Customer details\n");
        List<string> updateOptions = new List<string>() 
        { 
            "Name", 
            "Email", 
            "Password", 
            "Done"
        };
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
                    Console.Clear();
                    Console.WriteLine("Customer updated!");
                    Console.WriteLine("Before\n" + ogCustomerDetails + "\nAfter\n" + customer.ToString());
                    return;
                default:
                    break;
            }
        }
    }

    private static void DeleteCustomer(CustomerAccount customer)
    {
        Console.Clear();
        Console.WriteLine("Are you sure you want to delete this customer account?");
        int selectedOption = MenuSelector.RunMenuNavigator(new List<string>() { "Yes", "No" });
        if (selectedOption == 0)
        {
            Restaurant.Accounts.Remove(customer);  
            Restaurant.UpdateRestaurantFiles();
        }
        else
        { Console.WriteLine("This customer account will not be deleted."); }
    }

    private static CustomerAccount? GetCustomer()
    {
        List<string> options = Restaurant.CustomerAccounts.Select(c => c.ToString()).ToList();
        options.Add("Exit");
        int selectedOption = MenuSelector.RunMenuNavigator(options);
        if (selectedOption == options.Count - 1) { return null; }
        options.Remove(options.Last());
        return Restaurant.CustomerAccounts[selectedOption];
    }
}