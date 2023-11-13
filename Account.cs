using Newtonsoft.Json;
using System.Text;
using System;

public class Account
{
    public int Id { get; }
    public string Name { get; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<Reservation>? Reservations = new List<Reservation>(); 

    public Account(int id, string name, string email, string password)
    {
        string firstLetter = name[0].ToString();
        Id = id;
        Name = string.Concat(firstLetter.ToUpper(), name.AsSpan(1));
        Email = email;
        Password = password;
        if (LoginSystem.Accounts != null)
        {
            LoginSystem.Accounts.Add(this);
        }
        else
        {
            LoginSystem.Accounts = new List<Account> { this };
        }
    }

    public virtual List<Reservation> GetReservations() => Reservations;

    public override string ToString() => $"ID: {Id}\nEmail: {Email}\n Password: {Utensils.HashPassword(Password)}";   //Password hashed for privacy of customers

}