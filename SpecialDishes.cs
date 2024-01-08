
using System.Drawing;
using Colorful;
using Console = Colorful.Console;

public class SpecialDishes : MenuItem<Dish>
{
    private static List<Dish> rawHolidayMenu;
    private List<Dish> finishedHolidayMenu = new List<Dish>();
    private static List<DateTime> keys = new List<DateTime>();
    public static List<Dish> rawSeasonalMenu;

    public SpecialDishes(bool isHolidayMenu) : base(isHolidayMenu ? "HolidayDishes.json" : "SeasonalDishes.json")
    {
        if (isHolidayMenu)
        {
            rawHolidayMenu = Items;
            keys.AddRange(holidays.Keys);
            getHoliday(DateTime.Now.Date);
        }
        else
        {
            rawSeasonalMenu = Items;
            PrintInfo(MainNavigator(), GetSeason(DateTime.Today.Month));
        }
    }

    private static Dictionary<DateTime, string> holidays = new Dictionary<DateTime, string>
    {
        // International holidays
        { new DateTime(DateTime.Now.Year, 1, 1), "New Year" },
        { new DateTime(DateTime.Now.Year, 2, 28), "Carnaval" },
        { new DateTime(DateTime.Now.Year, 4, 27), "Koningsdag" },
        { new DateTime(DateTime.Now.Year, 12, 5), "Sinterklaas" },
        { new DateTime(DateTime.Now.Year, 12, 25), "Christmas" },
        { new DateTime(DateTime.Now.Year, 12, 26), "Christmas" },
        { new DateTime(DateTime.Now.Year, 12, 27), "Christmas" },
        { new DateTime(DateTime.Now.Year, 12, 28), "Christmas" },
        { new DateTime(DateTime.Now.Year, 12, 29), "Christmas" },
        { new DateTime(DateTime.Now.Year, 12, 30), "Christmas" },
        { new DateTime(DateTime.Now.Year, 12, 31), "Christmas" },
        { new DateTime(DateTime.Now.Year, 10, 31), "Halloween" },
    };

    public override void PrintInfo(List<Dish> dishlist, string header, bool keyContinue = true)
    {
        Console.Clear();
        Console.ForegroundColor = Color.Cyan;
        Console.WriteLine(header + " Menu", Console.WindowWidth / 2);
        Console.WriteLine();
        Console.ResetColor();

        // Set up column headers
        Console.ForegroundColor = Color.Yellow;
        Console.Write("{0,-50} ", "Dish");
        Console.ResetColor();

        Console.ForegroundColor = Color.Green;
        Console.Write("{0,-15}", "Price");
        Console.ResetColor();

        Console.ForegroundColor = Color.Orange;
        Console.WriteLine("{0,-140} ", "Description");
        Console.ResetColor();

        Console.WriteLine(new string('-', Console.WindowWidth));

        foreach (var dish in dishlist)
        {
            Console.WriteLine();
            Console.Write("{0,-50} ", dish.Name);

            Console.ForegroundColor = Color.LightGreen;
            Console.Write("{0,-15:N2}", dish.Price);
            Console.ResetColor();

            Console.WriteLine("{0,-140} ", dish.Description);

            Console.WriteLine();
        }

        Console.WriteLine(new string('-', Console.WindowWidth));

        if (keyContinue)
        {
            Console.ForegroundColor = Color.Magenta;
            Console.WriteLine("\nPress any key to continue...");
            Console.ResetColor();
            Console.ReadKey(true);
            Console.Clear();
        }
    }

    public static bool CheckDate(DateTime date) => holidays.ContainsKey(date) ? true : false;

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

    //entrypoint
    private void getHoliday(DateTime key)
    {
        if (CheckDate(key))
        {
            finishedHolidayMenu.Clear();
            Console.WriteLine($"today is {holidays[key]}");
            finishedHolidayMenu.AddRange(getHolidayMenu(holidays[key]));
            PrintInfo(finishedHolidayMenu, holidays[key]);
            return;
        }

        GetNearestMenu();
    }

    private void GetNearestMenu()
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(" No holiday found for today.");
        Console.WriteLine(" Do you want to see the menu of the nearest holiday?");
        Console.WriteLine();
        int choice = MenuSelector.RunMenuNavigator(new List<string>() { " yes", " no" });
        Console.Clear();
        Console.WriteLine();

        if (choice == 0) // 'yes' 
        {
            string closestHoliday = FindNextHoliday();
            Console.WriteLine($"Displaying the menu of the nearest holiday: {closestHoliday}");
            Console.WriteLine();
            Thread.Sleep(250);
            PrintInfo(getHolidayMenu(closestHoliday), closestHoliday);
            Console.WriteLine();
            Console.WriteLine();
        }

        Console.WriteLine("Here are the holidays as reference: ");
        Console.WriteLine();
        DisplayAllHolidays();
        Console.WriteLine();
        return;
    }
    public static List<Dish> getHolidayMenu(string holiday) => rawHolidayMenu.FindAll(x => x.Holiday == holiday);

    public static string FindNextHoliday()
    {
        DateTime date = DateTime.Now.Date;
        DateTime holidayDate = keys.Where(x => x >= date).First();

        if (holidayDate != null && holidays.ContainsKey(holidayDate))
        {
            return holidays[holidayDate];
        }

        return "";
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------------------
    public static string? GetSeason(int month)
    {
        return month switch
        {
            12 or 1 or 2 => "Winter",
            3 or 4 or 5 => "Spring",
            6 or 7 or 8 => "Summer",
            9 or 10 or 11 => "Autumn",
            _ => null,
        };
    }

    public static List<Dish>? MainNavigator()
    {
        int month = DateTime.Today.Month;
        string? season = GetSeason(month);
        if (season != null) { var menu = SeasonNavigator(season); return menu; }
        return null;

    }

    public static List<Dish>? SeasonNavigator(string season) => rawSeasonalMenu.Where(dish => dish.Season == season).OrderBy(dish => dish.Price).ToList();
}
