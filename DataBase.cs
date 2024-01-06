using Microsoft.EntityFrameworkCore;

public class Database : DbContext
{
    public static Database context = new Database();

    public DbSet<Feedback> Feedbacks { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsbuilder)
    {
        optionsbuilder.UseSqlite("Data Source = database.db");
    }

    public void DataWriter(Feedback feedback)
    {
        context.Feedbacks.Add(feedback);
        context.SaveChanges();
    }

    public List<Feedback> DataReader()
    {
        return context.Feedbacks.ToList();
    }
}