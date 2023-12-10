using System.Xml.Serialization;

class Program
{
    
    public static void Main()
    {
        LoginSystem.Start();
        Database db = new Database();
        db.DataReader();
    }
}

