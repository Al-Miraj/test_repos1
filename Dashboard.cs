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
        bool isSuperAdmin = CurrentUser is SuperAdminAccount;

        Console.Clear();
        Console.WriteLine($"Welcome {CurrentUser.Name}!");
        Console.WriteLine("This is your dashboard.");

        List<string> dashboardOptions = new List<string>()
        { isSuperAdmin ? "Reservation Management" : (isAdmin ? "Reservation Management" : "Order History"), // todo: ipv order history misschien view profile details ofzo?
          isSuperAdmin ? "Customer Management" : (isAdmin ? "Customer Management" : "Reservation Manager"),
          isSuperAdmin ? "Add admin" : (isAdmin ? null : "Daily Menu"),
          isSuperAdmin ? "Remove admin" : null,
          isSuperAdmin ? "Admin Overview" : null,
          "Exit to main menu",
          "Log out"
        };

        // Removes the null entries (without this, an empty option appears on the dashboard)
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
                if (isSuperAdmin)
                { AddAdmin(); } // Add this case for regular users
                else
                { DailyMenu(); }
                break;
            case 3:
                if (isSuperAdmin)
                { RemoveAdmin(); }
                else
                { OptionMenu.RunMenu(); }
                break;
            case 4:
                if (isSuperAdmin)
                { AdminOverview(); }
                else
                { OptionMenu.RunMenu(); }
                break;
            case 5:
                OptionMenu.RunMenu(); // loop warning
                break;
            case 6:
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
        Console.WriteLine("Daily Menu");
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
            Console.WriteLine("You have not reserved at this restaurant yet.");
            return;
        }
        foreach (Reservation reservation in reservations)
        {
            Console.WriteLine(reservation.ToString());
            Console.WriteLine();
        }
    }
}