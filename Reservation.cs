using System;
using System.Text;
using System.Text.Json;


public class Reservation
{
    public Account? CustomerAccount { get; set; } = null;
    public int ReservationNumber { get; set; }
    public int NumberOfPeople { get; set; }
    public string Date { get; set; }
    public string TimeSlot { get; set; }
    public ReservationSystem.Table SelectedTable { get; set; }

    public Reservation(int reservationNumber, int numberOfPeople, string date, string timeSlot, ReservationSystem.Table selectedTable)
    {
        ReservationNumber = reservationNumber;
        NumberOfPeople = numberOfPeople;
        Date = date;
        TimeSlot = timeSlot;
        SelectedTable = selectedTable;
        if (CustomerAccount != null)
        {
            CustomerAccount.Reservations.Add(this);
        }
    }

    public override string ToString()
    {
        return $"Reservation Number: {ReservationNumber}\nAmount of People: {NumberOfPeople}\nDate: {Date}\nTimeslot: {TimeSlot}\nTable number: {SelectedTable.TableNumber}";
    }

    
}
