﻿using Newtonsoft.Json;

public static class HolidayMenu
{
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
        };

    public static void DisplayAllHolidays()
    {
        bool christmasDisplayed = false;

        foreach (var holiday in HolidayMenu.holidays)
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

    private static List<DateTime> keys = new List<DateTime>();

    private static List<HolidayMenuItem> rawMenu = LoadFoodMenuData();
    private static List<HolidayMenuItem> finishedHolidayMenu = new List<HolidayMenuItem>();

    private static void addKeys() => keys.AddRange(holidays.Keys);

    public static bool CheckDate(DateTime date) => holidays.ContainsKey(date) ? true : false;

    public static void getHoliday(DateTime Date)
    {
        addKeys();
        bool foundHoliday = false;

        foreach (DateTime key in holidays.Keys)
        {
            if (key == Date)
            {
                foundHoliday = true;
                Console.WriteLine($"today is {holidays[key]}");
                holidayNavigator(holidays[key]);
                PrintInfo();
                return;
            }
        }

        if (!foundHoliday)
        {
            Console.WriteLine("No holiday found for today.");
        }
    }

    public static void holidayNavigator(string holiday)
    {
        //switch case voor elke holiday om later tijdens het debuggen problemen makkelijker op te lossen
        //dit kan later zonder switch case door direct de holiday mee te geven aab getHolidayMenu
        switch (holiday)
        {
            case "New Year":
                Console.WriteLine("New Year");
                getHolidayMenu("New Year");
                break;
            case "Christmas":
                Console.WriteLine("Christmas");
                getHolidayMenu("Christmas");
                break;
            case "Halloween":
                Console.WriteLine("Halloween");
                getHolidayMenu("Halloween");
                break;
            case "Koningsdag":
                Console.WriteLine("Koningsdag");
                getHolidayMenu("Koningsdag");
                break;
            //case "Bevrijdingsdag":
            // Console.WriteLine("Bevrijdingsdag");
            //getHolidayMenu("Bevrijdingsdag");
            //break;
            case "Sinterklaas":
                Console.WriteLine("Sinterklaas");
                getHolidayMenu("Sinterklaas");
                break;
            case "Carnaval":
                Console.WriteLine("Carnaval");
                getHolidayMenu("Carnaval");
                break;
            default:
                Console.WriteLine("Holiday not found.");
                break;
        }
    }

    public static void getHolidayMenu(string holiday)
    {
        List<HolidayMenuItem> holidayMenu = new List<HolidayMenuItem>();
        holidayMenu = rawMenu.FindAll(x => x.Holiday == holiday);
        finishedHolidayMenu.AddRange(holidayMenu);
    }

    public static void PrintInfo(bool keyContinue = true)
    {
        string currentHoliday = "";

        foreach (var dish in finishedHolidayMenu)
        {
            Console.WriteLine($"Name: {dish.Name}");
            Console.WriteLine($"Description: {dish.Description}");
            Console.WriteLine($"Ingredients: {string.Join(", ", dish.Ingredients)}");
            Console.WriteLine($"Timeslot: {dish.Timeslot}");
            Console.WriteLine($"Price: {dish.Price}");
            Console.WriteLine($"Potential Allergens: {string.Join(", ", dish.PotentialAllergens)}");
            Console.WriteLine($"Icon: {dish.Icon}");
            Console.WriteLine($"Holiday: {dish.Holiday}");
            Console.WriteLine();
            if (currentHoliday != dish.Name)
            {
                Console.WriteLine("------------------------------------------------------------------------------------");
                Console.WriteLine();
                currentHoliday = dish.Name;
            }
        }
        if (keyContinue)
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }

    public static List<HolidayMenuItem>? LoadFoodMenuData()
    {
        try
        {
            using StreamReader reader = new StreamReader("HolidayItems.json");
            string json = reader.ReadToEnd();
            var items = JsonConvert.DeserializeObject<List<HolidayMenuItem>>(json);
            return items;
        }
        catch (JsonReaderException)
        { return null; }
        catch (FileNotFoundException)
        { return null; }
        catch (UnauthorizedAccessException)
        { return null; }
    }
}