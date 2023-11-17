public class Dashboard
{
    public Account CurrentUser { get; set; }

    public Dashboard(Account account)
    {
        CurrentUser = account;
    }

    int selectedOption = 1;


    public void Display()
    {
        bool isAdmin = CurrentUser is AdminAccount;
        while (true)
        {
            Console.Clear();
            Console.WriteLine(isAdmin); // todo delete later
            Console.WriteLine($"Welcome {CurrentUser.Name}!");
            Console.WriteLine("This is your dashboard.");

            // Highlight the currently selected option
            for (int i = 1; i <= 4; i++)
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
                        Console.WriteLine(!isAdmin ? " Order History" : " Reservation Management");
                        break;
                    case 2:
                        Console.WriteLine(!isAdmin ? " Reservation Manager" : " Customer Management");
                        break;
                    case 3:
                        Console.WriteLine(" Exit to main menu");
                        break;
                    case 4:
                        Console.WriteLine(" Log out");
                        break;
                }
            }

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
                HandleSelection(selectedOption, isAdmin);
            }
            else
            {
                continue;
            }
        }
    }

    void HandleSelection(int option, bool isAdmin)
    {
        Console.Clear();

        switch (option)
        {
            case 1:
                if (!isAdmin)
                {
                    OrderHistory();
                    Console.ReadLine();
                }
                else
                    ReservationManager();
                break;
            case 2:
                if (!isAdmin)
                    CustomerReservationManager();
                else
                    CustomerManager();
                break;
            case 3:
                Menu.RunMenu();
                break;
            case 4:
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