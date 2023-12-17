using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Dashboard
{
    public Account CurrentUser { get; set; }
    private static int selectedOption;

    public Dashboard(Account account)
    {
        CurrentUser = account;
    }

    public void RunDashboardMenu()
    {
        UserRole userRole = GetUserRole();
        Console.Clear();
        Console.WriteLine($"Welcome {CurrentUser.Name}!");
        Console.WriteLine("This is your dashboard.");

        List<ICommand> commands = GetCommandsOfType(userRole);
        int selectedOption = MenuSelector.RunMenuNavigator(commands);
        commands[selectedOption].Execute();

        Console.WriteLine("\n[Press any key to return to the your dashboard.]");
        Console.ReadKey();
        RunDashboardMenu();
    }

    public void GetOrders()
    {
        List<Reservation> reservations = ((CustomerAccount)CurrentUser).GetReservations();
        List<string> options = reservations.Select(GetReservationInfo).ToList();
        options.Add("Exit");
        if (reservations != null) { selectedOption = MenuSelector.RunMenuNavigator(options); }
        else { return; }
        Console.Clear();
        options.Remove(options.Last());
        Reservation reservation = reservations[selectedOption];
        SendFeedback(reservation);
    }
    private void SendFeedback(Reservation reservation)
    {
        Console.WriteLine("How would you rate our service?");
        int rating = MenuSelector.RunMenuNavigator(new List<int>() { 1, 2, 3, 4, 5 });
        rating++;
        Console.WriteLine($"You rated our service {rating} out of 5.");
        if (rating < 3)
        {
            Console.WriteLine("We are sorry to hear that.");
        }
        Console.WriteLine("What could we have done better to improve your experience? Or what went good?");
        string message = Console.ReadLine()!;
        Console.WriteLine("Thank you for the feedback!");
        //Feedback feedback = new Feedback(((CustomerAccount)CurrentUser).Email, rating, message, reservation);
        // todo: write to xml file
        Feedback feedback = new Feedback()
        {
            Email = ((CustomerAccount)CurrentUser).Email,
            Rating = rating,
            Message = message,
            ReservationNumber = reservation.ReservationNumber,
        };

        Restaurant.database.DataWriter(feedback);
    }

    public void ReservationManager()
    {
        ReservationManagement.CurrentUser = CurrentUser;
        ReservationManagement.Display();
    }

    public void OrderHistory()
    {
        Console.Clear();
        Console.WriteLine("Your past reservations:\n");
        List<Reservation> reservations = ((CustomerAccount)CurrentUser).GetReservations();  // todo check if works: WORKS
        if (reservations.Count == 0)
        {
            Console.WriteLine("You have not booked at this restaurant yet.");
            return;
        }
        reservations.ForEach(Console.WriteLine);
    }

    private string GetReservationInfo(Reservation reservation)
    {
        return
        $" Reservation Number: {reservation.ReservationNumber}" +
        $" \n    Date: {reservation.Date}" +
        $" \n    Timeslot: {reservation.TimeSlot}" +
        $" \n    Price: {reservation.GetTotalPrice()}" +
        $"\n";

    }

    public void ReadFeedback() // made some private functions public for using Command Pattern
    {
        Console.WriteLine("Customer Feedback: \n");
        List<Feedback> feedbacks = Restaurant.database.DataReader();
        foreach (Feedback feedback in feedbacks)
        {
            DisplayFeedback(feedback);
        }
        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
    }

    private void DisplayFeedback(Feedback feedback)
    {
        Console.WriteLine(feedback.Email + "                  " + feedback.Rating + " out of 5");
        Console.WriteLine(feedback.Message);
        Console.WriteLine();
    }

    private enum UserRole // better alternative for booleans
    {
        Customer,
        Admin,
        SuperAdmin
    }

    private UserRole GetUserRole() // sets the appropiate UserRole
    {
        switch (CurrentUser)
        {
            case Account n when n is SuperAdminAccount:
                return UserRole.SuperAdmin;
            case Account n when n is AdminAccount:
                return UserRole.Admin;
            default:
                return UserRole.Customer;
        }
    }

    private List<ICommand> GetCommandsOfType(UserRole userRole) // more files, but more readability using c# Command Pattern
    {
        if (userRole == UserRole.SuperAdmin)
        {
            return ((SuperAdminAccount)CurrentUser).GetCommands(this);
        }
        else if (userRole == UserRole.Admin)
        {
            return ((AdminAccount)CurrentUser).GetCommands(this);
        }
        else
        {
            return ((CustomerAccount)CurrentUser).GetCommands(this);
        }
    }
}