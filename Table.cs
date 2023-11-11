using System.Text.Json.Serialization;

public class Table
{
    public int Number { get; set; }
    public int Seats { get; set; }
    public bool IsSelected { get; set; }

    [JsonPropertyName("TableNumber")]
    public int TableNumber;

    [JsonPropertyName("Coordinate")]
    public (int, int) Coordinate;

    [JsonPropertyName("Capacity")]
    public int Capacity;

    [JsonPropertyName("IsReservated")]
    public bool IsReservated;

    [JsonPropertyName("TablePrice")]
    public double TablePrice;

    public Table(int TableNumber, (int, int) Coordinate, int Capacity, double tablePrice, bool IsReservated)
    {
        this.TableNumber = TableNumber;
        this.Coordinate = Coordinate;
        this.Capacity = Capacity;
        this.TablePrice = tablePrice;
        this.IsReservated = IsReservated;
    }
}
