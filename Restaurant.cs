using Newtonsoft.Json;

public static class Restaurant
{
    public static string DealsXmlFileName = "Deals.xml";
    public static string TablesJsonFileName = "Tables.json";
    public static string ReservationsXmlFileName = "Reservations.xml";
    public static string AccountsXmlFileName = "Accounts.xml";
    public static List<Account> Accounts = InitializeAccounts();
    public static List<AdminAccount> AdminAccounts = GetAdminAccounts();
    public static List<Deal> Deals = InitializeDeals();
    public static List<Table> Tables = InitializeTables();
    public static List<Reservation> Reservations = InitializeReservations();


    static public List<Account> InitializeAccounts()
    {
        List<Account> accounts;
        if (File.Exists(AccountsXmlFileName))
        {
            accounts = XmlFileHandler.ReadFromFile<Account>(AccountsXmlFileName);
        }
        else
        {
            AdminAccount admin = new AdminAccount(1, "Admin", "admin@work.com", "Admin-123");
            accounts = new List<Account>() { admin };
            XmlFileHandler.WriteToFile(accounts, AccountsXmlFileName);
        }
        return accounts;
    }

    public static List<AdminAccount> GetAdminAccounts()
    {
        List<AdminAccount> adminAccounts = new List<AdminAccount>();
        foreach (Account account in Accounts)
        {
            if (account is AdminAccount admin)
            {
                adminAccounts.Add(admin);
            }
        }
        return adminAccounts;
    }

    static public List<Deal> InitializeDeals()
    {
        List<Deal> deals;
        if (File.Exists(DealsXmlFileName))
        {
            deals = XmlFileHandler.ReadFromFile<Deal>(DealsXmlFileName);
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
            XmlFileHandler.WriteToFile(deals, DealsXmlFileName);
        }
        return deals;
    }

    static public List<Table> InitializeTables()
    {
        List<Table> tables;

        if (File.Exists(TablesJsonFileName))
        {
            tables = JsonFileHandler.ReadFromFile<Table>(TablesJsonFileName);
        }
        else
        {
            tables = new List<Table>()
            {
                new Table(1, (1, 1), 2, 7.50, false),
                new Table(2, (2, 1), 2, 7.50, false),
                new Table(3, (3, 1), 2, 7.50, false),
                new Table(4, (1, 2), 2, 7.50, false),
                new Table(5, (2, 2), 2, 7.50, false),
                new Table(6, (3, 2), 2, 7.50, false),
                new Table(7, (1, 3), 2, 7.50, false),
                new Table(8, (2, 3), 2, 7.50, false),
                new Table(9, (3, 3), 4, 10.0, false),
                new Table(10, (1, 4), 4, 10.0, false),
                new Table(11, (2, 4), 4, 10.0, false),
                new Table(12, (3, 4), 4, 10.0, false),
                new Table(13, (1, 5), 4, 10.0, false),
                new Table(14, (2, 5), 6, 15.0, false),
                new Table(15, (3, 5), 6, 15.0, false),
            };
            JsonFileHandler.WriteToFile(tables, TablesJsonFileName);
        }
        return tables;
    }

    static public List<Reservation> InitializeReservations()
    {
        List<Reservation> reservations = new List<Reservation>();

        if (File.Exists(ReservationsXmlFileName))
        {
            reservations.AddRange(XmlFileHandler.ReadFromFile<Reservation>(ReservationsXmlFileName));
        }
        else
        {
            foreach (Account account in Accounts)
            {
                if (account is CustomerAccount customerAccount)
                {
                    reservations.AddRange(customerAccount.Reservations);
                }
            }
            XmlFileHandler.WriteToFile(reservations, ReservationsXmlFileName);
        }
        return reservations;
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
    } // todo maybe have a deal handler or put this in Deal.cs

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
    } // todo maybe have a deal handler or put this in Deal.cs
}