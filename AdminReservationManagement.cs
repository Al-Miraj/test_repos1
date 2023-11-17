using System.Runtime.CompilerServices;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualBasic.FileIO;

public static class AdminReservationManagement
{

    public static AdminAccount? CurrentAdmin { private get; set; } = null;

    public static void Display()
    {
        int selectedOption = 1;

        while (true)
        {
            Console.Clear();

            // Highlight the currently selected option
            for (int i = 1; i <= 6; i++)
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
                        Console.WriteLine(" View Reservations");
                        break;
                    case 2:
                        Console.WriteLine(" Add Reservation");
                        break;
                    case 3:
                        Console.WriteLine(" Update Reservation by number");
                        break;
                    case 4:
                        Console.WriteLine(" Cancel Reservation by number");
                        break;
                    case 5:
                        Console.WriteLine(" Search Reservations");
                        break;
                    case 6:
                        Console.WriteLine(" Back to Admin Dashboard");
                        break;
                }
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.UpArrow && selectedOption > 1)
            {
                selectedOption--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && selectedOption < 6)
            {
                selectedOption++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                if (selectedOption == 6)
                {
                    return;
                }
                HandleSelection(selectedOption);
            }
            else
            {
                continue;
            }
        }



        static void HandleSelection(int option)
        {
            Console.Clear();

            switch (option)
            {
                case 1:
                    ViewReservations();
                    break;
                case 2:
                    AddReservation();
                    break;
                case 3:
                    UpdateReservation();
                    break;
                case 4:
                    CancelReservation();
                    break;
                case 5:
                    SearchReservations();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }

            Console.WriteLine("\n[Press any key to return to the admin dashboard.]");
            Console.ReadKey();
        }
    }

    private static void AddReservation()
    {
        Console.WriteLine("Add Reservation manually");
        Console.WriteLine();
        ReservationSystem.Reservate(true);
    }

    private static void ViewReservations()
    {
        List<Reservation> reservations = CurrentAdmin!.GetReservations();
        Console.WriteLine("Reservation Overview");
        Console.WriteLine();
        if (reservations == null)
        {
            Console.WriteLine("There are no reservations yet");
            return;
        }
        foreach (Reservation reservation in reservations)
        {
            Console.WriteLine(reservation.ToString());
            Console.WriteLine();
        }
    }

    private static void UpdateReservation()
    {
        List<Reservation> reservations = CurrentAdmin.GetReservations();
        Console.WriteLine("Update Reservation");
        Console.WriteLine();
        Console.Write("Enter reservation number or press Enter to return: ");
        string input = Console.ReadLine()!;
        if (input == "")
        {
            return;
        }
        int reservationNumber = int.Parse(input);

        Reservation? reservation = reservations.FirstOrDefault(reservation => reservation.ReservationNumber == reservationNumber);
        if (reservation != null)
        {
            Console.WriteLine("Old data for reservation:");
            Console.WriteLine(reservation.ToString());
            Console.WriteLine("New data for reservation (just enter if you don't want to change a property):");
            int selectedOption = 1;
            while (true)
            {
                for (int i = 1; i <= 4; i++)
                {
                    Console.Write(i == selectedOption ? " >" : "  ");

                    // Display text labels for options
                    switch (i)
                    {
                        case 1:
                            Console.WriteLine(" Number of guests");
                            break;
                        case 2:
                            Console.WriteLine(" Date");
                            break;
                        case 3:
                            Console.WriteLine(" Timeslot");
                            break;
                        case 4:
                            Console.WriteLine(" Done");
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
                    if (selectedOption == 4)
                    {
                        ReservationSystem.UpdateJson();
                        Console.WriteLine("Reservation updated!");
                        return;
                    }
                    HandleSelection(reservation, selectedOption);
                }
            }
        }
        else
        {
            Console.WriteLine("Reservation not found. View the reservation list for reservation numbers.");
            return;
        }

    }

    private static void HandleSelection(Reservation reservation, int option)
    {
        Console.Clear();

        switch (option)
        {
            case 1:
                reservation.NumberOfPeople = ReservationSystem.GetNumberOfPeople(true);
                break;
            case 2:
                reservation.Date = ReservationSystem.GetReservationDate();
                break;
            case 3:
                reservation.TimeSlot = ReservationSystem.GetTimeslot();
                break;
            default:
                break;
        }
    }

    private static void CancelReservation()
    {
        List<Reservation> reservations = CurrentAdmin.GetReservations();
        Console.WriteLine("Cancel Reservation");
        Console.WriteLine();
        Console.Write("Enter reservation number or press Enter to return: ");
        string input = Console.ReadLine()!;
        if (input == "")
        {
            return;
        }
        int reservationNumber = int.Parse(input);
        Reservation? reservation = reservations.FirstOrDefault(reservation => reservation.ReservationNumber == reservationNumber);
        if (reservation != null)
        {
            ReservationSystem.Reservations.Remove(reservation);
            ReservationSystem.UpdateJson();
            Console.WriteLine("Reservation cancelled!");
        }
    }

    private static void SearchReservations()
    {
        var reservations = CurrentAdmin.GetReservations();
        while (true)
        {
            Console.WriteLine("Search Reservation\n");

            bool byDate = true;
            Console.WriteLine("Search by: ");
            Console.Write(byDate ? "   Reservation number\n > Date" : " > Reservation number\n   Date");

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.UpArrow && byDate)
            {
                byDate = false;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && !byDate)
            {
                byDate = true;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                if (byDate)
                {
                    Console.Write("Enter date (dd-MM-yyyy): ");
                    DateOnly date = ReservationSystem.GetReservationDate();
                    List<Reservation> reservationsOnDate = reservations.FindAll(reservation => reservation.Date == date);
                    if (reservationsOnDate != null)
                    {
                        foreach (Reservation r in reservationsOnDate)
                        {
                            Console.WriteLine(r.ToString());
                            Console.WriteLine();
                        }
                    }
                    return;
                }
                else
                {
                    Console.Write("Enter reservation number: ");
                    int reservationNumber = int.Parse(Console.ReadLine()!);
                    var reservation = reservations.FirstOrDefault(reservation => reservation.ReservationNumber == reservationNumber);
                    if (reservation != null)
                    {
                        Console.WriteLine(reservation.ToString());
                        Console.WriteLine();
                    }

                    return;
                }
            }
        }
    }
}