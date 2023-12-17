using Menus;

//should be called first
public abstract class MenuItem<T> : IMenu
{
    protected List<T> Items;

    public MenuItem(string jsonFilePath)
    {
        Items = JsonFileHandler.ReadFromFile<T>(jsonFilePath);
    }

    protected abstract void PrintInfo(List<T> dishlist, string header, bool keyContinue = true);
    

    // Common methods that can be used in derived classes
    protected List<Dish> GetTimeSlotMenu(string timeSlot) => Items.Where(dish => dish.Timeslot == timeSlot).ToList();


}