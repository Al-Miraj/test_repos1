class MenuItem
{
    public string Name { get; set; }
    public string Details { get; set; }
    public double Price { get; set; }
    public string? Icon;

    public MenuItem(string name, string details, double price)
    {
        Name = name;
        Details = details;
        Price = price;
        GetAppropiateIcon();
    }

    public void GetAppropiateIcon()
    {
        string[] icons = Details.Split(" ");
        if (icons[0] == "Beef")
            Icon = "";
    }
}