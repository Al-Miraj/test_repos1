
public class Dishes : MenuItem<Dish>
{
    private int selectedFoodMenuOption;

    public Dishes() : base("Dish.json")
    {
        selectedFoodMenuOption = MenuSelector.RunMenuNavigator(new List<string>() { "Lunch", "Dinner", "Filter Menu", "Exit" });
        HandleSelection();
        PrintInfo(GetDefaultMenu().Value.Item1, ifDinner(TimeOnly.FromDateTime(DateTime.Now)) == true ? "Dinner" : "Lunch");
    }

    public override void PrintInfo(List<Dish> dishlist, string header, bool keyContinue = true)
    {
        var lastDish = dishlist.LastOrDefault();
        int consoleWidth = Console.WindowWidth;
        int timeslotLength = header.Length;
        int startPosition = (consoleWidth / 2) - (timeslotLength / 2);
        Console.SetCursorPosition(Math.Max(startPosition, 0), 0); // Ensure the cursor position is not negative

        Console.WriteLine(header);
        Console.WriteLine(); 
        Console.WriteLine("==================================================================================================================");
        for (int i = 0; i < dishlist.Count; i++)
        {
            Console.WriteLine();
            Console.WriteLine($"{i + 1}. {dishlist[i].Name,-20} {dishlist[i].Price,74}");
            if (dishlist[i].Description.Length > 52)
            {
                Console.WriteLine($"{dishlist[i].Description.Substring(0, 50)}");
                Console.WriteLine(dishlist[i].Description.Substring(50, dishlist[i].Description.Length - 50));
            }
            else
            {
                Console.WriteLine($"Ingredients: {string.Join(", ", dishlist[i].Ingredients)}");
                Console.WriteLine();
                Thread.Sleep(100);
            }
            Console.WriteLine();
            Console.WriteLine("==================================================================================================================");

        }

        if (keyContinue)
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }

    public override void HandleSelection()
    {
        Console.Clear();

        switch (selectedFoodMenuOption)
        {
            case 1:
                PrintInfo(GetTimeSlotMenu("Lunch"), "Lunch");
                break;
            case 2:
                PrintInfo(GetTimeSlotMenu("Dinner"), "Dinner");
                break;
            case 3:
                List<Dish>? filteredDishes = HandleFilterMenuSelection();
                if (filteredDishes != null && filteredDishes.Count > 0)
                {
                    PrintInfo(filteredDishes, "Filtered Menu");
                }
                else
                {
                    Console.WriteLine("No dishes found for the selected filters.");
                }
                break;
            case 4:
                break;
        }
        return;
    }

    private (List<Dish>, string timeslot)? GetDefaultMenu()
    {
        var dt = SetTime();
        DateOnly date = dt.date;
        TimeOnly time = dt.time;

        if (ifDinner(time))
        {
            return (GetTimeSlotMenu("Dinner"), "Dinner");

        }
        else if (!ifDinner(time))
        {
            return (GetTimeSlotMenu("Lunch"), "Lunch");
        }

        return null;
    }

    private static bool ifDinner(TimeOnly time)
    {
        TimeOnly startTime = new TimeOnly(18, 0);
        TimeOnly endTime = new TimeOnly(22, 0);

        if (time >= startTime && time <= endTime)
        {
            return true;
        }

        return false;
    }

    private static (DateOnly date, TimeOnly time) SetTime()
    {
        DateTime now = DateTime.Now;

        DateOnly date = DateOnly.FromDateTime(now);
        TimeOnly time = TimeOnly.FromDateTime(now);

        return (date, time);
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

    private List<Dish>? HandleFilterMenuSelection()
    {
        switch (selectedFilterMenuOption)
        {
            case 1:
                return FilterIngredients(HandleTimeSlotSelection());
            case 2:
                Console.WriteLine("Enter the maximum price:");
                if (double.TryParse(Console.ReadLine(), out double maxPrice))
                {
                    return FilterPrice(HandleTimeSlotSelection(), maxPrice);
                }
                else
                {
                    Console.WriteLine("Invalid price input. Please enter a valid price.");
                }
                break;
            case 3:
                return FilterCategory(HandleTimeSlotSelection().ToString());
            case 4:
                break;
            default:
                break;
        }

        return null;
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

    private List<Dish>? FilterCategory(string menuType)
    {
        List<Dish> unsortedMenu = menuType == "Dinner" ? GetTimeSlotMenu("Dinner") : GetTimeSlotMenu("Lunch");
        Console.WriteLine("Enter the category (1. Meat, 2. Chicken, 3. Fish, 4. Vegetarian, 5. Exit):");
        List<string> categoryList = Console.ReadLine().Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        foreach (string category in categoryList)
        {
            switch (category)
            {
                case "1":
                    return unsortedMenu.FindAll(x => x.Category == "Meat" || x.Category == "Chicken");
                case "2":
                    return unsortedMenu.FindAll(x => x.Category == "Chicken");
                case "3":
                    return unsortedMenu.FindAll(x => x.Category == "Fish");
                case "4":
                    return unsortedMenu.FindAll(x => x.Category == "Vegetarian");
                case "5":
                    return null;
                default:
                    Console.WriteLine("Input incorrect. Please enter 1, 2, 3, 4, or 5.");
                    break;
            }
        }
        return null;
    }

    public List<Dish> FilterPrice(string menuType, double price)
    {
        Console.WriteLine(menuType);
        List<Dish> finalMenu = menuType == "Dinner" ? GetTimeSlotMenu("Dinner") : GetTimeSlotMenu("Lunch");
        return finalMenu.Where(x => x.Price <= price).OrderBy(x => x.Price).ToList();
    }

    public List<Dish> FilterIngredients(string menuType)
    {
        Console.WriteLine("Enter the ingredients (use comma):");
        string ingredientRaw = Console.ReadLine().ToLower();
        List<string> ingredients = ingredientRaw.Split(", ").ToList();

        List<Dish> unsortedDishes = menuType == "Dinner" ? GetTimeSlotMenu("Dinner") : GetTimeSlotMenu("Lunch");
        List<Dish> filteredByIngredients = unsortedDishes.Where(x => x.Ingredients.SelectMany(i => i.ToLower().Split(" ")).Any(ingredient => ingredients.Contains(ingredient))).ToList();
        List<Dish> filteredByAllergens = unsortedDishes.Where(x => x.PotentialAllergens.Select(i => i.ToLower()).Any(allergen => ingredients.Contains(allergen.ToLower()))).ToList();
        return filteredByIngredients.Union(filteredByAllergens).OrderBy(x => x.Price).ToList();
    }
}
