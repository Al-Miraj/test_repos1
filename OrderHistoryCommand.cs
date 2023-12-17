public class OrderHistoryCommand : ICommand
{
    private readonly Dashboard _dashboard;

    public OrderHistoryCommand(Dashboard dashboard) =>
        _dashboard = dashboard;

    public void Execute()
    {
        _dashboard.OrderHistory();
    }
}