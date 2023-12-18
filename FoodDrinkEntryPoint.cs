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
    private int selectedMenuOption = MenuSelector.RunMenuNavigator(new List<string>() { " Normal Menu", " Seasonal Menu", " Holiday Menu", " Drinks", " Exit" });

    public void GetCorrectMenu()
    {
        Dishes dishes;
        SpecialDishes specialDishes;
        DrinksMenu drinks;

        switch (selectedMenuOption + 1)
        {
            case 1:
                dishes = new Dishes();
                break;
            case 2:
                specialDishes = new SpecialDishes(false);
                break;
            case 3:
                specialDishes = new SpecialDishes(true);
                break;
            case 4:
                drinks = new DrinksMenu();
                break;
            default:
                break;
        }
    }
}