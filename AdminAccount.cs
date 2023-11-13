public class AdminAccount : Account
{
    public AdminAccount(int id, string email, string password) : base(id, email, password) { }

    public override List<Reservation> GetReservations()
    {
        return ReservationSystem.Reservations;
    }

    public List<Account>? GetAccounts()
    {
        return LoginSystem.Accounts;
    }


}