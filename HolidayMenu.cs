using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Menus
{
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

        private static List<DateTime> keys = new List<DateTime>();

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

        private static List<HolidayMenuItem> rawMenu = LoadFoodMenuData();
        private static List<HolidayMenuItem> finishedHolidayMenu = new List<HolidayMenuItem>();

        private static void addKeys() => keys.AddRange(holidays.Keys);

        public static bool CheckDate(DateTime date) => holidays.ContainsKey(date) ? true : false;

        public static void getHoliday(DateTime key)
        {
            if (CheckDate(key))
            {
                addKeys();
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
            Console.WriteLine();

            if (choice == 0) // 'yes' was selected
            {
                string closestHoliday = FindNextHoliday();
                Console.WriteLine(closestHoliday);
                holidayNavigator(closestHoliday);
                Console.WriteLine($"Displaying the menu of the nearest holiday: {closestHoliday}");
                Console.WriteLine();
                Thread.Sleep(250);
                PrintInfo();
                Console.WriteLine();
                Console.WriteLine();
            }

            else // 'no' was selected
            {
                Console.WriteLine("press anything to continue");
                Console.ReadKey();
            }

            Console.WriteLine();
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

        public static List<HolidayMenuItem>? LoadFoodMenuData() => JsonFileHandler.ReadFromFile<HolidayMenuItem>("HolidayItems.json");

    }
}
