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

        if (File.Exists(DealsJson))
        {
            deals = ReadFromFile(DealsJson);
        }
        else
        {
            string dealName = "Party Deal";
            string dealDescription = "For groups of 10 people or more, you get a 10% discount."; //todo: discount on what tho?
            double dealDiscountFactor = 0.10; // 10% discount
            deals = new List<Deal>()
            {
                new Deal(dealName, dealDescription, dealDiscountFactor)
            };
            WriteToFile(deals, DealsJson);
        }
        return deals;
    }
    public void WriteToFile(List<Deal> deals, string dealsFileName) //todo change name
    {
        StreamWriter writer = new StreamWriter(dealsFileName);
        writer.Write(JsonConvert.SerializeObject(deals, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        writer.Close();
    }

    public List<Deal> ReadFromFile(string DealsFileName) 
    {
        StreamReader reader = new StreamReader(DealsFileName);
        string jsonString = reader.ReadToEnd();
        reader.Close();
        List<Deal> deals = JsonConvert.DeserializeObject<List<Deal>>(jsonString)!;
        return deals;
    }
}