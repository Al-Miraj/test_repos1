using Newtonsoft.Json;

class FoodMenu
{
    public List<MenuItem> MenuItems { get; set; }
    public bool isDinner = true;

    public FoodMenu()
    {
        MenuItems = new List<MenuItem>();
        MenuItems = GetAppropiateMenu();
    }

    public List<MenuItem>? LoadData()
    {
        try
        {
            using StreamReader reader = new StreamReader("");
            string json = reader.ReadToEnd();
            var items = JsonConvert.DeserializeObject<List<MenuItem>>(json);
            return items;
        }
        catch (JsonReaderException)
        { return null; }
    }

    public void Display()
    {
        MenuItems.Clear();
        var allItems = LoadData();
        Console.WriteLine();
        Console.WriteLine("=========================================================");
        for (int i = 0; i < MenuItems.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {MenuItems[i].Name}                      {MenuItems[i].Price}");
            Console.WriteLine($"{MenuItems[i].Details}");
            Console.WriteLine();
        }
        Console.WriteLine("=========================================================");
    }

    public List<MenuItem> GetAppropiateMenu()
    {
        MenuItems.Clear();
        var allItems = LoadData();
        List<MenuItem> timeslotMenu = new List<MenuItem>();
        if (isDinner)
        {
            //var dinnerMenuItems = allItems.FindAll();
        }
        else
        {
            //var lunchMenuItems = allItems.FindAll();
        }

        return timeslotMenu;
    }

}
