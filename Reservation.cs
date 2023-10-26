public class Reservation
{
    public int? ReservationNumber;
    public int? NumberOfPeople;
    public DateOnly? Date;
    public string? TimeSlot;
    public Table SelectedTable { get; set; }
    public Reservation()
    {
        ReservationNumber = null;
        NumberOfPeople = null;
        Date = null;
        TimeSlot = null;
        SelectedTable = null;
    }
}