

//should be called first
public abstract class MenuItem<T>
{
    protected List<T> Items;

    public MenuItem(string jsonFilePath)
    {
        Items = JsonFileHandler.ReadFromFile<T>(jsonFilePath);
    }

    public virtual void HandleSelection() { }

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