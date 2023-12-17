public class DailyMenuCommand : ICommand
{
    public void Execute() =>
        DailyMenuGenerator.DisplayDailyMenu();

    public override string ToString()
    {
        return "Daily Menu";
    }
}
