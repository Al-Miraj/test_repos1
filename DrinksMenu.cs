using Newtonsoft.Json;

public static class DrinksMenu
{
    private static int selectedOption = 1;

    private static List<Drinks> defaultMenu = LoadDrinksData();

    public static List<Drinks>? LoadDrinksData()
    {
        try
        {
            using StreamReader reader = new StreamReader("Drinks.json");
            string json = reader.ReadToEnd();
            var items = JsonConvert.DeserializeObject<List<Drinks>>(json);
            return items;
        }
        catch (JsonReaderException)
        { return null; }
        catch (FileNotFoundException)








        { return null; }
        catch (UnauthorizedAccessException)
        { return null; }
    }

    public static void Display()
    {
        Console.CursorVisible = false;
        PrintInfo(defaultMenu);

        while (true)
        {
            Console.Clear();

            DisplayMenuOptions();

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.UpArrow && selectedOption > 1)
            {
                selectedOption--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && selectedOption < 7)
            {
                selectedOption++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                HandleSelection();
            }
        }
    }

    private static void DisplayMenuOptions()
    {
        for (int i = 1; i <= 7; i++)
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
                    Console.WriteLine(" Drink Menu");
                    break;
                case 2:
                    Console.WriteLine(" Soda");
                    break;
                case 3:
                    Console.WriteLine(" Wine");
                    break;
                case 4:
                    Console.WriteLine(" Whiskey");
                    break;
                case 5:
                    Console.WriteLine(" Cognac");
                    break;
                case 6:
                    Console.WriteLine(" Beer");
                    break;
                case 7:
                    Console.WriteLine(" Exit");
                    break;
            }
        }
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

    public static void PrintInfo(List<Drinks> drinks, bool KeyContinue = true)
    {
        Console.Clear();
        string CurrentCategory = "";

        foreach (var drink in drinks)
        {
            Console.WriteLine($"Name: {drink.Name}");
            Console.WriteLine($"Description: {drink.Description}");
            Console.WriteLine($"Price: {drink.Price}");
            Console.WriteLine($"Category: {drink.Category}");
            Console.WriteLine($"Alcohol percentage: {drink.Alcohol}%");
            Console.WriteLine();

            if (CurrentCategory != drink.Category)
            {
                Console.WriteLine("------------------------------------------------------------------------------------");
                Console.WriteLine();
                CurrentCategory = drink.Category;
            }
        }

        if (KeyContinue)
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }

    public static List<Drinks> GetCategory(string type) => defaultMenu.FindAll(x => x.Category == type);
}