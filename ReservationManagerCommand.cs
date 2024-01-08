public class ReservationManagerCommand : ICommand
{
    private readonly Dashboard _dashboard;

    public ReservationManagerCommand(Dashboard dashboard) =>
        _dashboard = dashboard;

    public void Execute() =>
        _dashboard.ReservationManager();

    public override string ToString()
    {
        return "Reservation Management";
    }
}