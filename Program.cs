using System.Xml.Serialization;

class Program
{
    public static Database context;
    public static void Main()
    {
        context = new Database();
        Feedback feedback = context.Feedbacks.ToList()[0];
        Console.WriteLine(feedback.Email);
        //OptionMenu.RunMenu();

    }
}

