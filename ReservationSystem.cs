using Newtonsoft.Json;
using System.Diagnostics.Tracing;
using System.Text.Json;

public static class ReservationSystem // Made class static so loginsystem and dashboard don't rely on instances
{
    public static string TablesJson = "Tables.json"; //
    public static Reservation Reservation;
    public static List<Table> Tables;
    public static List<Reservation> Reservations = new List<Reservation>(); // List to store reservations
    public static Random Random = new Random();


    static ReservationSystem()
    {
        // all the restaurant tables are put in a list upon
        // initializing a ReservationSystem object
        Tables = InitializeTables(); // todo: change name
        // todo create mechanism that creates a new reservation object when reservating
        Reservation = new Reservation();
    }

    public static List<Table> InitializeTables()
    {
        List<Table> tables;

        if (File.Exists(TablesJson))
        {
            tables = ReadFromFile(TablesJson);
        }
        else
        {
            tables = new List<Table>()
            {
                new Table(1, (1, 1), 2, 7.50, false),
                new Table(2, (2, 1), 2, 7.50, false),
                new Table(3, (3, 1), 2, 7.50, false),
                new Table(4, (1, 2), 2, 7.50, false),
                new Table(5, (2, 2), 2, 7.50, false),
                new Table(6, (3, 2), 2, 7.50, false),
                new Table(7, (1, 3), 2, 7.50, false),
                new Table(8, (2, 3), 2, 7.50, false),
                new Table(9, (3, 3), 4, 10.0, false),
                new Table(10, (1, 4), 4, 10.0, false),
                new Table(11, (2, 4), 4, 10.0, false),
                new Table(12, (3, 4), 4, 10.0, false),
                new Table(13, (1, 5), 4, 10.0, false),
                new Table(14, (2, 5), 6, 15.0, false),
                new Table(15, (3, 5), 6, 15.0, false),
            };
            WriteToFile(tables, TablesJson);
        }
        return tables;
    }
    public static void WriteToFile(List<Table> tables, string TablesFileName) //todo change name
    {
        StreamWriter writer = new StreamWriter(TablesFileName);
        writer.Write(JsonConvert.SerializeObject(tables, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        writer.Close();
    }

    public static List<Table> ReadFromFile(string TablesFileName) //
    {
        StreamReader reader = new StreamReader(TablesFileName);
        string jsonString = reader.ReadToEnd();
        reader.Close();
        List<Table> tables = JsonConvert.DeserializeObject<List<Table>>(jsonString)!;
        return tables;
    }

    public static void RunSystem()
    {
        Reservate();
    }

    public static void Reservate()
    {
        Console.Write("Enter the number of people in your group: ");
        int numberOfPeople = GetNumberOfPeople();
        Reservation.NumberOfPeople = numberOfPeople;
        Deal deal = Restaurant.GetDealByName("Party Deal");
        if (deal != null)
        {
            PartyDeal partyDeal = PartyDeal.DealToPartyDeal(deal);
            bool isApplicable = partyDeal.DealIsApplicable(numberOfPeople);
            if (isApplicable) 
            { 
                partyDeal.DisplayDealIsAplied();
                Reservation.DealsApplied.Add(partyDeal);
                Reservation.Discount = partyDeal.DiscountFactor;
                Console.WriteLine("\n [Press any key to continue]");
                Console.ReadLine();
            }
        }
        
        Console.Clear();
        Console.Write("Enter a date (dd-mm-yyyy): ");
        DateOnly selectedDate = GetReservationDate();
        Reservation.Date = selectedDate;

        string timeslot = GetTimeslot();
        Console.WriteLine(timeslot);
        Reservation.TimeSlot = timeslot;

        Table selectedTable = GetChosenTable();
        Console.WriteLine(selectedTable);
        if (selectedTable != null)
        {
            Reservation.SelectedTable = selectedTable;
            Reservation.NonDiscountedPrice += selectedTable.TablePrice;
            int reservationNumber = GenerateReservationNumber();
            Reservation.ReservationNumber = reservationNumber;

            HandleSecondReservation();

            // todo: reservations must be stored somehow
        }
        else
        {
            Console.WriteLine("You weren't able to finish the reservation.");
            Reservation = new Reservation();
        }

    }


    public static void HandleSecondReservation()
    {
        bool bookAgain = true;
        while (true)
        {
            Console.Clear();
            DisplayReservationDetails();
            Console.WriteLine("\nWould you like to book the same table again today?");
            if (bookAgain)
            {
                Console.WriteLine(" > Yes");
                Console.WriteLine("   No");
            }
            else
            {
                Console.WriteLine("   Yes");
                Console.WriteLine(" > No");
            }
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.UpArrow && !bookAgain)
            {
                bookAgain = true;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && bookAgain)
            {
                bookAgain = false;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                if (bookAgain)
                {
                    if (BookAgain()) { break; }
                }
                else
                {
                    Console.WriteLine("Reservation has been made.");
                    break;
                }
            }
            else { continue; }
        }
    }

    public static bool BookAgain()
    {
        Console.Clear();
        Console.WriteLine("Booking again...\n");

        string secondtimeslot = GetTimeslot();
        Reservation.SecondTimeSlot = secondtimeslot;

        Table secondselectedTable = GetChosenTable();
        if (secondselectedTable != null)
        {
            Reservation.SecondSelectedTable = secondselectedTable;
            Reservation.NonDiscountedPrice += secondselectedTable.TablePrice;
            Console.Clear();
            DisplayReservationDetails();
            return true;
        }
        else
        {
            Console.WriteLine("You weren't able to finish the reservation.");
            Reservation = new Reservation();
        }
        return false;
    }


    public static int GetNumberOfPeople()
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
            //IsBiggerThan6 = numberOfPeople > 6;
            if (IsIncorrectFormat)
                Console.WriteLine("Invalid input. Please enter a valid number of people like: 7, 1, 12, etc");
            else if (IsSmallerThan0)
                Console.WriteLine("Invalid input. Please enter a number greater than 0.");
            //else if (IsBiggerThan6)
            //    Console.WriteLine("Invalid input. Our biggest table has 6 seats. Enter a number smaller than 6 or contact us for more information.");

        }
        while (IsIncorrectFormat || IsSmallerThan0 /*|| IsBiggerThan6*/);

        return numberOfPeople;
    }

    public static DateOnly GetReservationDate()
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

    public static string GetTimeslot()
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

    public static void DisplayTimeslots(List<string> timeslots, int selectedOption)
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

    public static void DisplayTablesMap()
    {
        string door = "Entrance";
        string aisle = "Main Aisle";

        // First row of tables (2 seats)
        DisplayTableRange(1, 4); // Tables 1-4

        Console.WriteLine("  {0}  ", door.PadRight(10)); // Entrance representation

        // Second row of tables (4 seats)
        DisplayTableRange(5, 8); // Tables 5-8

        Console.WriteLine("  {0}  ", new string(' ', 10)); // Space representing an aisle

        // Third row of tables (2 seats)
        DisplayTableRange(9, 12); // Tables 9-12

        Console.WriteLine("  {0}  ", aisle.PadRight(10)); // Main Aisle

        // Fourth row of tables (6 seats)
        DisplayTableRange(13, 15); // Tables 13-15
    }

    private static void DisplayTableRange(int startTable, int endTable)
    {
        // Display a range of tables
        for (int tableNumber = startTable; tableNumber <= endTable; tableNumber++)
        {
            Table table = Tables.FirstOrDefault(t => t.TableNumber == tableNumber);
            if (table != null)
            {
                if (table.IsReservated)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                // Display a single table with the appropriate number of seats
                if (tableNumber <= 4 || (tableNumber >= 9 && tableNumber <= 12)) // 2-seat tables
                    Console.Write($"  [ {table.TableNumber} ]  ");
                else if (tableNumber >= 5 && tableNumber <= 8) // 4-seat tables
                    Console.Write($"  [ {table.TableNumber} ]-[ {table.TableNumber} ]  ", table.TableNumber);
                else // 6-seat tables
                    Console.Write($"  [ {table.TableNumber} ]-[ {table.TableNumber} ]-[ {table.TableNumber} ]  ");
            }
            Console.ResetColor();
        }
        Console.WriteLine("\n");
    }


    private static (int, int) DetermineMaxCoordinates(List<Table> tables)
    {
        int maxX = tables.Max(t => t.Coordinate.Item1);
        int maxY = tables.Max(t => t.Coordinate.Item2);
        return (maxX, maxY);
    }

    public static Table GetChosenTable()
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
                TableSelectionFeedback(selectedTable);
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


        return selectedTable;
    }

    public static void TableSelectionFeedback(Table selectedTable)
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

    public static Table ShowSelectedTable(int xc, int yc)
    {
        Table selectedTable = null;
        int availableTablesCount = 0;

        // colors tables that are already booked red
        // tables with too little seats gray
        // puts [] around the selected table
        foreach (Table table in Tables)
        {
            string ws = table.TableNumber < 10 ? " " : ""; // ws = whitespace to format the table options
            string wst = table.TablePrice < 10 ? " " : ""; // wst = whitespace table to format the table options

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
            else
            {
                Console.ForegroundColor = ConsoleColor.Green; // Green for available
            }

            string tablePrice = table.TablePrice.ToString("F2");
            if (table.Coordinate.Item1 == xc && table.Coordinate.Item2 == yc)
            {
                Console.Write($"[Table {table.TableNumber} {ws} {table.Capacity}   Seats ${tablePrice}]  {wst}");
                selectedTable = table;
            }
            else
            {
                Console.Write($" Table {table.TableNumber} {ws} {table.Capacity}   Seats ${tablePrice}   {wst}");
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

    public static void DisplayReservationDetails()
    {
        Reservation R = Reservation;
        Table T = R.SelectedTable;
        Table T2 = R.SecondSelectedTable;
        string numOfPeople = R.NumberOfPeople > 1 ? $"{R.NumberOfPeople} guests" : $"{R.NumberOfPeople} guest";

        Console.Clear();
        Console.WriteLine("R E S E R V A T I O N   D E T A I L S\n");
        Console.WriteLine($"You reservated Table {T.TableNumber} for {numOfPeople} on {R.Date} during {R.TimeSlot}.");
        if (T2 != null)
        {
            Console.WriteLine($"You reservated Table {T2.TableNumber} for {numOfPeople} on {R.Date} during {R.SecondTimeSlot}.");
        }
        Console.WriteLine($"Your reservation number: {R.ReservationNumber}");
        Console.WriteLine($"Deals applied:");
        if (R.DealsApplied.Count == 0)
        {
            Console.WriteLine("  > No deals were applied to this reservation.");
        }
        else
        {
            foreach (Deal deal in R.DealsApplied)
            {
                Console.WriteLine($"  > {deal.Name} ({deal.DiscountFactor * 100}% discount)");
            }
        }
        Console.WriteLine($"Total Price: {R.GetTotalPrice()}");
        
    }

    public static int GenerateReservationNumber()
    {
        return Random.Next(1, 10000); // Generates a random number between 1 and 9999 (inclusive).
    }
    public static void UpdateJson()
    {
        Utensils.WriteJson("Reservations.json", Reservations);
    }
}