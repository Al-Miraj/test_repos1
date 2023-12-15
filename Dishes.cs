using Menus;

public class Dishes
{
    private static int selectedFoodMenuOption = MenuSelector.RunMenuNavigator(new List<string>() { " Lunch", " Dinner", " Filter Menu", " Exit" });
    public static List<Dish> rawFoodMenu = JsonFileHandler.ReadFromFile<Dish>("Dish.json");
    public static List<Dish> finishedFoodMenu = new List<Dish> { };

    private static bool HandleFoodMenuSelection()
    {
        Console.Clear();
        bool exitMenu = false;

        switch (selectedFoodMenuOption)
        {
            case 1:
                PrintInfo(getTimeSlotMenu("Lunch"), "Lunch");
                break;
            case 2:
                PrintInfo(getTimeSlotMenu("Dinner"), "Dinner");
                break;
            case 3:
                DisplayFilterMenuOptions();
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

    private static int selectedFilterMenuOption = MenuSelector.RunMenuNavigator(new List<string>() { " Filter by Ingredients", " Filter by Price", " Filter by Category", " Exit" });
    private static int selectedTimeSlotOption = MenuSelector.RunMenuNavigator(new List<string>() { " Lunch", " Dinner", " Exit" });

    private static string HandleTimeSLotSelection()
    {
        switch (selectedTimeSlotOption)
        {
            case 1:
                return "Lunch";
            case 2:
                return "Dinner";
            case 3:
                return "Exit";
            default:
                return "";
        }
    }

    private static List<Dish>? HandleFilterMenuSelection()
    {
        Console.Clear();

        switch (selectedFilterMenuOption)
        {
            case 1:
                Console.WriteLine("Enter the ingredients (use comma):");
                string ingredientRaw = Console.ReadLine().ToLower();
                List<string> ingredients = ingredientRaw.Split(", ").ToList();
                
                return FilterIngredients(HandleTimeSlotSelection().ToString());
            case 2:
                Console.WriteLine("Enter the maximum price:");
                if (double.TryParse(Console.ReadLine(), out double maxPrice))
                {
                    return FilterPrice(HandleTimeSlotSelection().ToString(), maxPrice);
                }
                else
                {
                    Console.WriteLine("Invalid price input. Please enter a valid price.");
                }
                break;
            case 3:
                Console.WriteLine("Enter the category (1. Meat, 2. Chicken, 3. Fish, 4. Vegetarian, 5. Exit):");
                List<string> categoryList = Console.ReadLine().Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                return FilterCategory(selectedTimeSlotOption == 2 ? true : false, categoryList);
            case 4:
                break;
        }
    }

    private static string HandleTimeSlotSelection()
    {
        Console.Clear();

        switch (selectedTimeSlotOption)
        {
            case 1:
                return "Lunch";
            case 2:
                return "Dinner";
            case 3:
                Environment.Exit(0);
                break;
        }

        return "";
    }

    public static List<Dish> FilterCategory(bool isDinner, List<string> Categories)
    {
        List<Dish> unsortedMenu = isDinner ? getTimeSlotMenu("Dinner") : getTimeSlotMenu("Lunch");
        List<Dish> selectedCategories = new List<Dish>();

        Console.WriteLine("Categories:");
        foreach (var cat in FoodMenu.categoryEmojis)
        {
            Console.WriteLine($"{cat.Key}: {cat.Value}");
        }

        foreach (string category in Categories)
        {
            switch (category)
            {
                case "1":
                    selectedCategories.AddRange(unsortedMenu.FindAll(x => x.Category == "Meat" || x.Category == "Chicken"));
                    break;
                case "2":
                    selectedCategories.AddRange(unsortedMenu.FindAll(x => x.Category == "Chicken"));
                    break;
                case "3":
                    selectedCategories.AddRange(unsortedMenu.FindAll(x => x.Category == "Fish"));
                    break;
                case "4":
                    selectedCategories.AddRange(unsortedMenu.FindAll(x => x.Category == "Vegetarian"));
                    break;
                case "5":
                    return new List<Dish>(); // Return an empty list to indicate the user's exit choice.
                default:
                    Console.WriteLine("Input incorrect. Please enter 1, 2, 3, 4, or 5.");
                    break;
            }
        }
        return selectedCategories;
    }

    public static List<Dish> FilterPrice(string menuType, double price)
    {
        List<Dish> finalMenu = new List<Dish>();
        Console.WriteLine(menuType);
        finalMenu = menuType == "Dinner" ? getTimeSlotMenu("Dinner") : getTimeSlotMenu("Lunch");

        List<Dish> sortedMenu = finalMenu.Where(x => x.Price <= price).ToList();
        sortedMenu = sortedMenu.OrderBy(x => x.Price).ToList();

        return (sortedMenu);
    }

    public static List<Dish> FilterIngredients(string menuType)
    {
        Console.WriteLine("Enter the ingredients (use comma):");
        string ingredientRaw = Console.ReadLine().ToLower();
        List<string> ingredients = ingredientRaw.Split(", ").ToList();

        List<Dish> unsortedDishes = menuType == "Dinner" ? getTimeSlotMenu("Dinner") : getTimeSlotMenu("Lunch");
        List<Dish> filteredByIngredients = unsortedDishes.Where(x => x.Ingredients.SelectMany(i => i.ToLower().Split(" ")).Any(ingredient => ingredients.Contains(ingredient))).ToList();
        List<Dish> filteredByAllergens = unsortedDishes.Where(x => x.PotentialAllergens.Select(i => i.ToLower()).Any(allergen => ingredients.Contains(allergen.ToLower()))).ToList();
        return filteredByIngredients.Union(filteredByAllergens).OrderBy(x => x.Price).ToList();
    }




}