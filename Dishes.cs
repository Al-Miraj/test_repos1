
using System.Drawing;
using Colorful;
using Console = Colorful.Console;

public class Dishes : MenuItem<Dish>
{
    private int selectedFoodMenuOption;
    private static bool shown;

    public Dishes() : base("Dish.json")
    {
        if (!shown)
            PrintInfo(GetDefaultMenu().Value.Item1, ifDinner(TimeOnly.FromDateTime(DateTime.Now)) ? "Dinner" : "Lunch");
            shown = true;
            Console.Clear();

        selectedFoodMenuOption = MenuSelector.RunMenuNavigator(new List<string>() { "Lunch", "Dinner", "Filter Menu", "Exit" });

        HandleSelection();
    }


    //header is de categorie (meat, chicken etc)
    public void PrintDishesByCategory(List<Dish> dishlist, string header, string category, bool keyContinue = true)
    {
        // Filter dishes by category or any other logic specific to Dishes
        var filteredDishes = dishlist.Where(dish => dish.Category == category).ToList();

        // Now call the base PrintInfo with the filtered list
        PrintInfo(filteredDishes, header, keyContinue);
    }

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

        Console.ForegroundColor = Color.Magenta;
        Console.WriteLine("\nPress any key to continue...");
        Console.ResetColor();
        Console.ReadKey(true);
        Console.Clear();
       
    }

    public override void HandleSelection()
    {
        Console.Clear();
        switch (selectedFoodMenuOption)
        {
            case 0:
                PrintInfo(GetTimeSlotMenu("Lunch"), "Lunch");
                break;
            case 1:
                PrintInfo(GetTimeSlotMenu("Dinner"), "Dinner");
                break;
            case 2:
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
            case 3:
                return;
        }
        return;
    }

    private (List<Dish>, string timeslot)? GetDefaultMenu()
    {
        var dt = SetTime();
        TimeOnly time = dt.time;

        Console.Clear();

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
    private List<Dish>? HandleFilterMenuSelection()
    {
        int selectedFilterMenuOption = MenuSelector.RunMenuNavigator(new List<string>() { " Filter by Ingredients", " Filter by Price", " Filter by Category", " Exit" });

        switch (selectedFilterMenuOption)
        {
            case 0:
                return FilterIngredients(HandleTimeSlotSelection());
            case 1:
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
            case 2:
                return FilterCategory(HandleTimeSlotSelection().ToString());
            case 3:
                break;
            default:
                break;
        }

        return null;
    }

    private string HandleTimeSlotSelection()
    {
        Console.Clear();

        int selectedTimeSlotOption = MenuSelector.RunMenuNavigator(new List<string>() { " Lunch", " Dinner", " Exit" });

        switch (selectedTimeSlotOption)
        {
            case 1:
                return "Lunch";
            case 2:
                return "Dinner";
            case 3:
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
        Console.WriteLine();
        Console.WriteLine("Enter the ingredients (use comma):");
        string ingredientRaw = Console.ReadLine().ToLower();
        List<string> ingredients = ingredientRaw.Split(", ").ToList();

        List<Dish> unsortedDishes = menuType == "Dinner" ? GetTimeSlotMenu("Dinner") : GetTimeSlotMenu("Lunch");
        List<Dish> filteredByIngredients = unsortedDishes.Where(x => x.Ingredients.SelectMany(i => i.ToLower().Split(" ")).Any(ingredient => ingredients.Contains(ingredient))).ToList();
        List<Dish> filteredByAllergens = unsortedDishes.Where(x => x.PotentialAllergens.Select(i => i.ToLower()).Any(allergen => ingredients.Contains(allergen.ToLower()))).ToList();
        return filteredByIngredients.Union(filteredByAllergens).OrderBy(x => x.Price).ToList();
    }
}
