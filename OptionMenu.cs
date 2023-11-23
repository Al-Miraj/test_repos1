
using System;

public class Menu
{
    private static int selectedOption = 1;

    public void RunMenu()
    {
        Console.CursorVisible = false;
        while (true)
        {
            Console.Clear();

            // Highlight the currently selected option
            for (int i = 1; i <= 6; i++)
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
                        Console.WriteLine(" Food");
                        break;
                    case 5:
                        Console.WriteLine(" Drinks");
                        break;
                    case 6:
                        Console.WriteLine(" Login");
                        break;
                    case 7:
                        Console.WriteLine(" Exit");
                        break;
                }
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.UpArrow && selectedOption > 1)
            {
                selectedOption--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && selectedOption < 6)
            {
                selectedOption++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                HandleSelection(selectedOption);
            }
        }
    }

    static void HandleSelection(int option)
    {
        Console.Clear();

        switch (option)
        {
            case 1:
                Console.WriteLine("Reservation - Please contact us to make a reservation.");
                //ReservationSystem sstm = new ReservationSystem();
                //sstm.SystemRun();
                Console.ReadLine();
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
                Console.WriteLine("Drinks lalalalala");
                DrinksMenu.Display();
                break;
            case 6:
                Console.WriteLine("Login - Enter your credentials to log in.");
                break;
            case 7:
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