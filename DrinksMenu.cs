using Newtonsoft.Json;

public class DrinksMenu : MenuItem
{
    private static int selectedOption;

    public Dishes() : base("Dish.json")
    {
        selectedOption = MenuSelector.RunMenuNavigator(new List<string>() { "Lunch", "Dinner", "Filter Menu", "Exit" });
        HandleSelection();
    }


    private static void HandleSelection()
    {
        switch (selectedOption)
        {
            case 1:
                PrintInfo(defaultMenu);
                break;
            case 2:
                PrintInfo(GetCategory("Soda"));
                break;
            case 3:
                PrintInfo(GetCategory("Wine"));
                break;
            case 4:
                PrintInfo(GetCategory("Whiskey"));
                break;
            case 5:
                PrintInfo(GetCategory("Cognac"));
                break;
            case 6:
                PrintInfo(GetCategory("Beer"));
                break;
            case 7:
                break;
        }
    }

    protected override void PrintInfo(List<Drinks> drinks, string header, bool KeyContinue = true)
    {
        Console.Clear();
        string CurrentCategory = "";

        foreach (var drink in drinks)
        {
            if (CurrentCategory != drink.Category)
            {
                Console.WriteLine("------------------------------------------------------------------------------------");
                Console.WriteLine();
                CurrentCategory = drink.Category;
            }

            Console.WriteLine($"Name: {drink.Name}");
            Console.WriteLine($"Description: {drink.Description}");
            Console.WriteLine($"Price: {drink.Price}");
            Console.WriteLine($"Category: {drink.Category}");
            Console.WriteLine($"Alcohol percentage: {drink.Alcohol}%");
            Console.WriteLine();
        }

        if (KeyContinue)
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }

    public static List<Drinks> GetCategory(string type) => defaultMenu.FindAll(x => x.Category == type);
}