public class OptionMenuCommand : ICommand
{
    public void Execute() =>
        OptionMenu.RunMenu();

    public override string ToString()
    {
        return "Exit to main menu";
    }
}