using Microsoft.EntityFrameworkCore;

public class Database : DbContext
{
    public DbSet<Feedback> Feedbacks { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsbuilder)
    {
        optionsbuilder.UseSqlite("Data Source = database.db");
    }

}