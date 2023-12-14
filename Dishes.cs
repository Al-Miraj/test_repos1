using Menus;

public class Dish
{
    //alles voor holiday
    private static List<Dish> rawHolidayMenu = JsonFileHandler.ReadFromFile<Dish>("HolidayDish.json");
    private static List<Dish> finishedHolidayMenu = new List<Dish>();
    private static List<DateTime> keys = new List<DateTime>();
    private static Dictionary<DateTime, string> holidays = new Dictionary<DateTime, string>
    {
        // International holidays
        { new DateTime(DateTime.Now.Year, 1, 1), "New Year" },
        { new DateTime(DateTime.Now.Year, 12, 25), "Christmas" },
        { new DateTime(DateTime.Now.Year, 12, 26), "Christmas" },
        { new DateTime(DateTime.Now.Year, 12, 27), "Christmas" },
        { new DateTime(DateTime.Now.Year, 12, 28), "Christmas" },
        { new DateTime(DateTime.Now.Year, 12, 29), "Christmas" },
        { new DateTime(DateTime.Now.Year, 12, 30), "Christmas" },
        { new DateTime(DateTime.Now.Year, 12, 31), "Christmas" },
        { new DateTime(DateTime.Now.Year, 10, 31), "Halloween" },

        // Nederlandse feestdagen

        { new DateTime(DateTime.Now.Year, 4, 27), "Koningsdag" },
        //{ new DateTime(DateTime.Now.Year, 5, 5), "Bevrijdingsdag" },
        { new DateTime(DateTime.Now.Year, 12, 5), "Sinterklaas" },
        { new DateTime(DateTime.Now.Year, 2, 28), "Carnaval" },
    }
    public static bool CheckDate(DateTime date) => holidays.ContainsKey(date) ? true : false;
    private static void addKeys() => keys.AddRange(holidays.Keys);

    public static void DisplayAllHolidays()
    {
        bool christmasDisplayed = false;

        foreach (var holiday in holidays)
        {
            if (holiday.Value == "Christmas")
            {
                if (!christmasDisplayed)
                {
                    Console.WriteLine($"{holiday.Value} - Date: 25-12-2023 to 31-12-2023");
                    christmasDisplayed = true;
                }
            }
            else
            {
                Console.WriteLine($"{holiday.Value} - Date: {holiday.Key.ToShortDateString()}");
            }
        }
    }

    public static void getHoliday(DateTime key)
    {
        addKeys();

        if (CheckDate(key))
        {
            finishedHolidayMenu.Clear();
            Console.WriteLine($"today is {holidays[key]}");
            holidayNavigator(holidays[key]);
            PrintInfo();
            return;
        }

        GetNearestMenu();
    }

    public static void GetNearestMenu()
    {
        Console.WriteLine("No holiday found for today.");
        Console.WriteLine("Do you want to see the menu of the nearest holiday?");
        int choice = MenuSelector.RunMenuNavigator(new List<string>() { "yes", "no" });
        Console.Clear();
        Console.WriteLine();

        if (choice == 0) // 'yes' was selected
        {
            string closestHoliday = FindNextHoliday();
            holidayNavigator(closestHoliday);
            Console.WriteLine($"Displaying the menu of the nearest holiday: {closestHoliday}");
            Console.WriteLine();
            Thread.Sleep(250);
            PrintInfo();
            Console.WriteLine();
            Console.WriteLine();
        }

        Console.WriteLine("Here are the holidays as reference: ");
        Console.WriteLine();
        DisplayAllHolidays();
        Console.WriteLine();
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    public static void holidayNavigator(string holiday)
    {
        //switch case voor elke holiday om later tijdens het debuggen problemen makkelijker op te lossen
        //dit kan later zonder switch case door direct de holiday mee te geven aab getHolidayMenu
        switch (holiday)
        {
            case "New Year":
                getHolidayMenu("New Year");
                break;
            case "Christmas":
                getHolidayMenu("Christmas");
                break;
            case "Halloween":
                getHolidayMenu("Halloween");
                break;
            case "Koningsdag":
                getHolidayMenu("Koningsdag");
                break;
            //case "Bevrijdingsdag":
            // Console.WriteLine("Bevrijdingsdag");
            //getHolidayMenu("Bevrijdingsdag");
            //break;
            case "Sinterklaas":
                getHolidayMenu("Sinterklaas");
                break;
            case "Carnaval":
                getHolidayMenu("Carnaval");
                break;
            default:
                Console.WriteLine("Holiday not found.");
                break;
        }
    }

    public static void getHolidayMenu(string holiday)
    {
        List<Dish> holidayMenu = new List<Dish>();
        holidayMenu = rawHolidayMenu.FindAll(x => x.Holiday == holiday);
        finishedHolidayMenu.AddRange(holidayMenu);
    }

    public static string FindNextHoliday()
    {
        DateTime date = DateTime.Now.Date;
        DateTime holidayDate = keys.Where(x => x >= date).FirstOrDefault();


        if (holidayDate != null && holidays.ContainsKey(holidayDate))
        {
            return holidays[holidayDate];
        }
        else
        {
            return "No upcoming holiday found";
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------

    public static List<Dish> DishsRaw = JsonFileHandler.ReadFromFile<Dish>("SeasonMenu.json");
    public static string? GetSeason(int month)
    {
        // Determine the season based on the month
        switch (month)
        {
            case 12:
            case 1:
            case 2:
                return "Winter";

            case 3:
            case 4:
            case 5:
                return "Spring";

            case 6:
            case 7:
            case 8:
                return "Summer";

            case 9:
            case 10:
            case 11:
                return "Autumn";

            default:
                return null;
        }
    }

    public static List<Dish>? MainNavigator()
    {
        DateTime currentDateTime = DateTime.Today;
        int month = currentDateTime.Month;
        string? season = GetSeason(month);
        if (season != null) { var menu = SeasonNavigator(season); return menu; }
        return null;

    }

    public static List<Dish>? SeasonNavigator(string season)
    {
        switch (season)
        {
            case "Winter":
                return GetDishs("Winter");

            case "Spring":
                return GetDishs("Spring");

            case "Summer":
                return GetDishs("Summer");

            case "Autumn":
                return GetDishs("Autumn");

            default:
                return null;
        }
    }

    public static List<Dish> GetDishs(string season)
    {
        List<Dish> seasonMenu = DishsRaw.FindAll(x => x.Season == season).OrderBy(x => x.Price).ToList();
        return seasonMenu;
    }

    public static void PrintInfo(List<Dish> dishlist, bool keyContinue = true)
    {
        string currentHoliday = "";
        var lastDish = dishlist.LastOrDefault();
        foreach (var dish in dishlist)
        {
            if (currentHoliday != dish.Name)
            {
                Console.WriteLine("------------------------------------------------------------------------------------");
                Console.WriteLine();
                currentHoliday = dish.Name;
            }

            Console.WriteLine($"Name: {dish.Name}");
            Console.WriteLine($"Description: {dish.Description}");
            Console.WriteLine($"Ingredients: {string.Join(", ", dish.Ingredients)}");
            Console.WriteLine($"Timeslot: {dish.Timeslot}");
            Console.WriteLine($"Price: {dish.Price}");
            Console.WriteLine($"Potential Allergens: {string.Join(", ", dish.PotentialAllergens)}");
            Console.WriteLine($"Icon: {dish.Category}");
            if (dish.Holiday != "")
                Console.WriteLine($"Holiday: {dish.Holiday}");
            Console.WriteLine($"Season: {dish.Season}");
            Console.WriteLine();

            if (dish == lastDish)
            {
                Console.WriteLine("------------------------------------------------------------------------------------");
                Console.WriteLine();
            }

        }
        if (keyContinue)
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    private static int selectedFoodMenuOption = MenuSelector.RunMenuNavigator(new List<string>() { " Lunch", " Dinner", " Filter Menu", " Exit" });

    public static List<Dish> rawFoodMenu = JsonFileHandler.ReadFromFile<Dish>("Dish.json");
    public static List<Dish> finishedFoodMenu = new List<Dish> { };

    private static bool HandleSelection()
    {
        Console.Clear();
        bool exitMenu = false;

        switch (selectedOption)
        {
            case 1:
                PrintInfo(getTimeSlotMenu("Lunch"), "Lunch");
                break;
            case 2:
                PrintInfo(getTimeSlotMenu("Dinner"), "Dinner");
                break;
            case 3:
                string timeSlot = FilterFoodMenu.cursoroptionTimeSlot();
                if (timeSlot == "") { break; }
                PrintInfo(FilterFoodMenu.cursoroptionMenu(), timeSlot);
                //PrintInfo(SortFoodMenu.Dishess, SortFoodMenu.SelectedTimeSlotOption == 2 ? "Dinner" : "Lunch");
                break;
            case 4:
                exitMenu = true;
                break;
        }
        return exitMenu;
    }

    public static (string timeslot, List<Dish> timeslotMenu)? GetDefaultMenu()
    {
        var dt = SetTime();
        DateOnly date = dt.date;
        TimeOnly time = dt.time;

        if (ifDinner(time) && rawFoodMenu != null)
        {
            return ("Dinner", getTimeSlotMenu("Dinner"));

        }
        else if (rawFoodMenu != null)
        {
            return ("Lunch", getTimeSlotMenu("Lunch"));
        }

        return null;
    }

    public static bool ifDinner(TimeOnly time)
    {
        TimeOnly startTime = new TimeOnly(18, 0);
        TimeOnly endTime = new TimeOnly(22, 0);

        if (time >= startTime && time <= endTime)
        {
            return true;
        }

        return false;
    }

    public static (DateOnly date, TimeOnly time) SetTime()
    {
        DateTime now = DateTime.Now;

        DateOnly date = DateOnly.FromDateTime(now);
        TimeOnly time = TimeOnly.FromDateTime(now);

        return (date, time);

    }

    public static List<Dish> getTimeSlotMenu(string ts) => rawFoodMenu.FindAll(x => x.Timeslot == ts);

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    private static int selectedFilterMenuOption = MenuSelector.RunMenuNavigator(new List<string>() { " Filter by Ingredients", " Filter by Price", " Filter by Category", " Exit" });
    private static void DisplayFilterMenuOptions()
    {
        int selectedOption = MenuSelector.RunMenuNavigator(new List<string>() { " Filter by Ingredients", " Filter by Price", " Filter by Category", " Exit"} );
        for (int i = 1; i <= 4; i++)
        {
            if (i == selectedFilterMenuOption)
            {
                Console.Write(">");
            }
            else
            {
                Console.Write(" ");
            }

            switch (i)
            {
                case 1:
                    Console.WriteLine(" Filter by Ingredients");
                    break;
                case 2:
                    Console.WriteLine(" Filter by Price");
                    break;
                case 3:
                    Console.WriteLine(" Filter by Category");
                    break;
                case 4:
                    Console.WriteLine(" Exit and Save");
                    break;
            }
        }
    }

    private static void DisplayTimeSlotMenuOptions()
    {
        for (int i = 1; i <= 3; i++)
        {
            if (i == selectedTimeSlotOption)
            {
                Console.Write(">");
            }
            else
            {
                Console.Write(" ");
            }

            switch (i)
            {
                case 1:
                    Console.WriteLine(" Lunch");
                    break;
                case 2:
                    Console.WriteLine(" Dinner");
                    break;
                case 3:
                    Console.WriteLine(" Exit");
                    break;
            }
        }
    }

    private static List<Dish> HandleSelection()
    {
        Console.Clear();

        switch (selectedFilterMenuOption)
        {
            case 1:
                Console.WriteLine("Enter the ingredients (use comma):");
                string ingredientRaw = Console.ReadLine().ToLower();
                List<string> ingredients = ingredientRaw.Split(", ").ToList();
                return FilterIngredients(ingredients, selectedTimeSlotOption.ToString());

            case 2:
                Console.WriteLine("Enter the maximum price:");
                if (double.TryParse(Console.ReadLine(), out double maxPrice))
                {
                    Dishess.AddRange(FilterPrice(selectedTimeSlotOption.ToString(), maxPrice));
                }
                else
                {
                    Console.WriteLine("Invalid price input. Please enter a valid price.");
                }
                break;

            case 3:
                Console.WriteLine("Enter the category (1. Meat, 2. Chicken, 3. Fish, 4. Vegetarian, 5. Exit):");
                List<string> categoryList = Console.ReadLine().Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                Dishess.AddRange(FilterCategory(selectedTimeSlotOption == 2 ? true : false, categoryList));
                break;
            case 4:
                break;
        }
        return Dishess;
    }




}