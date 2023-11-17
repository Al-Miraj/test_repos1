//public class Reservation
//{
//    public int? ReservationNumber;
//    public int? NumberOfPeople;
//    public DateOnly? Date;
//    public string? TimeSlot;
//    public List<Deal>? DealsApplied;
//    public Table? SelectedTable;
//    public Reservation()
//    {
//        ReservationNumber = null;
//        NumberOfPeople = null;
//        Date = null;
//        TimeSlot = null;
//        SelectedTable = null;
//        DealsApplied = null;
//    }
//}

using System;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

[XmlInclude(typeof(Account))]
[XmlInclude(typeof(Table))]
[XmlInclude(typeof(Deal))]


public class Reservation
{
    public int? CustomerAccount = 1;
    public int? ReservationNumber;
    public int NumberOfPeople;
    public DateOnly? Date;
    public string? TimeSlot;
    public Table? SelectedTable;
    public List<Deal> DealsApplied = new List<Deal>();
    public double NonDiscountedPrice = 0;
    public double Discount = 0;

    public Reservation(int? reservationNumber, int numberOfPeople, DateOnly? date, string? timeSlot, Table? selectedTable)
    {
        ReservationNumber = reservationNumber;
        NumberOfPeople = numberOfPeople;
        Date = date;
        TimeSlot = timeSlot;
        SelectedTable = selectedTable;
    }

    public Reservation()
    {
        CustomerAccount = 0;
        ReservationNumber = 0;
        NumberOfPeople = 0;
        Date = new DateOnly();
        TimeSlot = "";
        SelectedTable = new Table();
        DealsApplied = new List<Deal>();
        NonDiscountedPrice = 0;
        Discount = 0;
    }



    public override string ToString()
    {
        return $"Reservation Number: {ReservationNumber}\nAmount of People: {NumberOfPeople}\nDate: {Date}\nTimeslot: {TimeSlot}\nTable number: {SelectedTable.TableNumber}";
    }

    public double GetTotalPrice()
    {
        return NonDiscountedPrice * (1 - Discount);
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