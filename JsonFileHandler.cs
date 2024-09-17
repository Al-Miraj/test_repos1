using Newtonsoft.Json;
/*
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
            throw new IOException($"Writing to file '{fileName}' went wrong. " + ex.Message, ex);
        }
    }

    public static List<T> ReadFromFile<T>(string fileName)
    {
        try
        {
            string json = File.ReadAllText(fileName);
            List<T> data = JsonConvert.DeserializeObject<List<T>>(json)!;
            return data;
        }
        catch (Exception ex )
        {
            if (ex is FileNotFoundException)
            {
                throw new FileNotFoundException($"File '{fileName}' does not exist. " + ex.Message, ex );
            }
            else if ( ex is JsonReaderException )
            {
                throw new IOException("Invalid JSON. " + ex.Message, ex);
            }
            else { throw new IOException("Somethingggg went wrong. " + ex.Message, ex); }
        }
    }
}
*/

// O L D   M E T H O D S
//   (Need feedback from the PO, so im saving the old versions just in case)


public static class JsonFileHandler
{
    public static void WriteToFile<T>(List<T> deals, string dealsFileName)
    {
        StreamWriter writer = new StreamWriter(dealsFileName);
        writer.Write(JsonConvert.SerializeObject(deals, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        writer.Close();
    }

    public static List<T> ReadFromFile<T>(string DealsFileName)
    {
        StreamReader reader = new StreamReader(DealsFileName);
        string jsonString = reader.ReadToEnd();
        reader.Close();
        List<T> deals = JsonConvert.DeserializeObject<List<T>>(jsonString)!;
        return deals;
    }
}
