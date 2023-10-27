using System;

public class Menu
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Restaurant Menu:");
            Console.WriteLine("1. Reservation");
            Console.WriteLine("2. About Us");
            Console.WriteLine("3. Contact Us");
            Console.WriteLine("4. Menu");
            Console.WriteLine("5. Login");
            Console.WriteLine("6. Exit");
            Console.Write("Please select an option (1-6): ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Reservation - Please contact us to make a reservation.");
                    Console.ReadLine();
                    break;
                case "2":
                    Console.ReadLine();
                    break;
                case "3":
                    Console.WriteLine("Contact Us - Get in touch with us.");
                    Console.ReadLine();
                    break;
                case "4":
                    FoodMenu.Display();
                    break;
                case "5":
                    Console.WriteLine("Login - Enter your credentials to log in.");
                    Console.ReadLine();
                    break;
                case "6":
                    Console.WriteLine("Goodbye! Thank you for visiting.");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    Console.ReadLine();
                    break;
            }
        }
    }
}
