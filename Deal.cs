public class Deal
{
    public string Name;
    public string Description;
    public double DiscountFactor;
    //public bool CurrentlyRunning; will be used by the admin later
    //public DateOnly FromDate;
    //public DateOnly ToDate;
    public Deal(string name, string description, double discountFactor)
    {
        Name = name;
        Description = description;
        DiscountFactor = discountFactor;
    }
}