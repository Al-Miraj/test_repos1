public class SendFeedBackCommand : ICommand
{
    private readonly Dashboard _dashboard;

    public SendFeedBackCommand(Dashboard dashboard)
    {
        _dashboard = dashboard;
    }

    public void Execute()
    {
        _dashboard.GetOrders();
    }

    public override string ToString()
    {
        return "Send Feedback";
    }
}