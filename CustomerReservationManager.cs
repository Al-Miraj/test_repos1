public static class CustomerReservationManagement
{
    public static Account CurrentUser { get; set; }

    public static void Display()
    {
        Console.CursorVisible = false;
        int selectedOption = 1;
        while (true)
        {
            Console.Clear();

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
                        Console.WriteLine(" Make a Reservation");
                        break;
                    case 2:
                        Console.WriteLine(" Update Reservation");
                        break;
                    case 3:
                        Console.WriteLine(" Cancel Reservation");
                        break;
                    case 4:
                        Console.WriteLine(" Back to Customer Dashboard");
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
                HandleSelection(selectedOption);
            }
        }


        static void HandleSelection(int option)
        {
            Console.Clear();

            switch (option)
            {
                case 1:
                    Console.WriteLine("Make a Reservation");
                    MakeReservation();
                    break;
                case 2:
                    Console.WriteLine("Update Reservation");
                    UpdateReservation();
                    break;
                case 3:
                    Console.WriteLine("Cancel Reservation");
                    CancelReservation();
                    break;
                case 4:
                    Console.WriteLine("Back to Customer Dashboard");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    Console.ReadLine();
                    break;
            }

            Console.WriteLine("\n\n[Press any key to return to the main menu.]");
            Console.ReadKey();
        }


    }

    private static void CancelReservation()
    {
        Console.WriteLine("Which reservation would you like to cancel?");
        var reservations = CurrentUser.GetReservations();
        foreach (var item in reservations)
        {
            Console.WriteLine(item.ToString());
            Console.WriteLine();
        }
        do
        {
            Console.Write("Enter Reservation Number: ");
            int id = int.Parse(Console.ReadLine()!);
            var reservation_ids = reservations.Select(reservation => reservation.ReservationNumber);
            if (reservation_ids.Contains(id))
            {
                var reservation = reservations.FirstOrDefault(reservation => reservation.ReservationNumber == id);
                ReservationSystem.reservations.Remove(reservation!);
                Console.WriteLine("Reservation cancelled.");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Reservation not found.");
        } while (true);
    }

    private static void UpdateReservation()
    {
        Console.WriteLine("Which reservation would you like to update?");
        var reservations = CurrentUser.GetReservations();
        foreach (var item in reservations)
        {
            Console.WriteLine(item.ToString());
            Console.WriteLine();
        }
        Console.Write("Enter Reservation Number: ");
        int id = int.Parse(Console.ReadLine()!);
        var reservation = reservations.FirstOrDefault(reservation => reservation.ReservationNumber == id);
        if (reservation != null)
        {
            Console.WriteLine("Old data for reservation:");
            Console.WriteLine(reservation.ToString());
            Console.WriteLine("New data for reservation (just enter if you don't want to change a part of the info):");
            do
            {
                Console.Write("Amount of people (0-6): ");
                string? input = Console.ReadLine();
                if (input != null)
                {
                    int amount = int.Parse(input);
                    if (amount > 0 && amount <= 6)
                    {
                        reservation.NumberOfPeople = amount;
                        break;
                    }
                    else
                        Console.WriteLine("Amount must be between 0 and 6");
                }
                else
                    break;
            } while (true);



            Console.Write("Date (dd-MM-yyyy): ");
            string dateStr = Console.ReadLine()!.Trim();
            DateOnly date = DateOnly.ParseExact(dateStr, "dd-MM-yyyy", null);
            reservation.Date = date;
            Console.Write("Timeslot (Lunch, dinner): "); // Add all timeslots
            string timeslot = Console.ReadLine()!;
            reservation.TimeSlot = timeslot;
            LoginSystem.UpdateJson();
            Console.WriteLine("Reservation updated!");
            Console.ReadLine();
        }
    }

    private static void MakeReservation() => ReservationSystem.RunSystem();
}