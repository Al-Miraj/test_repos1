public static class SpecialDishes
{
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
}