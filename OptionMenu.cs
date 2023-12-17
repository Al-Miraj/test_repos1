
public static class OptionMenu // Made class static so LoginSystem and Dashboard files don't rely on instances
{
    public static bool IsUserLoggedIn = false;
    public static Account? CurrentUser;
    public static Dashboard? UserDashboard;

    public static void RunMenu()
    {

        List<string> menuOptions = new List<string>()
        {
            "Reservation",
            "About Us",
            "Contact Us",
            "Menu",
            "Drinks",
            "Deals",
            "Login/Register",
            "Exit"
        };
        while (true)
        {
            Console.Clear();


            // Inside the while loop so that the name gets displayed each time the options menu is shown
            Console.WriteLine("    __            ____       ___              ______                            _     ");
            Console.WriteLine("   / /   ___     / __ \\___  / (_)_______     / ____/________ _____  _________ _(_)____");
            Console.WriteLine("  / /   / _ \\   / / / / _ \\/ / / ___/ _ \\   / /_  / ___/ __ `/ __ \\/ ___/ __ `/ / ___/");
            Console.WriteLine(" / /___/  __/  / /_/ /  __/ / / /__/  __/  / __/ / /  / /_/ / / / / /__/ /_/ / (__  ) ");
            Console.WriteLine("/_____/\\___/  /_____/\\___/_/_/\\___/\\___/  /_/   /_/   \\__,_/_/ /_/\\___/\\__,_/_/____/ \n ");


            int selectedOption = MenuSelector.RunMenuNavigator(menuOptions);
            Console.Clear();
            switch (selectedOption)
            {
                case 0:
                    ReservationSystem.RunSystem();
                    break;
                case 1:
                    About.RestaurantInformation();
                    break;
                case 2:
                    Contact.ContactInformation();
                    AboutUs.travel();
                    break;
                case 3:
                    MenuItem.Display();
                    break;
                case 4:
                    DrinksMenu.Display();
                    break;
                case 5:
                    Restaurant.DisplayDeals();
                    break;
                case 6:
                    if (IsUserLoggedIn)
                        UserDashboard!.RunDashboardMenu();
                    else
                        LoginSystem.Start();
                    break;
                case 7:
                    Restaurant.UpdateRestaurantFiles();
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
}