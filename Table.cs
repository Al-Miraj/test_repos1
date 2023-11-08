using System.Text.Json.Serialization;

public class Table
{
    [JsonPropertyName("TableNumber")]
    public int TableNumber;

    [JsonPropertyName("Coordinate")]
    public (int, int) Coordinate;

    [JsonPropertyName("Capacity")]
    public int Capacity;

    [JsonPropertyName("Price")]
    public double Price;

    [JsonPropertyName("IsReservated")]
    public bool IsReservated;

    public Table(int TableNumber, (int, int) Coordinate, int Capacity, double Price, bool IsReservated)
    {
        this.TableNumber = TableNumber;
        this.Coordinate = Coordinate;
        this.Capacity = Capacity;
        this.Price = Price;
        this.IsReservated = IsReservated;
    }
}