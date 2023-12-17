
using Newtonsoft.Json;

public class DrinksMenu : MenuItem<Drinks>
{
    private static int selectedOption;
    public DrinksMenu() : base("Dish.json")
    {
        selectedOption = MenuSelector.RunMenuNavigator(new List<string>() { "Complete menu", "Soda", "Wine", "Whiskey", "Cognac", "Beer", "Exit" });
        HandleSelection();
    }


    public override void HandleSelection()
    {
        if (selectedOption >= 1 && selectedOption <= 6)
        {
            string? category = selectedOption switch
            {
                1 => "Soda",
                2 => "Soda",
                3 => "Wine",
                4 => "Whiskey",
                5 => "Cognac",
                6 => "Beer",
                _ => null
            };

            PrintInfo(GetCategory(category), category);
        }
    }

    public override void PrintInfo(List<Drinks> drinks, string header, bool KeyContinue = true)
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

    public List<Drinks> GetCategory(string type) => Items.FindAll(x => x.Category == type);
}