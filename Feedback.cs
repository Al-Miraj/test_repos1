public sealed class Feedback
{
    public string Email { get; }
    public int Rating { get; }
    public string Message { get; }

    public Feedback(string email, int rating, string message)
    {
        Email = email;
        Rating = rating;
        Message = message;
    }
}