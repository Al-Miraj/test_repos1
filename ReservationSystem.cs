using Newtonsoft.Json;
using System.Text.Json;

class ReservationSystem
{
    public string TablesJson = "Tables.json"; //
    public Reservation Reservation;
    static int selectedTableIndex = 0;
    static List<Table> Tables;
    static List<Reservation> reservations = new List<Reservation>(); // List to store reservations
    static int numberOfPeople = 0; // Store the number of people specified by the user
    static DateTime selectedDate; // Store the selected date

    static Random random = new Random();
    static int reservationNumber = 0;

    public ReservationSystem()
    {
        // all the restaurant tables are put in a list upon
        // initializing a ReservationSystem object
        Tables = InitializeTabless(); // todo: change name
        // todo create mechanism that creates a new reservation object when reservating
        Reservation = new Reservation(); 
    }

    public List<Table> InitializeTabless() 
    {
        List<Table> tables;

        if (File.Exists(TablesJson))
        {
            tables = ReadFromFile(TablesJson);
        }
        else
        {
            // Create the JSON file and write integers 1 to 
            tables = new List<Table>()
            {
                new Table(1, (1, 1), 2, false),
                new Table(2, (2, 1), 2, false),
                new Table(3, (3, 1), 2, false),
                new Table(4, (1, 2), 2, false),
                new Table(5, (2, 2), 2, false),
                new Table(6, (3, 2), 2, false),
                new Table(7, (1, 3), 2, false),
                new Table(8, (2, 3), 2, false),
                new Table(9, (3, 3), 4, false),
                new Table(10, (1, 4), 4, false),
                new Table(11, (2, 4), 4, false),
                new Table(12, (3, 4), 4, false),
                new Table(13, (1, 5), 4, false),
                new Table(14, (2, 5), 6, false),
                new Table(15, (3, 5), 6, false),
            };
            WriteToFile(tables, TablesJson);
        }
        return tables;
    }
    public void WriteToFile(List<Table> tables, string TableFileName) //todo change name
    {
        StreamWriter writer = new StreamWriter(TableFileName);
        writer.Write(JsonConvert.SerializeObject(tables, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        writer.Close();
    }

    public List<Table> ReadFromFile(string TableFileName) //
    {
        StreamReader reader = new StreamReader(TableFileName);
        string jsonString = reader.ReadToEnd();
        reader.Close();
        List<Table> tables = JsonConvert.DeserializeObject<List<Table>>(jsonString)!;
        return tables;
    }

    public void RunSystem()
    {
        Reservate();
    }

    public void Reservate()
    {
        Console.Write("Enter the number of people in your group: ");
        int numberOfPeople = GetNumberOfPeople();
        Reservation.NumberOfPeople = numberOfPeople;

        Console.Write("Enter a date (dd-mm-yyyy): ");
        DateOnly selectedDate = GetReservationDate();
        Reservation.Date = selectedDate;

        string timeslot = GetTimeslot();
        Console.WriteLine(timeslot);
        Reservation.TimeSlot = timeslot;
    }

    public int GetNumberOfPeople()
    {
        int numberOfPeople;
        bool IsIncorrectFormat;
        bool IsSmallerThan0;
        bool IsBiggerThan6;

        do
        {
            string number = Console.ReadLine().Trim();
            IsIncorrectFormat = !int.TryParse(number, out numberOfPeople); //true if the format is incorrect
            IsSmallerThan0 = numberOfPeople <= 0;
            IsBiggerThan6 = numberOfPeople > 6;
            if (IsIncorrectFormat)
                Console.WriteLine("Invalid input. Please enter a valid number of people like: 7, 1, 12, etc");
            else if (IsSmallerThan0)
                Console.WriteLine("Invalid input. Please enter a number greater than 0.");
            else if (IsBiggerThan6)
                Console.WriteLine("Invalid input. Our biggest table has 6 seats. Enter a number smaller than 6 or contact us for more information.");

        }
        while (IsIncorrectFormat || IsSmallerThan0 || IsBiggerThan6);

        Console.Clear();
        return numberOfPeople;
    }

    public DateOnly GetReservationDate()
    {
        DateOnly reservationDate;
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        DateOnly yearFromNow = today.AddYears(1);
        bool formatIsIncorrect;
        bool dateHasPassed;
        bool dateIsMoreThanYearFromNow;

        do
        {
            string date = Console.ReadLine().Trim();

            // formatIsIncorrect is true if the format is incorrect
            formatIsIncorrect = !DateOnly.TryParseExact(date, "d-M-yyyy", out reservationDate);
            dateHasPassed = reservationDate < today;
            dateIsMoreThanYearFromNow = reservationDate > yearFromNow;
            if (formatIsIncorrect)
            {
                Console.WriteLine($"Invalid date format '{date}'. Please enter a date in the dd-mm-yyyy format.");
            }
            else if (dateHasPassed)
            {
                Console.WriteLine($"Invalid date '{date}'. Please enter a date that has not already passed.");
            }
            else if (dateIsMoreThanYearFromNow)
            {
                Console.WriteLine($"Invalid date '{date}'. Please enter a date that has not more than a year from today.");
            }

        }
        while (formatIsIncorrect || dateHasPassed);

        Console.Clear();
        return reservationDate;
    }

    public string GetTimeslot()
    {
        Console.CursorVisible = false;
        int selectedOption = 0;
        List<string> timeslots = new List<string>()
        {   "Lunch (12:00-14:00)",
            "The Afternoon (14:00-16:00)",
            "The Evening (16:00-18:00)",
            "Dinner (18:00-20:00)",
            "Late Dinner (20:00-22:00)"
        };
        string timeslot;

        while (true)
        {
            DisplayTimeslots(timeslots, selectedOption);

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.UpArrow && selectedOption > 0)
            {
                selectedOption--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && selectedOption < 4)
            {
                selectedOption++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                timeslot = timeslots[selectedOption];
                break;
            }
        }

        Console.CursorVisible = true;
        return timeslot;
    }

    public void DisplayTimeslots(List<string> timeslots, int selectedOption)
    {
        Console.Clear();
        Console.WriteLine("Select a time slot:");
        for (int i = 0; i < 5; i++)
        {
            if (i == selectedOption)
            {
                Console.WriteLine($"> {timeslots[i]}");
            }
            else
            {
                Console.WriteLine($"  {timeslots[i]}");
            }
        }
    }

    public void DisplayTablesMap()
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
    }

    public Table GetChosenTable()
    {
        Console.CursorVisible = false;
        ConsoleKeyInfo keyInfo;

        int xc = 1;
        int yc = 1;
        Table selectedTable;

        while (true)
        {
            Console.Clear();
            DisplayTablesMap();
            selectedTable = ShowSelectedTable(xc, yc);

            if (selectedTable != null)
            {
                keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.UpArrow && yc > 1)
                {
                    yc--;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow && yc < 5)
                {
                    yc++;
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow && xc > 1)
                {
                    xc--;
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow && xc < 3)
                {
                    xc++;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (!selectedTable.IsReservated) // the table has not been reservated yet
                    {
                        if (Reservation.NumberOfPeople <= selectedTable.Capacity) // the table has enough seats
                        {
                            selectedTable.IsReservated = true; // table at that coordinate has now been reservated
                            WriteToFile(Tables, TablesJson);
                            break;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Sorry! We are booked!");
                break;
            }

        }

        TableSelectionFeedback(selectedTable);

        return selectedTable;
    }

    public void TableSelectionFeedback(Table selectedTable)
    {
        if (selectedTable.IsReservated)
        {
            Console.WriteLine($"\nTable {selectedTable.TableNumber} has already been reservated. try another!");
        }
        else if (Reservation.NumberOfPeople > selectedTable.Capacity)
        {
            Console.WriteLine($"\nTable {selectedTable.TableNumber} does not have enough seats for you. Try another!");
        }
    }

    public Table ShowSelectedTable(int xc, int yc)
    {
        Table selectedTable = null;
        int availableTablesCount = 0;

        // colors tables that are already booked red
        // tables with too little seats gray
        // puts [] around the selected table
        foreach (Table table in Tables)
        {
            string ws = table.TableNumber < 10 ? " " : ""; // ws = whitespace to format the table options

            if (table.IsReservated)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                availableTablesCount++;
            }
            else if (Reservation.NumberOfPeople > table.Capacity)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                availableTablesCount++;
            }
            if (table.Coordinate.Item1 == xc && table.Coordinate.Item2 == yc)
            {
                Console.Write($"[Table {table.TableNumber} {ws} {table.Capacity} Seats]  ");
                selectedTable = table;
            }
            else
            {
                Console.Write($" Table {table.TableNumber} {ws} {table.Capacity} Seats   ");
            }
            Console.ResetColor();

            if (table.Coordinate.Item1 % 3 == 0)
            {
                Console.WriteLine(); //creates new row after every 3 tables
            }
        }

        if (availableTablesCount >= Tables.Count) // None of the tables are available for this reservation
        {
            return null;
        }
        return selectedTable;
    }







    public void RunSystem()
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
                    if (index < Tables.Count)
                    {
                        Table table = Tables[index];
                        ConsoleColor backgroundColor = (index == selectedTableIndex) ? ConsoleColor.Blue : ConsoleColor.Black;

                        Console.BackgroundColor = backgroundColor;

                        Console.Write($"[{Tables[index].TableNumber}] - Capacity: {Tables[index].Capacity}   ");
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
                selectedTableIndex = Math.Min(Tables.Count - 1, selectedTableIndex + 3);
            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                selectedTableIndex = Math.Max(0, selectedTableIndex - 1);
            }
            else if (keyInfo.Key == ConsoleKey.RightArrow)
            {
                selectedTableIndex = Math.Min(Tables.Count - 1, selectedTableIndex + 1);
            }
        } while (keyInfo.Key != ConsoleKey.Enter);

        // User has confirmed the selection
        Table selectedTable = Tables[selectedTableIndex];

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
        Tables = new List<Table>();
        for (int i = 1; i <= 9; i++)
        {
            Tables.Add(new Table { TableNumber = i, Capacity = 4 });
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

            foreach (Table table in Tables)
            {
                if (!takenTables.Contains(table.TableNumber))
                {
                    availableTables.Add(table);
                }
            }

            // Update the tables list with availableTables
            Tables = availableTables;
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