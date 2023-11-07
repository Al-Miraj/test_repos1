using System;
using System.Text;
using System.Text.Json;

public class ReservationSystem
{

    static int selectedTableIndex = 0;
    static List<Table> tables = new List<Table>();
    static List<Reservation> reservations = new List<Reservation>(); // List to store reservations
    static int numberOfPeople = 0; // Store the number of people specified by the user
    static DateTime selectedDate; // Store the selected date

    static Random random = new Random();

    public void SystemRun()
    {
        // Ask the user for the number of people
        Console.Write("\nEnter the number of people in your group: ");
        while (!int.TryParse(Console.ReadLine(), out numberOfPeople) || numberOfPeople <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a valid number of people.");
        }


        // Ask the user for a date in the "dd-mm-yyyy" format
        Console.Write("\nEnter a date (dd-mm-yyyy): ");
        while (true)
        {
            string input = Console.ReadLine();

            if (DateTime.TryParseExact(input, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out selectedDate))
            {
                // Check if the selected date is the date of tomorrow or in the future
                if (selectedDate.Date >= DateTime.Today.AddDays(1))
                {
                    break; // Valid date, exit the loop
                }
                else
                {
                    Console.WriteLine("\nPlease enter a date from tomorrow onwards.\n");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid date format. Please enter a date in the dd-MM-yyyy format.\n");
            }
        }

        Console.WriteLine("\nSelect a time slot:");
        Console.WriteLine("1. Lunch: 12:00-14:00");
        Console.WriteLine("2. Afternoon: 14:00-16:00");
        Console.WriteLine("3. Evening: 16:00-18:00");
        Console.WriteLine("4. Dinner: 18:00-20:00");
        Console.WriteLine("5. Late Dinner: 20:00-22:00");

        int selectedTimeSlotIndex;
        do
        {
            Console.Write("\nEnter the number corresponding to your desired time slot: ");
        } while (!int.TryParse(Console.ReadLine(), out selectedTimeSlotIndex) || selectedTimeSlotIndex < 1 || selectedTimeSlotIndex > 5);

        string selectedTimeSlot;
        switch (selectedTimeSlotIndex)
        {
            case 1:
                selectedTimeSlot = "Lunch: 12:00-14:00";
                break;
            case 2:
                selectedTimeSlot = "Afternoon: 14:00-16:00";
                break;
            case 3:
                selectedTimeSlot = "Evening: 16:00-18:00";
                break;
            case 4:
                selectedTimeSlot = "Dinner: 18:00-20:00";
                break;
            case 5:
                selectedTimeSlot = "Late Dinner: 20:00-22:00";
                break;
            default:
                selectedTimeSlot = "Unknown";
                break;
        }

        InitializeTables(selectedTimeSlot);
        if (tables.Count == 0)
        {
            Console.WriteLine("\nApologies, there are no tables available.\n");
            return; // Exit the program
        }


        // Map needs to be updated
        ConsoleKeyInfo keyInfo;
        do
        {
            Console.WriteLine("  ______    _____    _____  ");
            Console.WriteLine(" /      \\  /     \\  /     \\  ");
            Console.WriteLine("|   ___  |   ___  |   ___  | ");
            Console.WriteLine("|  |   |    |   |    |   | | ");
            Console.WriteLine("|  |[1]|    |[2]|    |[3]| | ");
            Console.WriteLine("|  |___|    |___|    |___| | ");
            Console.WriteLine("|                          | ");
            Console.WriteLine("|   ___      ___      ___  | ");
            Console.WriteLine("|  |   |    |   |    |   | | ");
            Console.WriteLine("|  |[4]|    |[5]|    |[6]| | ");
            Console.WriteLine("|  |___|    |___|    |___| | ");
            Console.WriteLine("|                          | ");
            Console.WriteLine("|   ___      ___      ___  | ");
            Console.WriteLine("|  |   |    |   |    |   | | ");
            Console.WriteLine("|  |[7]|    |[8]|    |[9]| | ");
            Console.WriteLine("|  |___|    |___|    |___| | ");
            Console.WriteLine("|                          | ");
            Console.WriteLine("|________|________|________| ");

            Console.WriteLine("\nSelect a table (use arrow keys, press Enter to confirm):");

            int rowCount = 6; // Number of rows
            int columnCount = 4; // Number of columns

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    int index = row * columnCount + col;
                    if (index < tables.Count)
                    {
                        Table table = tables[index];
                        ConsoleColor backgroundColor = (index == selectedTableIndex) ? ConsoleColor.Blue : ConsoleColor.Black;

                        Console.BackgroundColor = backgroundColor;

                        Console.Write($"[{tables[index].TableNumber}] - Capacity: {tables[index].Capacity} - Price: €{tables[index].TablePrice}   ");
                    }
                    else
                    {
                        Console.ResetColor();
                    }
                }
                Console.WriteLine(); // Move to the next row
            }
            keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                selectedTableIndex = Math.Max(0, selectedTableIndex - 3);
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                selectedTableIndex = Math.Min(tables.Count - 1, selectedTableIndex + 3);
            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                selectedTableIndex = Math.Max(0, selectedTableIndex - 1);
            }
            else if (keyInfo.Key == ConsoleKey.RightArrow)
            {
                selectedTableIndex = Math.Min(tables.Count - 1, selectedTableIndex + 1);
            }
        } while (keyInfo.Key != ConsoleKey.Enter);

        // User has confirmed the selection
        Table selectedTable = new Table(tables[selectedTableIndex].TableNumber, tables[selectedTableIndex].Capacity, tables[selectedTableIndex].TablePrice);

        if (tables[selectedTableIndex].Capacity < numberOfPeople)
        {
            Console.WriteLine($"\nTable {selectedTable.TableNumber} does not have enough capacity for your group of {numberOfPeople} people.\n");
        }
        else
        {
            Console.WriteLine("\nWould you like to reserve the same table a second time for the same day? (Y/N): \n");
            string secondReservationConfirmation = Console.ReadLine().Trim().ToUpper();

            while (true)
            {
                if (secondReservationConfirmation == "N")
                {
                    while (true)
                    {
                        // Show a summary of the chosen options
                        Console.WriteLine("\nReservation Summary:");
                        Console.WriteLine($"Number of People: {numberOfPeople}");
                        Console.WriteLine($"Date: {selectedDate:dd-MM-yyyy}");
                        Console.WriteLine($"Time Slot: {selectedTimeSlot}");
                        Console.WriteLine($"Selected Table: Table {selectedTable.TableNumber} (Capacity: {selectedTable.Capacity} - Price: €{selectedTable.TablePrice})");

                        // Ask for confirmation


                        Console.Write("\nIs this information correct? (Y/N): \n");
                        string userReservationConfirmation = Console.ReadLine().Trim().ToUpper();

                        if (userReservationConfirmation == "N")
                        {
                            // If details not correct, start over
                            SystemRun();
                            return;
                        }
                        else if (userReservationConfirmation == "Y")
                        {
                            string noSecondTimeSlot = "Not Selected";
                            // Create a reservation object
                            Reservation newReservation = new Reservation(
                                GenerateReservationNumber(),
                                numberOfPeople,
                                selectedDate.ToString("dd-MMM-yyyy"),
                                selectedTimeSlot,
                                noSecondTimeSlot,
                                selectedTable
                            );


                            // Add the reservation to the list
                            reservations.Add(newReservation);

                            // Save the reservation data to the JSON file
                            SaveReservationDataToJson(reservations);

                            Console.WriteLine($"\nYou reserved Table {selectedTable.TableNumber} with a capacity of {selectedTable.Capacity} seats and a price of €{selectedTable.TablePrice} for your group of {numberOfPeople} people on {selectedDate:dd-MM-yyyy} for the timeslot: {selectedTimeSlot}.");
                            Console.WriteLine($"Your reservation number: {newReservation.ReservationNumber}\n");

                            return;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter either 'Y' or 'N'.");
                        }
                    }
                }
                else if (secondReservationConfirmation == "Y")
                {
                    Console.WriteLine("\nSelect a time slot:");
                    Console.WriteLine("1. Lunch: 12:00-14:00");
                    Console.WriteLine("2. Afternoon: 14:00-16:00");
                    Console.WriteLine("3. Evening: 16:00-18:00");
                    Console.WriteLine("4. Dinner: 18:00-20:00");
                    Console.WriteLine("5. Late Dinner: 20:00-22:00");

                    int selectedTimeSlotIndexTwo;
                    do
                    {
                        Console.Write("\nEnter the number corresponding to your desired time slot: ");
                    } while (!int.TryParse(Console.ReadLine(), out selectedTimeSlotIndexTwo) || selectedTimeSlotIndexTwo < 1 || selectedTimeSlotIndexTwo > 5);


                    string selectedTimeSlotTwo;
                    switch (selectedTimeSlotIndexTwo)
                    {
                        case 1:
                            selectedTimeSlotTwo = "Lunch: 12:00-14:00";
                            break;
                        case 2:
                            selectedTimeSlotTwo = "Afternoon: 14:00-16:00";
                            break;
                        case 3:
                            selectedTimeSlotTwo = "Evening: 16:00-18:00";
                            break;
                        case 4:
                            selectedTimeSlotTwo = "Dinner: 18:00-20:00";
                            break;
                        case 5:
                            selectedTimeSlotTwo = "Late Dinner: 20:00-22:00";
                            break;
                        default:
                            selectedTimeSlotTwo = "Unknown";
                            break;
                    }

                    while (true)
                    {
                        // Show a summary of the chosen options
                        Console.WriteLine("\nReservation Summary:");
                        Console.WriteLine($"Number of People: {numberOfPeople}");
                        Console.WriteLine($"Date: {selectedDate:dd-MM-yyyy}");
                        Console.WriteLine($"Time Slots: {selectedTimeSlot} & {selectedTimeSlotTwo}");
                        Console.WriteLine($"Selected Table: Table {selectedTable.TableNumber} (Capacity: {selectedTable.Capacity} - Price: €{selectedTable.TablePrice})");

                        // Ask for confirmation


                        Console.Write("\nIs this information correct? (Y/N): \n");
                        string userReservationConfirmation = Console.ReadLine().Trim().ToUpper();

                        if (userReservationConfirmation == "N")
                        {
                            // If details not correct, start over
                            SystemRun();
                            return;
                        }
                        else if (userReservationConfirmation == "Y")
                        {
                            // Create a reservation object
                            Reservation newReservation = new Reservation(
                                GenerateReservationNumber(),
                                numberOfPeople,
                                selectedDate.ToString("dd-MMM-yyyy"),
                                selectedTimeSlot,
                                selectedTimeSlotTwo,
                                selectedTable
                            );



                            // Add the reservation to the list
                            reservations.Add(newReservation);

                            // Save the reservation data to the JSON file
                            SaveReservationDataToJson(reservations);

                            Console.WriteLine($"\nYou reserved Table {selectedTable.TableNumber} with a capacity of {selectedTable.Capacity} seats and a price of €{selectedTable.TablePrice} for your group of {numberOfPeople} people on {selectedDate:dd-MM-yyyy} for the time slots: {selectedTimeSlot} and {selectedTimeSlotTwo}.");
                            Console.WriteLine($"Your reservation number: {newReservation.ReservationNumber}\n");

                            return;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter either 'Y' or 'N'.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter either 'Y' or 'N'.");
                    secondReservationConfirmation = Console.ReadLine().Trim().ToUpper();
                }
            }
        }
    }

    static int GenerateReservationNumber()
    {
        return random.Next(1, 10000); // Generates a random number between 1 and 9999 (inclusive).
    }

    static void InitializeTables(string selectedTimeSlot)
    {
        // Always initialize the tables list with default values
        tables = new List<Table>();

        // 8 tables with each 2 seats
        for (int i = 1; i <= 8; i++)
        {
            tables.Add(new Table(i, 2, 7.50));
        }

        // 5 tables with each 4 seats
        for (int i = 9; i <= 13; i++)
        {
            tables.Add(new Table(i, 4, 10));
        }

        // 2 tables with each 6 seats
        for (int i = 14; i <= 15; i++)
        {
            tables.Add(new Table(i, 6, 15));
        }

        //  8 bar seats
        for (int i = 16; i <= 23; i++)
        {
            tables.Add(new Table(i, 1, 5));
        }

        // Check if the JSON file exists and load the table data if it does
        if (File.Exists("ReservationsData.json"))
        {
            string json = File.ReadAllText("ReservationsData.json");
            reservations = JsonSerializer.Deserialize<List<Reservation>>(json);

            foreach (Reservation reservation in reservations)
            {
                // Filter out reservations that have passed
                if (DateTime.ParseExact(reservation.Date, "dd-MMM-yyyy", null) >= DateTime.Today)
                {
                    // Find the corresponding table
                    Table table = tables.Find(t => t.TableNumber == reservation.SelectedTable.TableNumber);

                    // Check if the reservation's time slots overlap with the selected time slot
                    if (IsTimeSlotAvailable(table, selectedDate, selectedTimeSlot))
                    {
                        // Add the reservation to the table's Reservations list
                        table.Reservations.Add(reservation);
                    }
                }
            }

            // Update the tables list with available tables
            tables = tables.Where(t => IsTableAvailable(t, selectedDate, selectedTimeSlot)).ToList();
        }

    }

    private static bool IsTimeSlotAvailable(Table table, DateTime selectedDate, string selectedTimeSlot)
    {
        // Check if any of the table's reservations overlap with the selected time slot
        return table.Reservations.All(reservation =>
        {
            return !(reservation.Date == selectedDate.ToString("dd-MMM-yyyy") &&
                     (reservation.TimeSlot == selectedTimeSlot || reservation.TimeSlotTwo == selectedTimeSlot));
        });
    }

    private static bool IsTableAvailable(Table table, DateTime selectedDate, string selectedTimeSlot)
    {
        // Check if the table has no reservations that overlap with the selected time slot
        return table.Reservations.All(reservation =>
        {
            return !(reservation.Date == selectedDate.ToString("dd-MMM-yyyy") &&
                     (reservation.TimeSlot == selectedTimeSlot || reservation.TimeSlotTwo == selectedTimeSlot));
        });
    }



    static void SaveReservationDataToJson(List<Reservation> reservations)
    {
        try
        {
            // Serialize the reservations list to JSON
            string json = JsonSerializer.Serialize(reservations, new JsonSerializerOptions { WriteIndented = true });

            // Save the JSON data to the file "ReservationsData.json"
            File.WriteAllText("ReservationsData.json", json);

            Console.WriteLine("Reservation data has been successfully saved.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving the reservation data: {ex.Message}");
        }
    }


    class Table
    {
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public double TablePrice { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        public Table(int tableNumber, int capacity, double tablePrice)
        {
            TableNumber = tableNumber;
            Capacity = capacity;
            TablePrice = tablePrice;
        }
    }


    class Reservation
    {
        public int ReservationNumber { get; set; }
        public int NumberOfPeople { get; set; }
        public string Date { get; set; }
        public string TimeSlot { get; set; }
        public string? TimeSlotTwo { get; set; } // Nullable since it's optional
        public Table SelectedTable { get; set; }

        public Reservation(int reservationNumber, int numberOfPeople, string date, string timeSlot, string? timeSlotTwo, Table selectedTable)
        {
            ReservationNumber = reservationNumber;
            NumberOfPeople = numberOfPeople;
            Date = date;
            TimeSlot = timeSlot;
            TimeSlotTwo = timeSlotTwo;
            SelectedTable = selectedTable;
        }

    }

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8; // Set the console encoding to UTF-8 - Needed for the EUR sign
        ReservationSystem reservationSystem = new ReservationSystem();
        reservationSystem.SystemRun();
    }

}