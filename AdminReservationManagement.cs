using System.Runtime.CompilerServices;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualBasic.FileIO;

public static class AdminReservationManagement
{

    public static AdminAccount? CurrentAdmin { private get; set; } = null;

    public static void Display()
    {
        Console.Clear();
        Console.WriteLine("Reservation Manager:");

        List<string> RMOptions = new List<string>() // RM == ReservationManager
        {
            "View Reservations",
            "Add Reservation",
            "Update Reservation by number",
            "Cancel Reservation by number",
            "Search Reservations",
            "Back to Admin Dashboard"
        };
        int selectedOption = MenuSelector.RunMenuNavigator(RMOptions);
        switch (selectedOption)
        {
            case 0:
                ViewReservations();
                break;
            case 1:
                AddReservation();
                break;
            case 2:
                UpdateReservation();
                break;
            case 3:
                CancelReservation();
                break;
            case 4:
                SearchReservations();
                break;
            case 5:
                return;
            default:
                Console.WriteLine("Invalid choice. Please select a valid option.");
                break;
        }

        Console.WriteLine("\n[Press any key to return to the Reservation Manager menu.]");
        Console.ReadKey();
        Display();
    }

    private static void AddReservation()
    {
        Console.WriteLine("Add Reservation manualy");
        Console.WriteLine();
        ReservationSystem.Reservate(true);
    }

    private static void ViewReservations()
    {
        List<Reservation> reservations = CurrentAdmin!.GetReservations();
        Console.Clear();
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
        Console.Clear();
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
            List<string> updateOptions = new List<string>()
            {
                "Amount of people",
                "Date",
                "Timeslot",
                "Done"
            };
            bool updating = true;
            while (updating)
            {
                Console.Clear();
                Console.WriteLine($"Current data for reservation {reservationNumber}:");
                Console.WriteLine(reservation.ToString());
                Console.WriteLine("\nUpdate:");
                int selectedOption = MenuSelector.RunMenuNavigator(updateOptions);
                switch (selectedOption)
                {
                    case 0:
                        Console.Clear();
                        Console.WriteLine("Enter the new amount of people that are coming:");
                        reservation.NumberOfPeople = ReservationSystem.GetNumberOfPeople(true);
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
                        updating = false;
                        return;
                    default:
                        break;
                }
                Console.Clear();
                Console.WriteLine("Updated reservation:\n");
                Console.WriteLine(reservation.ToString() + "\n");
                Console.WriteLine("\n[Press any key to continue updating this reservation.]");
                Console.ReadKey();
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
            Restaurant.Reservations.Remove(reservation);
            //ReservationSystem.UpdateJson();
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