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
            Console.WriteLine();
            if (CurrentUser == null)
            {
                Console.WriteLine("This is where the bug is "); // Big bug : current user gets passed to Dashboard as null
                break;
            }
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
                if (!isAdmin)
                {
                    switch (i)
                    {
                        case 1:
                            Console.WriteLine(" Order History");
                            break;
                        case 2:
                            Console.WriteLine(" Reservation Manager");
                            break;
                        case 3:
                            Console.WriteLine(" Log out");
                            break;
                        case 4:
                            Console.WriteLine(" Exit to main menu");
                            break;
                    }
                }
                else
                {
                    switch (i)
                    {
                        case 1:
                            Console.WriteLine(" Reservation Management");
                            break;
                        case 2:
                            Console.WriteLine(" Customer Management");
                            break;
                        case 3:
                            Console.WriteLine(" Log out");
                            break;
                        case 4:
                            Console.WriteLine(" Exit to main menu");
                            break;
                    }
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
        }
    }

    void HandleSelection(int option, bool isAdmin)
    {
        Console.Clear();
        switch (option)
        {
            case 1:
                if (!isAdmin)
                    OrderHistory();
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
                LoginSystem.Logout();
                Menu.RunMenu();
                break;
            case 4:
                Menu.RunMenu();
                break;
            default:
                return;
        }
    }


    private void ReservationManager()
    {
        ReservationManagement.CurrentAdmin = (CurrentUser as AdminAccount)!;
        ReservationManagement.Display();
    }

    private void CustomerManager()
    {
        CustomerManagement.CurrentAdmin = (CurrentUser as AdminAccount)!;
        CustomerManagement.Display();
    }

    private void OrderHistory()
    {
        Console.WriteLine("Your past reservations:");
        foreach (var reservation in CurrentUser.GetReservations())
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