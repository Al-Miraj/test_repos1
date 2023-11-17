/*public class Menu //change name?
{
   public void RunMenu()
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
                   ReservationSystem sstm = new ReservationSystem();
                   sstm.SystemRun();
                   Console.ReadLine();
                   break;
               case "2":
                   About.RestaurantInformation();
                   Console.ReadLine();
                   break;
               case "3":
                   Console.WriteLine("Contact Us - Get in touch with us.");
                   Contact.ContactInformation();
                   Console.ReadLine();
                   AboutUs.travel();
                   Console.ReadLine();
                   break;
               case "4":
                   Console.WriteLine("Menu - Check out our delicious dishes.");
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
}*/


using System;

public static class Menu // Made class static so LoginSystem and Dashboard files don't rely on instances
{
    public static bool IsUserLoggedIn = false;
    public static Account? CurrentUser;
    public static Dashboard? UserDashboard;
    private static int selectedOption = 1;

    public static void RunMenu()
    {
        Console.CursorVisible = false;
        while (true)
        {
            Console.Clear();

            // Highlight the currently selected option
            for (int i = 1; i <= 8; i++)
            {
                if (i == selectedOption)
                {
                    Console.Write(">");
                }
                else
                {
                    Console.Write(" ");
                }

                // Display text labels for options
                switch (i)
                {
                    case 1:
                        Console.WriteLine(" Reservation");
                        break;
                    case 2:
                        Console.WriteLine(" About Us");
                        break;
                    case 3:
                        Console.WriteLine(" Contact Us");
                        break;
                    case 4:
                        Console.WriteLine(" Menu");
                        break;
                    case 5:
                        Console.WriteLine(" Drinks");
                        break;
                    case 6:
                        Console.WriteLine(" Deals");
                        break;
                    case 7:
                        Console.WriteLine(" Login/Register");
                        break;
                    case 8:
                        Console.WriteLine(" Exit");
                        break;
                }
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.UpArrow && selectedOption > 1)
            {
                selectedOption--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && selectedOption < 8)
            {
                selectedOption++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                HandleSelection(selectedOption);
            }
        }
    }

    public static void HandleSelection(int option)
    {
        Console.Clear();

        switch (option)
        {
            case 1:
                ReservationSystem.RunSystem();
                break;
            case 2:
                About.RestaurantInformation();
                break;
            case 3:
                Console.WriteLine("Contact Us - Get in touch with us.");
                Contact.ContactInformation();
                AboutUs.travel();
                break;
            case 4:
                Console.WriteLine("Menu - Check out our delicious dishes.");
                FoodMenu.Display();
                break;
            case 5:
                DrinksMenu.Display();
                break;
            case 6:
                Restaurant.DisplayDeals();
                break;
            case 7:
                if (IsUserLoggedIn)
                    UserDashboard!.RunDashboardMenu();
                else
                    LoginSystem.Start();
                break;
            case 8:
                Console.WriteLine("Goodbye! Thank you for visiting.");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid choice. Please select a valid option.");
                Console.ReadLine();
                break;
        }

        Console.WriteLine("\n\n[Press any key to return to the main menu.]");
        Console.ReadKey();
    }
}