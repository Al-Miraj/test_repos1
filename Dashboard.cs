public class Dashboard
{
    public Account CurrentUser { get; set; }
    private static int selectedOption = 1;

    public Dashboard(Account account)
    {
        CurrentUser = account;
    }

    public void RunDashboardMenu()
    {
        bool isAdmin = CurrentUser is AdminAccount;
        Console.Clear();
        Console.WriteLine($"Welcome {CurrentUser.Name}!");
        Console.WriteLine("This is your dashboard.");

        List<string> dashboardOptions = new List<string>()
        { isAdmin ? "Reservation Management" : "Order History", // todo: ipv order history misschien view profile details ofzo?   : ja, in reservation management heb je al order history (view reservations)
          isAdmin ? "Customer Management" : "Reservation Manager",
          isAdmin ? "Read Feedback" : "Send Feedback",
          "Exit to main menu",
          "Log out"
        };
        int selectedOption = MenuSelector.RunMenuNavigator(dashboardOptions);
        switch (selectedOption)
        {
            case 0:
                if (isAdmin)
                { ReservationManager(); }
                else
                { Console.Clear(); OrderHistory(); }
                break;
            case 1:
                if (isAdmin)
                { CustomerManager(); }
                else
                { ReservationManager(); }
                break;
            case 2:
                if (isAdmin)
                { Console.Clear(); /*ReadFeedback();*/ }
                else
                { Console.Clear(); SendFeedback(); }
                break;
            case 3:
                OptionMenu.RunMenu(); // loop warning
                break;
            case 4:
                LoginSystem.Logout();
                return;
            default:
                break;
        }

        Console.WriteLine("\n[Press any key to return to the your dashboard.]");
        Console.ReadKey();
        RunDashboardMenu();
    }

    private void SendFeedback()
    {
        Console.WriteLine("How would you rate our service?");
        int rating = MenuSelector.RunMenuNavigator(new List<int>() { 1, 2, 3, 4, 5 });
        Console.WriteLine($"You rated our service {rating} out of 5.");
        if (rating < 3)
        {
            Console.WriteLine("We are sorry to hear that.");
        }
        Console.WriteLine("What could we have done better to improve your experience? Or what went good?");
        string message = Console.ReadLine()!;
        Console.WriteLine("Thank you for the feedback!");
        Feedback feedback = new Feedback(((CustomerAccount)CurrentUser).Email, rating, message);
        // todo: write to xml file
    }

    private void ReadFeedback()
    {
        List<Feedback> feedback = XmlFileHandler.ReadFromFile<Feedback>("Feedback.xml");
    }


    private void ReservationManager()
    {
        ReservationManagement.CurrentUser = CurrentUser;
        ReservationManagement.Display();
    }

    private void CustomerManager()
    {
        CustomerManagement.CurrentAdmin = (CurrentUser as AdminAccount)!;
        CustomerManagement.Display();
    }

    private void OrderHistory()
    {
        Console.WriteLine("Your past reservations:\n");
        List<Reservation> reservations = ((CustomerAccount)CurrentUser).GetReservations();  // todo check if works: WORKS
        if (reservations.Count == 0)
        {
            Console.WriteLine("You have not reservated at this restaurant yet.");
            return;
        }

        DisplayMenuOptions(reservations);
        Display();
        
    }

    private void DisplayMenuOptions(List<Reservation> reservations)
    {
        for (int i = 1; i <= reservations.Count; i++)
        {
            if (i == selectedOption)
            {
                Console.Write(">");
            }
            else
            {
                Console.Write(" ");
            }

            switch (i)
            {
                case 1:
                    Console.WriteLine(" Lunch");
                    break;
                case 2:
                    Console.WriteLine(" Dinner");
                    break;
                case 3:
                    Console.WriteLine(" Sort menu by category");
                    break;
                case 4:
                    Console.WriteLine(" Exit");
                    break;
            }
        }
    }
    public void Display()
    {
        Console.CursorVisible = false;

        while (true)
        {
            Console.Clear();

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.UpArrow && selectedOption > 1)
            {
                selectedOption--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && selectedOption < 4)
            {
                selectedOption++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                bool exitMenu = HandleSelection();
                if (exitMenu) { break; }
            }
        }
    }

    private bool HandleSelection()
    {
        Console.Clear();
        bool exitMenu = false;

        switch (selectedOption)
        {
            case 1:

                break;
            case 2:

                break;
            case 3:
                if (timeSlot == "") { break; }
                //PrintInfo(SortFoodMenu.menuItems, SortFoodMenu.SelectedTimeSlotOption == 2 ? "Dinner" : "Lunch");
                break;
            case 4:
                exitMenu = true;
                break;
        }
        return exitMenu;
    }
}
