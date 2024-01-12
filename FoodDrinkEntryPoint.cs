public class FoodDrinkEntryPoint
{
    private Dishes dishes;
    private SpecialDishes specialDishes;
    private DrinksMenu drinks;
    private bool shown;

    public void GetCorrectMenu(List<Drinks> customMenu = null)
    {
        bool isValidSelection = false;
        bool shouldReturnToMain = false; // Flag to control the return to main menu

        while (!isValidSelection)
        {
            int selectedMenuOption = MenuSelector.RunMenuNavigator(new List<string>() { "Normal Menu", "Seasonal Menu", "Holiday Menu", "Drinks", "Exit" });
            
            Console.Clear();

            switch (selectedMenuOption)
            {
                case 0:
                    if (!shown)
                    {
                        dishes = new Dishes(false);
                        shown = true;
                    }
                    dishes.SelectOption();
                    dishes.HandleSelection();
                    break;
                case 1:
                    specialDishes = new SpecialDishes(false);
                    break;
                case 2:
                    specialDishes = new SpecialDishes(true);
                    break;
                case 3:
                    drinks = (customMenu != null && customMenu.Any()) ? new DrinksMenu(customMenu) : new DrinksMenu();
                    drinks.SelectOption();
                    drinks.PrintCorrectMenu(drinks.HandleSelection());
                    break;
                case 4:
                    shouldReturnToMain = true; // User chooses to exit, set flag to true
                    isValidSelection = true;
                    break;
                default:
                    Console.WriteLine("Invalid selection. Please try again.");
                    break;
            }
        }

        if (shouldReturnToMain)
        {
            OptionMenu.RunMenu();
        }
    }
}