public sealed class Feedback
{
    public Reservation Reservation { get; }
    public string Email { get; }
    public int Rating { get; }
    public string Message { get; }

    public Feedback(string email, int rating, string message, Reservation reservation)
    {
        Email = email;
        Rating = rating;
        Message = message;
        Reservation = reservation;
    }
}