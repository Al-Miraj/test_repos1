using static System.Runtime.InteropServices.JavaScript.JSType;

public class Dashboard : Reservation
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
                { Console.Clear(); OrderHistory(true); }
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

    private void SendFeedback(Reservation reservation)
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
        //Feedback feedback = new Feedback(((CustomerAccount)CurrentUser).Email, rating, message, reservation);
        // todo: write to xml file
        Feedback feedback = new Feedback()
        {
            Email = ((CustomerAccount)CurrentUser).Email,
            Rating = rating,
            Message = message,
            ReservationNumber = reservation.ReservationNumber,
        };

        Program.context.Feedbacks.Add(feedback);
        Program.context.SaveChanges();
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

    private void OrderHistory(bool withFeedback = false)
    {
        Console.WriteLine("Your past reservations:\n");
        List<Reservation> reservations = ((CustomerAccount)CurrentUser).GetReservations();  // todo check if works: WORKS
        if (reservations.Count == 0)
        {
            Console.WriteLine("You have not reservated at this restaurant yet.");
            return;
        }

        if (withFeedback)
        {
            DisplayMenuOptions(reservations);
            Display(reservations);
        }

        return;
    }

    private void DisplayMenuOptions(List<Reservation> reservations)
    {
        for (int i = 1; i < reservations.Count; i++)
        {
            if (i == selectedOption)
            {
                Console.Write(">");
            }
            else
            {
                Console.Write(" ");
            }

            // Display each reservation as a menu option
            Console.WriteLine($"{i}: {GetReservationInfo(reservations[i - 1])}");
        }
    }

    private string GetReservationInfo(Reservation reservation)
    {
        return
            $"\nReservation Number: {reservation.ReservationNumber}" +
            $"\nDate: {reservation.Date}\nTimeslot: {reservation.TimeSlot}" +
            $"\nPrice: {reservation.GetTotalPrice()}".ToString();
    }

    public void Display(List<Reservation> reservations)
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
            else if (keyInfo.Key == ConsoleKey.DownArrow && selectedOption < reservations.Count)
            {
                selectedOption++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                bool exitMenu = HandleSelection(reservations);
                if (exitMenu) { break; }
            }
        }
    }

    private bool HandleSelection(List<Reservation> reservations)
    {
        Console.Clear();
        bool exitMenu = false;

        switch (selectedOption)
        {
            case int n when n >= 1 && n <= reservations.Count:
                // Handle selection voor specifieke reservatie
                Console.WriteLine($"Selected Reservation: {GetReservationInfo(reservations[selectedOption - 1])}");
                Console.WriteLine("Press Enter to write a review or '4' to exit.");
                var keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Enter)
                    SendFeedback(reservations[selectedOption - 1]);
            break;

            case 4:
                exitMenu = true;
                break;
        }
        return exitMenu;
    }
}
