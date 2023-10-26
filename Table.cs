using System.Text.Json.Serialization;

public class Table
{
    [JsonPropertyName("TableNumber")]
    public int TableNumber;

    [JsonPropertyName("Coordinate")]
    public (int, int) Coordinate;

    [JsonPropertyName("Capacity")]
    public int Capacity;

    [JsonPropertyName("IsReservated")]
    public bool IsReservated;

    public Table(int TableNumber, (int, int) Coordinate, int Capacity, bool IsReservated)
    {
        this.TableNumber = TableNumber;
        this.Coordinate = Coordinate;
        this.Capacity = Capacity;
        this.IsReservated = IsReservated;
    }
}