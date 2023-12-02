public class SeasonalMenuItem
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string[] Ingredients { get; set; }
    public double Price { get; set; }
    public string Timeslot { get; set; }
    public string Category { get; set; }
    public string[] PotentialAllergens { get; set; }
    public string? Icon;
    public string Season;
    private static readonly string[] Allergens = new string[] { "Milk", "Eggs", "Peanuts", "Tree Nuts", "Fish", "Shellfish", "Soybeans", "Wheat", "Sesame" };

    public SeasonalMenuItem(string name, string description, List<string> ingredients, string timeslot, double price, List<string> potentialAllergens, string icon, string season)
    {
        Name = name;
        Description = description;
        Ingredients = ingredients.ToArray();
        Price = price;
        Timeslot = timeslot;
        Category = icon;
        PotentialAllergens = potentialAllergens.ToArray();
        Season = season;
        Icon = icon switch  // Easier then regular switch 
        {
            "Meat" => "🥩",
            "Chicken" => "🍗",
            "Fish" => "🐟",
            "Vegetarian" => "🥦",
            _ => "",
        };
    }
}