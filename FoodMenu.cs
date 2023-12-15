using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using System.Threading;
using Menus;

public static class FoodMenu
{
    //parsed data
    //private static List<Dishes> Dishess = new();

    private static int selectedOption = 1;

    public static readonly Dictionary<string, string> categoryEmojis = new Dictionary<string, string>
    {
        { "Meat", "🥩" },
        { "Chicken", "🍗" },
        { "Fish", "🐟" },
        { "Vegetarian", "🥦" }
    };

    public static List<Dishes>? LoadFoodMenuData() => JsonFileHandler.ReadFromFile<Dishes>("Dishes.json");
    
    public static void Display()
    {
        Console.CursorVisible = false;
        PrintInfo(GetDefaultMenu().timeslotMenu, GetDefaultMenu().timeslot, true);

        while (true)
        {
            Console.Clear();
            //PrintInfo(GetDefaultMenu().timeslotMenu, GetDefaultMenu().timeslot, false);
            DisplayMenuOptions();

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
                bool exitMenu = HandleSelection();
                if (exitMenu) { break; }
            }
        }
    }

    public static void PrintInfo(List<Dishes> Dishess, string timeSlot, bool keyContinue = true)
    {
        Console.Clear();
        Console.WriteLine(timeSlot);
        Console.WriteLine(); Console.WriteLine("==================================================================================================================");
        for (int i = 0; i < Dishess.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Dishess[i].Name,-20} {Dishess[i].Price,74}");
            if (Dishess[i].Description.Length > 52)
            {
                Console.WriteLine($"{Dishess[i].Description.Substring(0, 50)}- {Dishess[i].AllergensInfo,60}");
                Console.WriteLine(Dishess[i].Description.Substring(50, Dishess[i].Description.Length - 50));
            }
            else
            {
                Console.WriteLine($"{Dishess[i].Description,-50} {Dishess[i].AllergensInfo,60}");
            }
            Console.WriteLine($"Ingredients: {string.Join(", ", Dishess[i].Ingredients)}");
            Console.WriteLine();
            Thread.Sleep(100);
        }
        Console.WriteLine("==================================================================================================================");

        if (keyContinue)
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }

    private static void DisplayMenuOptions()
    {
        for (int i = 1; i <= 4; i++)
        {
            if (i == selectedOption)
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
                    Console.WriteLine(" Sort menu by category");
                    break;
                case 4:
                    Console.WriteLine(" Exit");
                    break;
            }
        }
    }

    /*switch (i)
    {
        case 1:
            Dishess = GetLunchMenu();
            break;
        case 2:
            Dishess = GetDinnerMenu();
            break;
        case 3:
            Dishess = SortFoodMenu.SortMenu();
            //SortFoodMenu.SortMenu().ForEach(x => Console.WriteLine(x));
            break;
        case 4:
            return;

        default:
            Console.WriteLine("Invalid choice. Please enter 1, 2, 3 or 4.");
            break;
    }*/


    private static bool HandleSelection()
    {
        Console.Clear();
        bool exitMenu = false;

        switch (selectedOption)
        {
            case 1:
                PrintInfo(GetLunchMenu(), "Lunch");
                break;
            case 2:

                PrintInfo(GetDinnerMenu(), "Dinner");
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

    public static (string timeslot, List<Dishes> timeslotMenu) GetDefaultMenu()
    {
        string x = "";
        var allDishes = LoadFoodMenuData();
        List<Dishes> timeslotMenu = new List<Dishes>();

        var dt = SetTime();
        DateOnly date = dt.date;
        TimeOnly time = dt.time;

        if (ifDinner(time) && allDishes != null)
        {
            var dinnerDishess = allDishes.FindAll(x => x.Timeslot == "Dinner");
            timeslotMenu.AddRange(dinnerDishess);
            x = "Dinner Menu";

        }
        else
        {
            if (allDishes != null)
            {
                var lunchDishess = allDishes.FindAll(x => x.Timeslot == "Lunch");
                timeslotMenu.AddRange(lunchDishess);
                x = "Lunch Menu";
            }
        }

        return (x, timeslotMenu);
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

    public static List<Dishes> GetLunchMenu()
    {
        var allDishes = LoadFoodMenuData();
        List<Dishes> tempMenu = new List<Dishes>();


        var lunchDishess = allDishes.FindAll(x => x.Timeslot == "Lunch");
        tempMenu.AddRange(lunchDishess);

        return tempMenu;
    }

    public static List<Dishes> GetDinnerMenu()
    {
        var allDishes = LoadFoodMenuData();
        List<Dishes> tempMenu = new List<Dishes>();

        var lunchDishess = allDishes.FindAll(x => x.Timeslot == "Dinner");
        tempMenu.AddRange(lunchDishess);

        return tempMenu;
    }
}
