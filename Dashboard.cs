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

        Console.WriteLine($"Welcome {CurrentUser.Name}!");
        Console.WriteLine("This is your dashboard.");

        List<string> dashboardOptions = new List<string>()
        { isAdmin ? "Reservation Management" : "Order History",
          isAdmin ? "Customer Management" : "Reservation Manager",
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
                { OrderHistory(); Console.ReadLine(); }
                break;
            case 1:
                if (isAdmin)
                { CustomerReservationManager(); }
                else
                { CustomerManager(); }
                break;
            case 2:
                Menu.RunMenu();
                break;
            case 3:
                LoginSystem.Logout();
                break;
            default:
                return;
        }
    }


    private void ReservationManager()
    {
        AdminReservationManagement.CurrentAdmin = (CurrentUser as AdminAccount)!;
        AdminReservationManagement.Display();
    }

    private void CustomerManager()
    {
        CustomerManagement.CurrentAdmin = (CurrentUser as AdminAccount)!;
        CustomerManagement.Display();
    }

    private void OrderHistory()
    {
        Console.WriteLine("Your past reservations:");
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

    private void CustomerReservationManager()
    {
        CustomerReservationManagement.CurrentUser = CurrentUser;
        CustomerReservationManagement.Display();
    }
}