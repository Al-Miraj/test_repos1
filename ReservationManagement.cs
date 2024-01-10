using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

public static class ReservationManagement
{
    public static Account CurrentUser { get; set; } = Restaurant.AdminAccounts[0];
    private static Restaurant.UserRole userRole = Restaurant.GetUserRole(CurrentUser);
    private static bool IsAdmin = userRole == Restaurant.UserRole.Admin || userRole == Restaurant.UserRole.SuperAdmin;
    private static List<Reservation> ReservationsOfUser =>
        GetReservationsOfUser();


    public static void Display()
    {
        
            List<string> options = new List<string>()
        {
            "View all Reservations",
            "View future Reservations",
            "Add Reservation",
            "View Reservations on date",
            "View Table Availablity",
            $"Back to {userRole} Dashboard"
        };
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Reservation Manager:");
            int selectedOption = MenuSelector.RunMenuNavigator(options);
            switch (selectedOption)
            {
                case 0:
                    Console.Clear();
                    ViewReservations(false);
                    return;
                case 1:
                    Console.Clear();
                    ViewReservations();
                    return;
                case 2:
                    Console.Clear();
                    AddReservation();
                    return;
                case 3:
                    Console.Clear();
                    SearchReservations();
                    return;
                case 4:
                    Console.Clear();
                    ViewTableAvailability();
                    return;
                case 5:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }

            Restaurant.UpdateRestaurantFiles();
            Console.WriteLine("\n[Press any key to return to the Reservation Manager menu.]");
            Console.ReadKey();
        }
    }


    private static void ViewTableAvailability()
    {
        Console.Clear();
        Console.WriteLine("Reservation Overview\n");
        Console.Write("Enter the date that you want to see the table availability of (dd-mm-yyyy): ");
        DateOnly overviewDate = ReservationSystem.GetReservationDate();
        Console.WriteLine("Choose timeslot that you want to see the table availability of: ");
        string overviewTimeslot = ReservationSystem.GetTimeslot();
        List<int> reservedTableNumbers = ReservationSystem.GetAvailability(overviewDate, overviewTimeslot);
        Console.Clear();
        ReservationSystem.PrintBarDisplay();
        ReservationSystem.PrintTablesMapClean((1, 1), reservedTableNumbers, 0);
    }

    private static List<Reservation> GetReservationsOfUser()
    {
        List<Reservation> reservations;
        if (IsAdmin)
        {
            reservations = Restaurant.Reservations;
        }
        else
        {
            reservations = ((CustomerAccount)CurrentUser).Reservations;
        }
        return reservations;
    }

    private static void ViewReservations(bool future = true)
    {
        while (true)
        {
            Console.Clear();
            Reservation? reservation;
            if (future)
            {
                List<Reservation> upcomingReservations = ReservationsOfUser.FindAll(reservation => reservation.Date >= DateOnly.FromDateTime(DateTime.Now));
                reservation = GetReservation(upcomingReservations);
            }
            else
            {
                reservation = GetReservation(ReservationsOfUser);
            }
            if (reservation == null)
                Display();
            HandleAction(reservation);
            Restaurant.UpdateRestaurantFiles();
            Display();
        }
    }

    private static void HandleAction(Reservation reservation)
    {
            Console.WriteLine("\nAction:\n");
            List<string> options = new()
        {
            "Update",
            "Cancel",
            "Back"
        };
            while (true)
            {
                int selectedOption = MenuSelector.RunMenuNavigator(options);
                switch (selectedOption)
                {
                    case 0:
                        Console.Clear();
                        if (reservation.Date >= DateOnly.FromDateTime(DateTime.Now))
                            UpdateReservation(reservation);
                        else
                        {
                            Console.WriteLine("Can't update past reservations.");
                        }
                        return;
                    case 1:
                        Console.Clear();
                        CancelReservation(reservation);
                        return;
                    case 2:
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please select a valid option.");
                        return;
                }
            }
    }


    private static void AddReservation() =>
        ReservationSystem.Reservate(IsAdmin);

    private static void UpdateReservation(Reservation reservation)
    {
        List<string> updateOptions = new List<string>()
            {
                "Amount of people",
                "Date",
                "Timeslot",
                "Done"
            };
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Current data for reservation {reservation.ReservationNumber}:");
            Console.WriteLine(reservation.ToString());
            Console.WriteLine("\nUpdate:");
            int selectedOption = MenuSelector.RunMenuNavigator(updateOptions);
            switch (selectedOption)
            {
                case 0:
                    Console.Clear();
                    UpdateAmountOfPeople(reservation);
                    break;
                case 1:
                    Console.Clear();
                    Console.WriteLine("Enter the new reservation date (dd-MM-yyyy):");
                    reservation.Date = ReservationSystem.GetReservationDate();
                    break;
                case 2:
                    Console.Clear();
                    reservation.TimeSlot = ReservationSystem.GetTimeslot();
                    break;
                case 3:
                    Console.WriteLine("Reservation updated!");
                    return;
                default:
                    return;
            }
        }
    }


    public static void UpdateAmountOfPeople(Reservation reservation)
    {
        while (true)
        {
            Console.WriteLine("Enter the new amount of people that are coming:");
            int numberOfPeople = ReservationSystem.GetNumberOfPeople(IsAdmin);
            if (numberOfPeople > reservation.SelectedTable.Capacity)
            {
                Console.WriteLine("The table you reservated does not have enough seats.\n");
                continue;
            }
            break;
        }
    }

    private static Reservation? GetReservation(List<Reservation> reservations)
    {
        Console.WriteLine("Reservation Overview:\n");
        List<string> options = reservations.Select(r => r.GetReservationInfo()).ToList();
        options.Add("Back");
        int selectedOption = MenuSelector.RunMenuNavigator(options);
        if (selectedOption == options.Count - 1) { return null; }
        options.Remove(options.Last());
        return reservations[selectedOption];
    }

    private static void CancelReservation(Reservation reservation)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        if (reservation.Date < today)
        {
            Console.WriteLine("It is not possible to cancel a reservation that has already passed.");
            return;
        }
        Console.WriteLine(reservation.ToString() + "\n");
        Console.WriteLine("Are you sure you want to cancel this reservation?");
        int selectedOption = MenuSelector.RunMenuNavigator(new List<string>() { "Yes", "No" });
        if (selectedOption == 0)
        {
            bool removed = Restaurant.Reservations.Remove(reservation);
            if (!removed)
            { throw new Exception($"Reservation \"{reservation.ReservationNumber}\" wasn't removed succesfully"); }
            else
            {
                Console.WriteLine("Reservation cancelled!");
            }
        }
        else
        { Console.WriteLine("This reservation will not be cancelled."); }
    }

    private static void SearchReservationByNumber()
    {
        int reservationNumber = GetReservationNumber();
        Reservation reservation = ReservationsOfUser.FirstOrDefault(reservation => reservation.ReservationNumber == reservationNumber);
        if (reservation != null)
        {
            Console.WriteLine(reservation.ToString());
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine($"No reservation with the reservation number \"{reservationNumber}\" was found.");
        }
    }

    private static void SearchReservationsByDate() // maybe: add option to enter on reservations and if you want to do something with the reservation
    {
        DateOnly date;
        do
        {
            Console.Write("Enter date (dd-MM-yyyy): ");
            string dateStr = Console.ReadLine().Trim();
            Console.Clear();
            bool correctDateFormat = DateOnly.TryParseExact(dateStr, "dd-MM-yyyy", out date);
            if (correctDateFormat)
            {
                List<Reservation> reservationsOnDate = ReservationsOfUser.FindAll(reservation => reservation.Date == date);
                if (reservationsOnDate.Count == 0)
                {
                    Console.WriteLine("There are no reservations on this date.");
                    Console.WriteLine("\n[Press any key to continue]");
                    Console.ReadKey();
                    break;
                }
                Reservation? reservation = GetReservation(reservationsOnDate);
                if (reservation == null) // User pressed "Back" option.
                {
                    Console.WriteLine("There are no reservations on this date.");
                    Console.WriteLine("\n[Press any key to continue]");
                    Console.ReadKey();
                    break;
                }
                HandleAction(reservation);
                break;
            }
            else
            {
                Console.WriteLine("Date must be entered in dd-MM-yyyy format.");
            }
        } while (true);
        Display();
    }

    private static void SearchReservations()
    {
        Console.WriteLine("Search Reservation\n");
        if (ReservationsIsEmpty(ReservationsOfUser)) { return; }
        Console.WriteLine("Search by:");
        int selectedOption = MenuSelector.RunMenuNavigator(new List<string>() { "Reservation number", "Date" });
        if (selectedOption == 0)
        {
            SearchReservationByNumber();
        }
        else
        {
            SearchReservationsByDate();
        }
    }

    private static bool ReservationsIsEmpty(List<Reservation> reservations)
    {
        if (reservations.Count == 0)
        {
            Console.WriteLine("\nThere are no reservations yet.");
            return true;
        }
        return false;
    }

    private static int GetReservationNumber()
    {
        int reservationNumber;
        Console.WriteLine("Enter reservation number:");
        while (true)
        {
            string input = Console.ReadLine();
            if (!input.All(char.IsDigit))
            { Console.WriteLine("Reservation numbers must only contain digits."); continue; }
            else if (input.Length != 5)
            { Console.WriteLine("Reservations numbers must have 5 digits."); continue; }
            reservationNumber = Convert.ToInt32(input);
            break;
        }
        return reservationNumber;
    }
}