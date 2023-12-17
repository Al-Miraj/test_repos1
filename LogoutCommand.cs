public class LogoutCommand : ICommand
{
    public void Execute() =>
        LoginSystem.Logout();

    public override string ToString()
    {
        return "Log out";
    }
}