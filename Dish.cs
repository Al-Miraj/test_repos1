using System;
using System.Collections.Generic;

public class Dish
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string[] Ingredients { get; set; }
    public string Timeslot { get; set; }
    public double Price { get; set; }
    public string[] PotentialAllergens { get; set; }
    public string Category { get; set; }
    public string Season { get; set; }
    public string Holiday { get; set; }

    public Dish(string name, string description, List<string> ingredients, string timeslot, double price, List<string> potentialAllergens, string category)
    {
        Name = name;
        Description = description;
        Ingredients = ingredients.ToArray();
        Price = price;
        Timeslot = timeslot;
        PotentialAllergens = potentialAllergens.ToArray();
        Category = category;
    }

    public Dish(string name, string description, List<string> ingredients, string timeslot, double price, List<string> potentialAllergens, string category, string season, string holiday)
        : this(name, description, ingredients, timeslot, price, potentialAllergens, category)
    {
        Season = season;
        Holiday = holiday;
    }
}