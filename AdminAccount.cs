using Newtonsoft.Json;

public class AdminAccount : Account
{
    public AdminAccount(int id, string name, string email, string password) : base(id, name, email, password)
    {
        Reservations = null;
        if (LoginSystem.AdminAccounts != null)
        {
            LoginSystem.AdminAccounts.Add(this);
            Utensils.WriteJson("Admins.json", LoginSystem.AdminAccounts);
        }
        else
        {
            LoginSystem.AdminAccounts = new List<AdminAccount>();
        }
    }

    public override List<Reservation> GetReservations()
    {
        return ReservationSystem.reservations;
    }

    public List<Account>? GetAccounts()
    {
        return LoginSystem.Accounts;
    }


}