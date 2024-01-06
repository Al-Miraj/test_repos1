public class Dashboard
{
    public Account CurrentUser { get; set; }

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
        {
            "Reservation Management",
            isAdmin ? "Reservation Overview" : "Order History",
            isAdmin ? "Customer Management" : "Reservation Overview",
            "Exit to main menu",
            "Log out"
        };

       

        int selectedOption = MenuSelector.RunMenuNavigator(dashboardOptions);

        switch (selectedOption)
        {
            case 0:
                ReservationManager();
                break;
            case 1:
                if (isAdmin)
                { ReservationOverview(); }
                else
                { Console.Clear(); OrderHistory(); }
                break;
            case 2:
                if (isAdmin)
                { CustomerManager(); }
                else
                { ReservationOverview(); }
                break;
            case 3:
                OptionMenu.RunMenu();
                break;
            case 4:
                LoginSystem.Logout();
                return;
            case 5:
                break;
        }
        Console.WriteLine("\n[Press any key to return to your dashboard.]");
        Console.ReadKey();
        RunDashboardMenu();
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
        List<Reservation> reservations = ((CustomerAccount)CurrentUser).GetReservations();  // todo check if works
        if (reservations.Count == 0)
        {
            Console.WriteLine("You have not reservated at this restaurant yet.");
            return;
        }
        foreach (Reservation reservation in reservations)
        {
            Console.WriteLine(reservation.ToString());
            Console.WriteLine();
        }
    }


    private void ReservationOverview()
    {
        Console.Clear();
        Console.WriteLine("Reservation Overview\n");

        // Prompt the user to enter a date
        Console.Write("Enter a date to view reservations (dd-mm-yyyy): ");
        DateOnly viewDate = ReservationSystem.GetReservationDate(); // Reuse existing method to get a valid date

        // Prompt the user to choose a timeslot
        Console.WriteLine("Choose a timeslot:");
        string timeslot = ReservationSystem.GetTimeslot(); // Reuse existing method to get a timeslot

        // Get the reserved table numbers for the selected date and timeslot
        List<int> reservatedTableNumbers = ReservationSystem.GetReservatedTablesAtDateAndTimeslot(viewDate, timeslot);

        // Example values for current table coordinate and number of people, adjust as needed
        (int, int) currentTableCoordinate = (1, 1);
        int numberOfPeople = 4; // This might be used differently depending on your implementation

        // Display the visual map of the restaurant with reserved/available tables
        ReservationSystem.PrintTablesMapClean(currentTableCoordinate, reservatedTableNumbers, numberOfPeople);

        Console.WriteLine("\n[Press any key to return to your dashboard.]");
        Console.ReadKey();
        RunDashboardMenu();
    }








}

