﻿public class FoodDrinkEntryPoint
{
    private Dishes dishes;
    private SpecialDishes specialDishes;
    private DrinksMenu drinks;

    public void GetCorrectMenu(List<Drinks> customMenu = null)
    {
        bool isValidSelection = false;
        while (!isValidSelection)
        {
            int selectedMenuOption = MenuSelector.RunMenuNavigator(new List<string>() { "Normal Menu", "Seasonal Menu", "Holiday Menu", "Drinks", "Exit" });

            switch (selectedMenuOption)
            {
                case 0:
                    dishes = new Dishes();
                    isValidSelection = true;
                    break;
                case 1:
                    specialDishes = new SpecialDishes(false);
                    isValidSelection = true;
                    break;
                case 2:
                    specialDishes = new SpecialDishes(true);
                    isValidSelection = true;
                    break;
                case 3:
                    drinks = (customMenu != null && customMenu.Any()) ? new DrinksMenu(customMenu) : new DrinksMenu();
                    isValidSelection = true;
                    break;
                case 4:
                    return;
                default:
                    Console.WriteLine("Invalid selection. Please try again.");
                    break;
            }
        }
    }
}