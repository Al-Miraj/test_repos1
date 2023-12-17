using Newtonsoft.Json;
using System.Security.Principal;


public sealed class SuperAdminAccount : AdminAccount // inherits form Admin
{
    public SuperAdminAccount(int id, string name, string email, string password) : base(id, name, email, password)
    {
    }

    public SuperAdminAccount() : base() { }

    public override List<ICommand> GetCommands(Dashboard dashboard)
    {
        return new()
        {
            new ReservationManagerCommand(dashboard),
            new CustomerManagementCommand(),
            new AdminManagementCommand(),
            new ReadFeedBackCommand(dashboard),
            new OptionMenuCommand(),
            new LogoutCommand()
        };
    }
    
}

