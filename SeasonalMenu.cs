using Newtonsoft.Json;
using Menus;

public static class SeasonalMenu
{
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

    //entry point
    public static void PrintInfo(bool keyContinue = true)
    {
        string currentHoliday = "";

        foreach (var dish in MainNavigator())
        {
            Console.WriteLine($"Name: {dish.Name}");
            Console.WriteLine($"Description: {dish.Description}");
            Console.WriteLine($"Ingredients: {string.Join(", ", dish.Ingredients)}");
            Console.WriteLine($"Timeslot: {dish.Timeslot}");
            Console.WriteLine($"Price: {dish.Price}");
            Console.WriteLine($"Potential Allergens: {string.Join(", ", dish.PotentialAllergens)}");
            Console.WriteLine($"Icon: {dish.Icon}");
            Console.WriteLine($"Season: {dish.Season}");
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

    public static List<SeasonalDishes>? MainNavigator()
    {
        DateTime currentDateTime = DateTime.Today;
        int month = currentDateTime.Month;
        string? season = GetSeason(month);
        if (season != null) { var menu = SeasonNavigator(season); return menu; }
        return null;

    }

    public static List<SeasonalDishes>? SeasonNavigator(string season)
    {
        switch (season)
        {
            case "Winter":
                return GetSeasonalDishess("Winter");

            case "Spring":
                return GetSeasonalDishess("Spring");

            case "Summer":
                return GetSeasonalDishess("Summer");

            case "Autumn":
                return GetSeasonalDishess("Autumn");

            default:
                return null;
        }
    }

    public static List<SeasonalDishes> GetSeasonalDishess(string season)
    {
        List<SeasonalDishes> seasonalDishessRaw = LoadFoodMenuData();
        List<SeasonalDishes> seasonMenu = seasonalDishessRaw.FindAll(x => x.Season == season).OrderBy(x => x.Price).ToList();
        return seasonMenu;

    }

    public static List<SeasonalDishes>? LoadFoodMenuData() => JsonFileHandler.ReadFromFile<SeasonalDishes>("SeasonMenu.json");





}