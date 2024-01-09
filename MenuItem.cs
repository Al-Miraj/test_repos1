public abstract class MenuItem<T>
{
    public List<T> Items;

    public MenuItem(string jsonFilePath)
    {
        Items = JsonFileHandler.ReadFromFile<T>(jsonFilePath);
    }
    public MenuItem(List<T> items)
    {
        Items = items;
    }

    public virtual string HandleSelection()
    {
        return "";
    }

    public abstract void PrintInfo(List<T> dishlist, string header, bool keyContinue = true);


    // Common methods that can be used in derived classes
    public List<T> GetTimeSlotMenu(string timeSlot)
    {
        // werkt alleen als Items van de type Dish is
        return Items
            .Select(item => item as Dish)
            .Where(dish => dish != null && dish.Timeslot == timeSlot)
            .Cast<T>()
            .ToList();
    }


}