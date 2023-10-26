using Newtonsoft.Json;

public class Restaurant
{
    public List<Deal> Deals = new List<Deal>();
    public string DealsJson = "Deals.json";
    public Restaurant()
    {
        Deals = InitializeDeals();
    }

    public List<Deal> InitializeDeals()
    {
        List<Deal> deals;


        try
        {
            deals = JsonFileHandler.ReadFromFile<Deal>(DealsJson);
        }
        catch (FileNotFoundException)
        {
            string dealName = "Party Deal";
            string dealDescription = "For groups of 10 people or more, you get a 10% discount."; //todo: discount on what tho?
            double dealDiscountFactor = 0.10; // 10% discount
            deals = new List<Deal>()
                {
                    new Deal(dealName, dealDescription, dealDiscountFactor)
                };
            try
            {
                JsonFileHandler.WriteToFile(deals, DealsJson);
            }
            catch (Exception ex) { throw new IOException(ex.Message); }
        }
        catch (Exception ex)
        {
            throw new IOException(ex.Message);
        }
        return deals;
    }

    public void DisplayDeals()
    {
        Console.Clear();

        //prints the title "DEALS"
        Console.WriteLine(" ___   ____   __    _     __ ");
        Console.WriteLine("| | \\ | |_   / /\\  | |   ( (` ");
        Console.WriteLine("|_|_/ |_|__ /_/--\\ |_|__ _)_) ");
        Console.WriteLine("\n");
        Console.WriteLine("-------------------------------\n");


        foreach (Deal deal in Deals)
        {
            Console.WriteLine(deal.Name);
            Console.WriteLine(deal.Description);
            Console.WriteLine("===========================================\n");
        }
    }
}

