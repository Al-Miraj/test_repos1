﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using System.Threading;

namespace Menus
{
    public static class FoodMenu
    {
        //parsed data
        //private static List<MenuItem> MenuItems = new();

        private static int selectedOption = 1;

        public static readonly Dictionary<string, string> categoryEmojis = new Dictionary<string, string>
    {
        { "Meat", "🥩" },
        { "Chicken", "🍗" },
        { "Fish", "🐟" },
        { "Vegetarian", "🥦" }
    };

        //raw data
        public static List<MenuItem>? LoadFoodMenuData() => JsonFileHandler.ReadFromFile<MenuItem>("Items.json");
        
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
                    HandleSelection();
                    return;
                }
            }
        }

        //entry point
        public static void PrintInfo(List<MenuItem> MenuItems, string timeSlot, bool keyContinue = true)
        {
            Console.Clear();
            Console.WriteLine(timeSlot);
            if (MenuItems.Count < 1) { Console.WriteLine("no menu items found"); }
            Console.WriteLine(); Console.WriteLine("==================================================================================================================");
            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {MenuItems[i].Name,-20} {MenuItems[i].Price,74}");
                if (MenuItems[i].Description.Length > 52)
                {
                    Console.WriteLine($"{MenuItems[i].Description.Substring(0, 50)}- {MenuItems[i].AllergensInfo,60}");
                    Console.WriteLine(MenuItems[i].Description.Substring(50, MenuItems[i].Description.Length - 50));
                }
                else
                {
                    Console.WriteLine($"{MenuItems[i].Description,-50} {MenuItems[i].AllergensInfo,60}");
                }
                Console.WriteLine($"Ingredients: {string.Join(", ", MenuItems[i].Ingredients)}");
                Console.WriteLine();
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

        private static void HandleSelection()
        {
            Console.Clear();

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
                    PrintInfo(FilterFoodMenu.cursoroptionMenu(), timeSlot);
                    //PrintInfo(SortFoodMenu.menuItems, SortFoodMenu.SelectedTimeSlotOption == 2 ? "Dinner" : "Lunch");
                    break;
                case 4:
                    return;
            }
        }

        public static (string timeslot, List<MenuItem> timeslotMenu) GetDefaultMenu()
        {
            string x = "";
            var allItems = LoadFoodMenuData();
            List<MenuItem> timeslotMenu = new List<MenuItem>();

            var dt = SetTime();
            DateOnly date = dt.date;
            TimeOnly time = dt.time;

            if (ifDinner(time) && allItems != null)
            {
                var dinnerMenuItems = allItems.FindAll(x => x.Timeslot == "Dinner").OrderBy(x => x.Price);
                timeslotMenu.AddRange(dinnerMenuItems);
                x = "Dinner Menu";

            }
            else
            {
                if (allItems != null)
                {
                    var lunchMenuItems = allItems.FindAll(x => x.Timeslot == "Lunch").OrderBy(x => x.Price);
                    timeslotMenu.AddRange(lunchMenuItems);
                    x = "Lunch Menu";
                }
            }

            //timeslotMenu.OrderBy(x => x.Price);
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

        public static List<MenuItem> GetLunchMenu()
        {
            var allItems = LoadFoodMenuData();
            List<MenuItem> tempMenu = new List<MenuItem>();


            var lunchMenuItems = allItems.FindAll(x => x.Timeslot == "Lunch").OrderBy(x => x.Price);
            tempMenu.AddRange(lunchMenuItems);

            return tempMenu;
        }


        public static List<MenuItem> GetDinnerMenu()
        {
            var allItems = LoadFoodMenuData();
            List<MenuItem> tempMenu = new List<MenuItem>();

            var lunchMenuItems = allItems.FindAll(x => x.Timeslot == "Dinner").OrderBy(x => x.Price);
            tempMenu.AddRange(lunchMenuItems);

            return tempMenu;
        }
    }
}