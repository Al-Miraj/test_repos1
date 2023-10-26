public class Reservation
{
    public int? ReservationNumber;
    public int? NumberOfPeople;
    public DateOnly? Date;
    public string? TimeSlot;
    public List<Deal>? DealsApplied;
    public Table? SelectedTable;
    public Reservation()
    {
        ReservationNumber = null;
        NumberOfPeople = null;
        Date = null;
        TimeSlot = null;
        SelectedTable = null;
        DealsApplied = null;
    }
}