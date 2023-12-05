public sealed class Feedback
{
    public int ID { get; set; }
    //public Reservation Reservation { get; set; }

    public int ReservationNumber { get; set;  }
    public string Email { get; set; }
    public int Rating { get; set; }
    public string Message { get; set; }
}