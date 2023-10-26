using System.Text.Json;

class ReservationSystem
{
    static int selectedTableIndex = 0;
    static List<Table> tables = new List<Table>();
    static List<Reservation> reservations = new List<Reservation>(); // List to store reservations
    static int numberOfPeople = 0; // Store the number of people specified by the user
    static DateTime selectedDate; // Store the selected date

    static Random random = new Random();
    static int reservationNumber = 0;

    public void SystemRun()
    {
        // Ask the user for the number of people
        Console.Write("Enter the number of people in your group: ");
        while (!int.TryParse(Console.ReadLine(), out numberOfPeople) || numberOfPeople < 0)
        {
            Console.WriteLine("Invalid input. Please enter a valid number of people.");
        }

        //Path.Combine("data", "filename.json")

        // Ask the user for a date in the "dd-mm-yyyy" format
        Console.Write("Enter a date (dd-mm-yyyy): ");
        while (!DateTime.TryParseExact(Console.ReadLine(), "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out selectedDate))
        {
            Console.WriteLine("Invalid date format. Please enter a date in the dd-mm-yyyy format.");
        }

        Console.WriteLine("Select a time slot:");
        Console.WriteLine("1. Lunch: 12:00-14:00");
        Console.WriteLine("2. Afternoon: 14:00-16:00");
        Console.WriteLine("3. Evening: 16:00-18:00");
        Console.WriteLine("4. Dinner: 18:00-20:00");
        Console.WriteLine("5. Late Dinner: 20:00-22:00");

        int selectedTimeSlotIndex;
        do
        {
            Console.Write("Enter the number corresponding to your desired time slot: ");
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

        InitializeTables();

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

            Console.WriteLine("Select a table (use arrow keys, press Enter to confirm):");

            int rowCount = 4; // Number of rows
            int columnCount = 3; // Number of columns

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

                        Console.Write($"[{tables[index].TableNumber}] - Capacity: {tables[index].Capacity}   ");
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
        Table selectedTable = tables[selectedTableIndex];

        if (selectedTable.Capacity < numberOfPeople)
        {
            Console.WriteLine($"Table {selectedTable.TableNumber} does not have enough capacity for your group of {numberOfPeople} people.");
        }
        else
        {
            // Create a reservation object
            Reservation newReservation = new Reservation
            {
                ReservationNumber = GenerateReservationNumber(),
                NumberOfPeople = numberOfPeople,
                Date = selectedDate.ToString("dd-MMM-yyyy"),
                TimeSlot = selectedTimeSlot,
                SelectedTable = selectedTable,
            };

            // Add the reservation to the list
            reservations.Add(newReservation);

            // Save the reservation data to the JSON file
            SaveReservationDataToJson(reservations);

            Console.WriteLine($"You reserved Table {selectedTable.TableNumber} with a capacity of {selectedTable.Capacity} seats for your group of {numberOfPeople} people on {selectedDate:dd-MM-yyyy} for the timeslot: {selectedTimeSlot}.");
            Console.WriteLine($"Your reservation number: {newReservation.ReservationNumber}");
        }
    }

    static int GenerateReservationNumber()
    {
        return random.Next(1, 10000); // Generates a random number between 1 and 9999 (inclusive).
    }

    static void InitializeTables()
    {
        // Always initialize the tables list with default values
        tables = new List<Table>();
        for (int i = 1; i <= 9; i++)
        {
            tables.Add(new Table { TableNumber = i, Capacity = 4 });
        }

        // Check if the JSON file exists and load the table data if it does
        if (File.Exists("ReservationsData.json"))
        {
            string json = File.ReadAllText("ReservationsData.json");
            reservations = JsonSerializer.Deserialize<List<Reservation>>(json);

            // Create a list to store the table numbers that are taken
            List<int> takenTables = new List<int>();

            // Populate the takenTables list with table numbers from reservations
            foreach (Reservation reservation in reservations)
            {
                takenTables.Add(reservation.SelectedTable.TableNumber);
            }

            // Create a list of available tables based on the original tables list
            List<Table> availableTables = new List<Table>();

            foreach (Table table in tables)
            {
                if (!takenTables.Contains(table.TableNumber))
                {
                    availableTables.Add(table);
                }
            }

            // Update the tables list with availableTables
            tables = availableTables;
        }
        // No else block needed because tables are already initialized above
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
}