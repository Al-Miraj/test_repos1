using System.Reflection.Metadata.Ecma335;

public static class SortFoodMenu
{
    public static List<MenuItem> SortMenu()
    {
        List<MenuItem> sortedMenu = new List<MenuItem>();
        List<MenuItem> categories = new List<MenuItem>();
        List<string> ingredients = new List<string>();
        //List<MenuItem> rawMenu = new List<MenuItem>();

        while (true)
        {
            sortedMenu.Clear();  ingredients.Clear(); 
            Console.WriteLine("What menu do you want to sort?");
            Console.WriteLine("1. Lunch, 2. Dinner");
            string choiceMenu = Console.ReadLine();

            if (choiceMenu == "1" || choiceMenu == "2")
            {
                //rawMenu = choiceMenu == "2" ? FoodMenu.GetDinnerMenu() : FoodMenu.GetLunchMenu();

                Console.WriteLine("What do you want to sort the menu on?");
                Console.WriteLine("These are the available options:");
                Console.WriteLine("1. ingredients, 2. price, 3. category, 4. exit menu and save your changes.");
                string sortChoice = Console.ReadLine();

                switch (sortChoice)
                {
                    case "1":
                        Console.WriteLine("Enter the ingredients (use comma):");
                        string ingredientRaw = Console.ReadLine().ToLower();
                        string[] ingredientsArray = ingredientRaw.Split(", ");
                        ingredients.AddRange(ingredientsArray);
                        sortedMenu.AddRange(SortIngredients(ingredients, choiceMenu));
                        break;

                    case "2":
                        Console.WriteLine("Enter the maximum price:");
                        if (double.TryParse(Console.ReadLine(), out double maxPrice))
                        {
                            sortedMenu.AddRange(SortPrice(choiceMenu, maxPrice));
                        }
                        else
                        {
                            Console.WriteLine("Invalid price input. Please enter a valid number.");
                        }
                        break;

                    case "3":
                        sortedMenu.AddRange(SortCategory(choiceMenu == "2" ? true : false));
                        break;

                    case "4":
                        Console.WriteLine("Exiting sorting menu. Returning to the main menu.");
                        return sortedMenu;

                    default:
                        Console.WriteLine("Invalid choice. Please enter 1, 2, 3, or 4");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Please enter 1 or 2");
            }

            return sortedMenu;
            
        }
    }

    public static List<MenuItem> SortCategory(bool isDinner)
    {
        List<MenuItem> unsortedMenu = isDinner ? FoodMenu.GetDinnerMenu() : FoodMenu.GetLunchMenu();
        List<MenuItem> selectedCategories = new List<MenuItem>();

        Console.WriteLine("Categories:");
        foreach (var cat in FoodMenu.categoryEmojis)
        {
            Console.WriteLine($"{cat.Key}: {cat.Value}");
        }

        Console.WriteLine("Enter the category (1. Meat, 2. Chicken, 3. Fish, 4. Vegetarian, 5. Exit):");
        string[] categoryArray = Console.ReadLine().Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string category in categoryArray)
            {
                if (category == "1")
                {
                    selectedCategories.AddRange(unsortedMenu.FindAll(x => x.Category == "Meat"));
                }
                else if (category == "2")
                {
                    selectedCategories.AddRange(unsortedMenu.FindAll(x => x.Category == "Chicken"));
                }
                else if (category == "3")
                {
                    selectedCategories.AddRange(unsortedMenu.FindAll(x => x.Category == "Fish"));
                }
                else if (category == "4")
                {
                    selectedCategories.AddRange(unsortedMenu.FindAll(x => x.Category == "Vegetarian"));
                }
                else if (category == "5")
                {
                    return new List<MenuItem>(); // Return an empty list to indicate the user's exit choice.
                }
                else
                {
                    Console.WriteLine("Input incorrect. Please enter 1, 2, 3, 4, or 5.");
                }
            }
        return selectedCategories;
    }

    public static List<MenuItem> SortPrice(string menuType, double price)
    {
        List<MenuItem> finalMenu = new List<MenuItem>();

        finalMenu = menuType == "2" ? FoodMenu.GetDinnerMenu() : FoodMenu.GetLunchMenu();
        
        List<MenuItem> sortedMenu = finalMenu.Where(x => x.Price <= price).ToList();
        //in sortedmenu zitten alleen de prijzen als deel van het object
        return sortedMenu;
    }

    public static List<MenuItem> SortIngredients(List<string> ingredients, string menuType)
    {
        
        List<MenuItem> unsortedItems = menuType == "2" ? FoodMenu.GetDinnerMenu() : FoodMenu.GetLunchMenu();
        List<MenuItem> temp = unsortedItems.Where(x => x.Ingredients.Select(i => i.ToLower()).Any(ingredient => ingredients.Contains(ingredient))).ToList();
        return temp;
    }

}