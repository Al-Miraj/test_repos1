using Newtonsoft.Json;

public class AdminAccount : Account
{
    public AdminAccount(int id, string name, string email, string password) : base(id, name, email, password)
    {
        //if (LoginSystem.AdminAccounts != null)
        //{
        //    LoginSystem.AdminAccounts.Add(this);
        //    Utensils.WriteJson("Admins.json", LoginSystem.AdminAccounts);
        //}
        //else
        //{
        //    LoginSystem.AdminAccounts = new List<AdminAccount>();
        //}
    }

    public AdminAccount() : base() { }

    public override List<Reservation> GetReservations()
    {
        return Restaurant.Reservations;
    }

    public List<Account>? GetAccounts()
    {
        return Restaurant.Accounts;
    }

    public List<string>? GetFeedback()
    {
        return null;
    }

    public override List<ICommand> GetCommands(Dashboard dashboard)
    {
        return new()
        {
            new ReservationManagerCommand(dashboard),
            new CustomerManagementCommand(),
            new DailyMenuCommand(),
            new ReadFeedBackCommand(dashboard),
            new OptionMenuCommand(),
            new LogoutCommand()
        };
    }
}


 