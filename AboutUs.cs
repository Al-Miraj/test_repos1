public class AboutUs
{
    public static string Location = "Wijnhaven 107, 3011 WN, Rotterdam, Zuid-Holland.";
    public static string RestaurantName = "";
    public static string OpeningHours = "11 am - 10 pm.";
    public static string RestaurantEmail = "";
    public static string PhoneNumber = "1234567890";
    public static List<string> SocialMedia = new List<string>() { "Restaurant: " };

    public static void Info()
    {
        Console.WriteLine(RestaurantName);
        Console.WriteLine($"Location: {Location}");
        Console.WriteLine($"Openinghours: {OpeningHours}");
    }

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

/*"-add option “info” to the interface to see the information
- add the address, contact information and opening/closing hours.
-add email, phone number and maybe social media to contact information.

"1. The restaurant's contact information, including phone number and email address, should be prominently displayed on the official website or mobile application's
""Contact Us"" or ""About Us"" page.
2. The restaurant's physical address, including street name and number, city, state, and postal code, should be clearly visible on the website or mobile application.
3. The contact information and address should be easily accessible from the main navigation menu or footer of the website or mobile application,
ensuring that customers can quickly find this information without unnecessary navigation or searching.
"
"*/