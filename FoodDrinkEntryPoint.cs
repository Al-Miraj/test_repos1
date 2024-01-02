/*public class FoodMenuNavigator
{
    int selectedMenuOption = MenuSelector.RunMenuNavigator(new List<string>() { " Normal Menu", " Seasonal Menu", " Holiday Menu", " Drinks", " Exit" });

    public void GetCorrectMenu()
    {
        Dishes dishes;
        SpecialDishes specialDishes;
        DrinksMenu drinks;

        selectedMenuOption switch
        {
            1 => dishes = new Dishes(),
            2 => specialDishes = new SpecialDishes(false),
            3 => specialDishes = new SpecialDishes(true),
            4 => drinks = new DrinksMenu(),
            _ => return,
        };
    }
}*/

public class FoodDrinkEntryPoint
{
    private Dishes dishes;
    private SpecialDishes specialDishes;
    private DrinksMenu drinks;

    public void GetCorrectMenu()
    {
        int selectedMenuOption = MenuSelector.RunMenuNavigator(new List<string>() { "Normal Menu", "Seasonal Menu", "Holiday Menu", "Drinks", "Exit" });

        switch (selectedMenuOption)
        {
            case 0:
                dishes = new Dishes();
                break;
            case 1:
                specialDishes = new SpecialDishes(false);
                break;
            case 2:
                specialDishes = new SpecialDishes(true);
                break;
            case 3:
                drinks = new DrinksMenu();
                break;
            default:
                Console.WriteLine("Invalid selection. Please try again.");
                GetCorrectMenu();
                break;
        }
    }
}