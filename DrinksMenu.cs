
using Newtonsoft.Json;
using System.Text;
using Colorful;
using Console = Colorful.Console;
using System.Drawing;
using System.Xml.Serialization;
using System.Reflection.PortableExecutable;

public class DrinksMenu : MenuItem<Drinks>
{
    private static int selectedOption;
    public DrinksMenu() : base("Drinks.json")
    {
        Console.Clear();
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
            {
                Console.Clear();
                PrintInfo(Items, "Soda");
                PrintInfo(Items, "Wine");
                PrintInfo(Items, "Whiskey");
                PrintInfo(Items, "Cognac");
                PrintInfo(Items, "Beer");

                /*PrintNonAlcoholicDrinkMenu("Soda", Items);
                PrintAlcoholicDrinkMenu("Wine", Items);
                PrintAlcoholicDrinkMenu("Whiskey", Items);
                PrintAlcoholicDrinkMenu("Cognac", Items);
                PrintAlcoholicDrinkMenu("Beer", Items);*/
            }
            else if (category != null)
            {
                Console.Clear();
                //GetCategory("Soda").ForEach(Console.WriteLine);
                PrintInfo(GetCategory(category), category);
                //(categoryDrinks.Any(d => d.Alcohol > 0) ? (Action<string, List<Drinks>>)PrintAlcoholicDrinkMenu : PrintNonAlcoholicDrinkMenu)(category, categoryDrinks);
            }

            else
            {
                return;
            }
        }
    }

    public List<Drinks> GetCategory(string type) => Items.FindAll(x => x.Category == type);


    public override void PrintInfo(List<Drinks> drinks, string category, bool KeyContinue = true)
    {
        /*int consoleWidth = Console.WindowWidth;
        int timeslotLength = category.Length;
        int startPosition = (consoleWidth / 2) - (timeslotLength / 2);

        Console.SetCursorPosition(Math.Max(0, startPosition), Console.CursorTop);
        Console.WriteLine(category.ToUpper(), Color.BlueViolet);
        Console.WriteLine(new string('=', category.Length));*/

        // Determine if the category includes alcoholic drinks
        bool includesAlcohol = drinks.Any(drink => drink.Alcohol > 0.0);

        // Print headers
        Console.ForegroundColor = Color.Yellow;
        Console.Write("{0,-50} ", category);
        Console.ResetColor();

        Console.ForegroundColor = Color.Red;
        Console.Write("{0,-140} ", "Description");
        Console.ResetColor();

        Console.ForegroundColor = Color.Green;
        Console.Write("{0,-15}", "Price");
        Console.ResetColor();

        if (includesAlcohol)
        {
            Console.ForegroundColor = Color.Orange;
            Console.WriteLine("{0,-15}", "Alcohol");
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine();
        }

        int lineLength = includesAlcohol ? 220 : 205;
        Console.WriteLine(new string('-', lineLength));

        foreach (var drink in drinks)
        {
            if (drink.Category.Equals(category))
            {
                Console.Write("{0,-50} {1,-140} {2,-15:N2}", drink.Name, drink.Description, drink.Price);

                if (includesAlcohol)
                {
                    Console.WriteLine("{0,-15:P1}", drink.Alcohol / 100);
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }

        Console.WriteLine();
        Console.WriteLine();
    }
    
    /*public void PrintNonAlcoholicDrinkMenu(string category, List<Drinks> drinks)
    {
        Console.WriteLine(category.ToUpper());
        Console.WriteLine(new string('=', category.Length));

        Console.ForegroundColor = Color.Yellow;
        Console.Write("{0,-20} ", "Name");

        // Print "Description" in Dark Red
        Console.ForegroundColor = Color.Red;
        Console.Write("{0,-50} ", "Description");

        // Print "Price" in Dark Green and then reset color
        Console.ForegroundColor = Color.Green;
        Console.WriteLine("{0,-5}", "Price");
        Console.ResetColor();

        Console.WriteLine(new string('-', 80));
        foreach (var drink in drinks)
        {
            if (drink.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("{0,-20} {1,-50} {2,-5:N2}", drink.Name, drink.Description, drink.Price);
            }
        }

        Console.WriteLine();
        Console.WriteLine();
    }

    public void PrintAlcoholicDrinkMenu(string category, List<Drinks> drinks)
    {
        Console.WriteLine(category.ToUpper());
        Console.WriteLine(new string('=', category.Length));

        Console.ForegroundColor = Color.Yellow;
        Console.Write("{0,-50} ", "Name");

        // Print "Description" in Dark Red
        Console.ForegroundColor = Color.Red;
        Console.Write("{0,-140} ", "Description");

        // Print "Price" in Dark Green and then reset color
        Console.ForegroundColor = Color.Green;
        Console.Write("{0,-15}", "Price");
        Console.ResetColor();

        Console.ForegroundColor = Color.Orange;
        Console.WriteLine("{0,-15}", "Alcohol");
        Console.ResetColor();

        Console.WriteLine(new string('-', 220));
        foreach (var drink in drinks)
        {
            if (drink.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("{0,-50} {1,-140} ${2,-15:N2} {3,-15:P1}", drink.Name, drink.Description, drink.Price, drink.Alcohol / 100);
            }
        }

        Console.WriteLine();
        Console.WriteLine();
    }*/
}