using Newtonsoft.Json;

public static class Restaurant
{
    public static int MaxTableCapacity;
    public static List<Deal> Deals;
    public static string DealsJsonFileName;

    static Restaurant()
    {
        MaxTableCapacity = 10;
        DealsJsonFileName = "Deals.xml";
        Deals = InitializeDeals();

    }


    static public List<Deal> InitializeDeals()
    {
        List<Deal> deals;
        if (File.Exists(DealsJsonFileName))
        {
            deals = XmlFileHandler.ReadFromFile<Deal>(DealsJsonFileName);
        }
        else
        {
            string dealName = "Party Deal";
            string dealDescription = "For groups of 6 people or more, you get a 10% discount."; //todo: discount on what tho? // change 6 to 10
            double dealDiscountFactor = 0.10; // 10% discount
            int minAmountGuests = 6; //todo change 6 back to 10
            deals = new List<Deal>()
            {
                new PartyDeal(dealName, dealDescription, dealDiscountFactor, minAmountGuests)
            };
            XmlFileHandler.WriteToFile(deals, DealsJsonFileName);
        }
        return deals;
    }

    public static Deal? GetDealByName(string dealName)
    {
        foreach (Deal deal in Deals)
        {
            if (deal.Name == dealName)
            {
                return deal;
            }
        }
        return null;
    }

    public static void DisplayDeals()
    {
        Console.Clear();

        //prints the title "DEALS"
        Console.WriteLine(" ___   ____   __    _     __ ");
        Console.WriteLine("| | \\ | |_   / /\\  | |   ( (` ");
        Console.WriteLine("|_|_/ |_|__ /_/--\\ |_|__ _)_) ");
        Console.WriteLine("-------------------------------\n");
        Console.WriteLine("Deals that we are currently offering!\n");

        if (Deals.Count <= 0) 
        { 
            Console.WriteLine("We are currently offering 0 Deals. Come back later or contact us for more information!"); 
            return;  
        }
        else
        {
            foreach (Deal deal in Deals)
            {
                Console.WriteLine(deal.Name);
                Console.WriteLine(deal.Description);
                Console.WriteLine("===========================================\n");
            }
        }
    }
}