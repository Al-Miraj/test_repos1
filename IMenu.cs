public interface IMenu
{
    void HandleFoodMenuSelection();
    (List<Dish>, string timeslot)? GetDefaultMenu();
    List<Dish>? HandleFilterMenuSelection();
    void PrintInfo(List<Dish> dishList, string header, bool keyContinue = true);
}