public class Deal
{
    public string Name;
    public string Description;
    public double DiscountFactor;
    public Deal(string name, string description, double discountFactor)
    {
        Name = name;
        Description = description;
        DiscountFactor = discountFactor;
    }
}