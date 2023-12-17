
using Newtonsoft.Json;

public class DrinksMenu : MenuItem<Drinks>
{
    private static int selectedOption;
    public DrinksMenu() : base("Drinks.json")
    {
        selectedOption = MenuSelector.RunMenuNavigator(new List<string>() { "Complete menu", "Soda", "Wine", "Whiskey", "Cognac", "Beer", "Exit" });
        HandleSelection();
    }


    public override void HandleSelection()
    {
        if (selectedOption >= 0 && selectedOption <= 5)
        {
            string? category = selectedOption switch
            {
                0 => "Full",     // "Complete menu"
                1 => "Soda",     // "Soda"
                2 => "Wine",     // "Wine"
                3 => "Whiskey",  // "Whiskey"
                4 => "Cognac",   // "Cognac"
                5 => "Beer",     // "Beer"
                _ => null
            };

            if (category == "Full")
                PrintInfo(Items, "Complete Menu");
            else
                PrintInfo(GetCategory(category), category);
        }
    }

    public override void PrintInfo(List<Drinks> drinks, string header, bool KeyContinue = true)
    {
        Console.Clear();
        string CurrentCategory = "";
        int consoleWidth = Console.WindowWidth;
        int timeslotLength = header.Length;
        int startPosition = (consoleWidth / 2) - (timeslotLength / 2);
        Console.SetCursorPosition(Math.Max(startPosition, 0), 0);

        Console.WriteLine(header);
        Console.WriteLine();
        Console.WriteLine("==================================================================================================================");
        foreach (var drink in drinks)
        {
            if (CurrentCategory != drink.Category)
                CurrentCategory = drink.Category;

            Console.WriteLine();
            Console.WriteLine($"Name: {drink.Name}");
            Console.WriteLine($"Description: {drink.Description}");
            Console.WriteLine($"Price: {drink.Price}");
            Console.WriteLine($"Category: {drink.Category}");
            Console.WriteLine($"Alcohol percentage: {drink.Alcohol}%");
            Console.WriteLine();
            Console.WriteLine("==================================================================================================================");
        }

        if (KeyContinue)
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }

    public List<Drinks> GetCategory(string type) => Items.FindAll(x => x.Category == type);
}