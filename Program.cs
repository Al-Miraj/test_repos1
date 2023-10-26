class Program
{
    public static void Main()
    {
        Restaurant r = new Restaurant();
        foreach (Deal d in r.Deals)
        {
            Console.WriteLine(d.Name);
            Console.WriteLine(d.Description);
            Console.WriteLine(d.DiscountFactor);
        }

        //Menu x = new();
        //x.RunMenu();
    }
}