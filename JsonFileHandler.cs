using Newtonsoft.Json;

public static class JsonFileHandler
{
    public static void WriteToFile<T>(List<T> data, string fileName)
    {
        try
        {
            string json = JsonConvert.SerializeObject(data, new JsonSerializerSettings { Formatting = Formatting.Indented });
            File.WriteAllText(fileName, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to JSON file: {ex.Message}");
        }
    }

    public static List<T>? ReadFromFile<T>(string fileName)
    {
        if (!File.Exists(fileName))
        {
            Console.WriteLine($"File '{fileName}' does not exist.");
            return null;
        }
        
        try
        {
            string json = File.ReadAllText(fileName);
            List<T> data = JsonConvert.DeserializeObject<List<T>>(json)!;
            return data;
        }
        catch (Exception ex )
        {
            if ( ex is JsonReaderException )
            {
                Console.WriteLine("Invalid JSON. ", ex.Message );
            }
            else { Console.WriteLine("Something went wrong. ", ex.Message); }
        }
        return null;
    }
}