public class ReadFeedBackCommand : ICommand
{
    private readonly Dashboard _dashboard;

    public ReadFeedBackCommand(Dashboard dashboard) =>
        _dashboard = dashboard;

    public void Execute()
    {
        _dashboard.ReadFeedback();
    }

    public override string ToString()
    {
        return "Read Feedback";
    }
}