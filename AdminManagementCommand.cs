public class AdminManagementCommand : ICommand
{
    public void Execute()
    {
        AdminManagement.Display();
    }

    public override string ToString()
    {
        return "Admin Management";
    }
}