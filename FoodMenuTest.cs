using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class FoodMenuTest
{
    public static void testTimeBasedFunctionalities()
    {
        try
        {
            List<TimeOnly> times = new List<TimeOnly>()
            {
                new TimeOnly(10, 30), // false
                new TimeOnly(18, 50), // true
                new TimeOnly(1, 20), // false
                new TimeOnly(13, 59), // false
                new TimeOnly(21, 2), // true
            };

            foreach (TimeOnly time in times)
            {
                bool isDinnerTime = FoodMenu.ifDinner(time);

                if (isDinnerTime)
                    Console.WriteLine($"{time} is dinner time ({isDinnerTime})");

                else
                    Console.WriteLine($"{time} is not dinner time ({isDinnerTime})");
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public static void testLoadFoodMenuData()
    {
        string testFilePath = "test_items.json";
        string testJsonContent = "[{\"Name\":\"Burger\",\"Price\":10,\"Description\":\"Tasty burger\",\"AllergensInfo\":\"None\",\"Ingredients\":[\"Bun\",\"Patty\"]}]";
        File.WriteAllText(testFilePath, testJsonContent);
    }
}