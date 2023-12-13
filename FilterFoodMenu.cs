using System.Linq;
using System.Reflection.Metadata.Ecma335;
namespace Menus;

public static class FilterFoodMenu
{
    private static int selectedOption = 1;
    private static int selectedTimeSlotOption = 1;
    public static List<Dishes> Dishess = new List<Dishes>();

    //entry point
    public static List<Dishes> cursoroptionMenu()
    {
        Console.CursorVisible = false;

        while (true)
        {
            Dishess.Clear();
            Console.Clear();
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
                return HandleSelection();
            }
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
                    Console.WriteLine(" Filter by Ingredients");
                    break;
                case 2:
                    Console.WriteLine(" Filter by Price");
                    break;
                case 3:
                    Console.WriteLine(" Filter by Category");
                    break;
                case 4:
                    Console.WriteLine(" Exit and Save");
                    break;
            }
        }

    }

    private static List<Dishes> HandleSelection()
    {
        Console.Clear();
        List<string> ingredients = new List<string>();

        switch (selectedOption)
        {
            case 1:
                Console.WriteLine("Enter the ingredients (use comma):");
                string ingredientRaw = Console.ReadLine().ToLower();
                ingredients.AddRange(ingredientRaw.Split(", "));

                Dishess.AddRange(FilterIngredients(ingredients, selectedTimeSlotOption.ToString()));
                break;

            case 2:
                Console.WriteLine("Enter the maximum price:");
                if (double.TryParse(Console.ReadLine(), out double maxPrice))
                {
                    Dishess.AddRange(FilterPrice(selectedTimeSlotOption.ToString(), maxPrice));
                }
                else
                {
                    Console.WriteLine("Invalid price input. Please enter a valid price.");
                }
                break;

            case 3:
                Console.WriteLine("Enter the category (1. Meat, 2. Chicken, 3. Fish, 4. Vegetarian, 5. Exit):");
                List<string> categoryList = Console.ReadLine().Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                Dishess.AddRange(FilterCategory(selectedTimeSlotOption == 2 ? true : false, categoryList));
                break;
            case 4:
                Environment.Exit(0);
                break;
        }
        return Dishess;
    }

    public static List<Dishes> FilterCategory(bool isDinner, List<string> Categories)
    {
        List<Dishes> unsortedMenu = isDinner ? FoodMenu.GetDinnerMenu() : FoodMenu.GetLunchMenu();
        List<Dishes> selectedCategories = new List<Dishes>();

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
                    return new List<Dishes>(); // Return an empty list to indicate the user's exit choice.
                default:
                    Console.WriteLine("Input incorrect. Please enter 1, 2, 3, 4, or 5.");
                    break;
            }
        }
        return selectedCategories;
    }

    public static List<Dishes> FilterPrice(string menuType, double price)
    {
        List<Dishes> finalMenu = new List<Dishes>();
        Console.WriteLine(menuType);
        finalMenu = menuType == "2" ? FoodMenu.GetDinnerMenu() : FoodMenu.GetLunchMenu();

        List<Dishes> sortedMenu = finalMenu.Where(x => x.Price <= price).ToList();
        sortedMenu = sortedMenu.OrderBy(x => x.Price).ToList();

        //in sortedmenu zitten alleen de prijzen als deel van het object
        return (sortedMenu);
    }

    public static List<Dishes> FilterIngredients(List<string> ingredients, string menuType)
    {
        List<Dishes> unsortedDishes = menuType == "2" ? FoodMenu.GetDinnerMenu() : FoodMenu.GetLunchMenu();
        List<Dishes> filteredByIngredients = unsortedDishes.Where(x => x.Ingredients.SelectMany(i => i.ToLower().Split(" ")).Any(ingredient => ingredients.Contains(ingredient))).ToList();
        List<Dishes> filteredByAllergens = unsortedDishes.Where(x => x.PotentialAllergens.Select(i => i.ToLower()).Any(allergen => ingredients.Contains(allergen.ToLower()))).ToList();
        List<Dishes> sortedMenu = filteredByIngredients.Union(filteredByAllergens).OrderBy(x => x.Price).ToList();
        return sortedMenu;
    }

    private static void DisplayTimeSlotMenuOptions()
    {
        for (int i = 1; i <= 3; i++)
        {
            if (i == selectedTimeSlotOption)
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
                    Console.WriteLine(" Exit");
                    break;
            }
        }
    }

    public static string cursoroptionTimeSlot()
    {
        Console.CursorVisible = false;

        while (true)
        {
            Console.Clear();
            DisplayTimeSlotMenuOptions();

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.UpArrow && selectedTimeSlotOption > 1)
            {
                selectedTimeSlotOption--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && selectedTimeSlotOption < 3)
            {
                selectedTimeSlotOption++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                return HandleTimeSlotSelection();
            }
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

}