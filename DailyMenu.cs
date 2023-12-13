﻿public class DailyMenuGenerator
{
    public static void DisplayDailyMenu()
    {

        List<MenuItem> menuItems = JsonFileHandler.ReadFromFile<MenuItem>("Items.json");

        //foreach (var menuItem in menuItems)
        //{
        //    Console.WriteLine($"{menuItem.Name} - {menuItem.Price}");
        //}

        List<Drinks> drinks = JsonFileHandler.ReadFromFile<Drinks>("Drinks.json");

        //foreach (var drink in drinks)
        //{
        //    Console.WriteLine($"{drink.Name} - {drink.Price}");
        //}


        MenuItem randomMeal = GetRandom(menuItems);
        Drinks randomDrink = GetRandom(drinks);

        double totalOriginalPrice = randomMeal.Price + randomDrink.Price;
        double totalDiscountedPrice = (randomMeal.Price + randomDrink.Price) * 0.9;

        //Format so that the prices are with two decimals only
        string formattedTotalOriginalPrice = totalOriginalPrice.ToString("0.00");
        string formattedTotalDiscountedPrice = totalDiscountedPrice.ToString("0.00");

        // Setting the output encoding to UTF8 allows for the € to be displayed properly
        Console.OutputEncoding = System.Text.Encoding.UTF8;


        Console.WriteLine($"\nThe daily menu for today is: {randomMeal.Name} + {randomDrink.Name}");
        Console.WriteLine($"Original Price: €{randomMeal.Price} + €{randomDrink.Price} = €{formattedTotalOriginalPrice}");
        Console.WriteLine($"Discounted Price (10% off): €{formattedTotalDiscountedPrice}");




    }


    // Method that will be used to randomly select a meal and a drink from their respective lists
    public static T GetRandom<T>(List<T> items)
    {
        Random random = new Random();
        int randomIndex = random.Next(items.Count);
        return items[randomIndex];
    }
}