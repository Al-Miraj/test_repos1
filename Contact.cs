public class Contact : AboutUs
{
    public static void ContactInformation()
    {
        Console.WriteLine(RestaurantName);
        Console.WriteLine($"Phonenumber: {PhoneNumber}");
        Console.WriteLine($"Email: {RestaurantEmail}");
        foreach (string item in SocialMedia)
        {
            Console.WriteLine(item);
        }
    }
}