using static System.Runtime.InteropServices.JavaScript.JSType;

public class Dashboard : Reservation
{
    public Account CurrentUser { get; set; }
    private static int selectedOption;
    Database database = new Database();

    public Dashboard(Account account)
    {
        CurrentUser = account;
    }

    public void RunDashboardMenu()
    {
        bool isAdmin = CurrentUser is AdminAccount;
        bool isSuperAdmin = CurrentUser is SuperAdminAccount;
        Console.Clear();
        Console.WriteLine($"Welcome {CurrentUser.Name}!");
        Console.WriteLine("This is your dashboard.");

        List<string> dashboardOptions = new List<string>()
        { isSuperAdmin ? "Reservation Management" : (isAdmin ? "Reservation Management" : "Order History"), // todo: ipv order history misschien view profile details ofzo?   : ja, in reservation management heb je al order history (view reservations)
          isSuperAdmin ? "Customer Management" : (isAdmin ? "Customer Management" : "Reservation Manager"),
          isSuperAdmin ? "Read Feedback" : (isAdmin ? "Read Feedback" : "Send Feedback"),
          isSuperAdmin ? "Add admin" : (isAdmin ? null : "Daily Menu"),
          isSuperAdmin ? "Remove admin" : null,
          isSuperAdmin ? "Admin Overview" : null,
          "Exit to main menu",
          "Log out"
        };

        dashboardOptions.RemoveAll(option => option == null);

        int selectedOption = MenuSelector.RunMenuNavigator(dashboardOptions);
        switch (selectedOption)
        {
            case 0:
                if (isAdmin || isSuperAdmin)
                { ReservationManager(); }
                else
                { Console.Clear(); OrderHistory(); }
                break;
            case 1:
                if (isAdmin || isSuperAdmin)
                { CustomerManager(); }
                else
                { ReservationManager(); }
                break;
            case 2:
                if (isAdmin || isSuperAdmin)
                { Console.Clear(); ReadFeedback(); }
                else
                { Console.Clear(); GetOrders(); }
                break;
            case 3:
                if (isSuperAdmin)
                { AddAdmin(); } // Add this case for regular users
                else
                { DailyMenu(); }
                break;
            case 4:
                if (isSuperAdmin)
                { RemoveAdmin(); }
                else
                { OptionMenu.RunMenu(); }
                break;
            case 5:
                if (isSuperAdmin)
                { AdminOverview(); }
                else
                { OptionMenu.RunMenu(); }
                break;
            case 6:
                OptionMenu.RunMenu(); // loop warning
                break;
            case 7:
                LoginSystem.Logout();
                return;
            default:
                break;
        }

        Console.WriteLine("\n[Press any key to return to the your dashboard.]");
        Console.ReadKey();
        RunDashboardMenu();
    }

    //private void DailyMenu()
    //{
    //    DailyMenuGenerator.DisplayDailyMenu();
    //}

    private void AddAdmin()
    {
        SuperAdminAccount.SuperAdminCanAddAdmin();
    }

    private void RemoveAdmin()
    {
        SuperAdminAccount.SuperAdminCanRemoveAdmin();
    }

    private void AdminOverview()
    {
        SuperAdminAccount.AdminAccountsOverview();
    }


    private void DailyMenu()
    {
        DailyMenuGenerator.DisplayDailyMenu();
    }

    private void GetOrders()
    {
        List<Reservation> reservations = ((CustomerAccount)CurrentUser).GetReservations();
        List<string> options = reservations.Select(r => GetReservationInfo(r)).ToList();
        options.Add("Exit");
        if (reservations != null) { selectedOption = MenuSelector.RunMenuNavigator(options); }
        else { return; }
        Console.Clear();
        options.Remove(options.Last());
        Reservation reservation = reservations[selectedOption];
        SendFeedback(reservation);
    }
    private void SendFeedback(Reservation reservation)
    {
        Console.WriteLine("How would you rate our service?");
        int rating = MenuSelector.RunMenuNavigator(new List<int>() { 1, 2, 3, 4, 5 });
        rating++;
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
            ReservationNumber = ((Reservation)reservation).ReservationNumber,
        };

        database.DataWriter(feedback);
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
            Console.WriteLine("You have not booked at this restaurant yet.");
            return;
        }
    }

    private string GetReservationInfo(Reservation reservation)
    {
        return
        $" Reservation Number: {reservation.ReservationNumber}" +
        $" \n    Date: {reservation.Date}" +
        $" \n    Timeslot: {reservation.TimeSlot}" +
        $" \n    Price: {reservation.GetTotalPrice()}" +
        $"\n";

    }

    private void ReadFeedback()
    {
        Database db = new Database();
        Console.WriteLine("Customer Feedback: \n");
        List<Feedback> feedbacks = db.DataReader();
        foreach (Feedback feedback in feedbacks)
        {
            DisplayFeedback(feedback);
        }
        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
    }

    private void DisplayFeedback(Feedback feedback)
    {
        Console.WriteLine(feedback.Email + "                  " + feedback.Rating + " out of 5");
        Console.WriteLine(feedback.Message);
        Console.WriteLine();
    }
}