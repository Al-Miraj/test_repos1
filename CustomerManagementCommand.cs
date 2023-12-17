public class CustomerManagementCommand : ICommand
{
    public void Execute() =>
        CustomerManagement.Display();

    public override string ToString()
    {
        return "Customer Management";
    }
}