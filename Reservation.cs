public class Reservation
{
    public int? ReservationNumber;
    public int? NumberOfPeople;
    public DateOnly? Date;
    public string? TimeSlot;
    public double? Discount;
    //public List<Deal> DealsApplied;
    public int? TotalPrice;
    public Table? SelectedTable;
    public Reservation()
    {
        ReservationNumber = null;
        NumberOfPeople = null;
        Date = null;
        TimeSlot = null;
        SelectedTable = null;
        Discount = null;
        //DealsApplied = new List<Deal>();
        TotalPrice = null;
    }

    //public void AddDeal(Deal deal)
    //{
    //    if (deal != null)
    //    {
    //        if (deal is GuestAmountDiscountDeal)
    //        { DealsApplied.Add(deal);}
    //    }
    //}

    // todo: get the total price of the reservation and store it
    //public double GetTotalPrice()
    //{
    //    if (DealsApplied.Count > 0)
    //    {
    //        foreach (Deal deal in DealsApplied)
    //        {

    //        }
    //    }
    //    return TotalPrice;
    //}
}