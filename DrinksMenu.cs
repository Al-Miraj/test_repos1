using Newtonsoft.Json;
using System.Text;
using Colorful;
using Console = Colorful.Console;
using System.Drawing;
using System.Xml.Serialization;
using System.Reflection.PortableExecutable;

public class DrinksMenu : MenuItem<Drinks>
{
    private int selectedOption;

    public int SelectedOption
    {
        get { return selectedOption; }
        set { selectedOption = value; }
    }

    public DrinksMenu() : base("Drinks.json") { }

    public DrinksMenu(List<Drinks> Items) : base(Items) { }

    public void SelectOption()
    {
        selectedOption = MenuSelector.RunMenuNavigator(new List<string>() { "Complete menu", "Soda", "Wine", "Whiskey", "Cognac", "Beer", "Exit" });
    }


    public override string HandleSelection()
    {
        string category = "";

        if (selectedOption >= 0 && selectedOption <= 5)
        {
            category = selectedOption switch
            {
                0 => "Full",     // "Complete menu"
                1 => "Soda",     // "Soda"
                2 => "Wine",     // "Wine"
                3 => "Whiskey",  // "Whiskey"
                4 => "Cognac",   // "Cognac"
                5 => "Beer",     // "Beer"
                _ => ""
            };
        }
        return category;
    }

    public void PrintCorrectMenu(string category)
    {
        if (category == "Full")
        {
            Console.Clear();
            Console.WriteLine(new string('-', 210));
            Console.WriteLine();
            PrintInfo(Items, "Soda");
            PrintInfo(Items, "Wine");
            PrintInfo(Items, "Whiskey");
            PrintInfo(Items, "Cognac");
            PrintInfo(Items, "Beer");
        }
        else if (category != null && category.Length > 0)
        {
            Console.Clear();
            //GetCategory("Soda").ForEach(Console.WriteLine);
            PrintInfo(GetCategory(category), category);
            //(categoryDrinks.Any(d => d.Alcohol > 0) ? (Action<string, List<Drinks>>)PrintAlcoholicDrinkMenu : PrintNonAlcoholicDrinkMenu)(category, categoryDrinks);
        }

        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
        Console.Clear();
        return;
        
    }

    public List<Drinks> GetCategory(string type) => Items.FindAll(x => x.Category == type);


    public override void PrintInfo(List<Drinks> drinks, string category, bool KeyContinue = true)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        bool includesAlcohol = drinks.Any(drink => drink.Alcohol > 0.0);

        Console.ForegroundColor = Color.Yellow;
        Console.Write("{0,-50}", category);
        Console.ResetColor();

        Console.ForegroundColor = Color.Green;
        Console.Write("{0,-14}", "Price");
        Console.ResetColor();

        if (includesAlcohol)
        {
            Console.ForegroundColor = Color.Orange;
            Console.Write("{0,-15}", "Alcohol");
            Console.ResetColor();
        }

        Console.ForegroundColor = Color.Pink;
        Console.Write("{0,-140}", "Description");
        Console.ResetColor();


        Console.WriteLine();
        Console.WriteLine(new string('-', 210));

        // Drink details
        foreach (var drink in drinks)
        {
            if (drink.Category.Equals(category))
            {
                Console.WriteLine();
                Console.Write("{0,-50}", drink.Name);
                Console.Write("{0,-15:N2}", "€" + drink.Price);

                if (includesAlcohol)
                {
                    Console.Write("{0,-15:P1}", drink.Alcohol / 100);
                    Console.WriteLine("{0,-140}", drink.Description);
                }
                else
                {
                    Console.Write("{0,-140}", drink.Description);
                    Console.WriteLine();
                }
            }
        }

        Console.WriteLine();
        Console.WriteLine(new string('-', 210));
        Console.WriteLine();
    }
}