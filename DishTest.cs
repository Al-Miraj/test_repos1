using System.Linq;

namespace DishMenuTests
{
    [TestClass]
    public class DishTest
    {
        [TestMethod]
        public void TestGetDefaultMenu()
        {
            List<Dish> dishes = JsonFileHandler.ReadFromFile<Dish>("Dish.json");
            var dishesMenu = new Dishes(dishes);

            var defaultMenu = dishesMenu.GetDefaultMenu();
            Assert.IsNotNull(defaultMenu);

            if (Dishes.ifDinner(TimeOnly.FromDateTime(DateTime.Now)))
            {
                Assert.AreEqual("Dinner", defaultMenu.Value.timeslot);
                Assert.IsTrue(defaultMenu.Value.Item1.TrueForAll(dish => dish.Timeslot == "Dinner"));
            }
            else
            {
                Assert.AreEqual("Lunch", defaultMenu.Value.timeslot);
                Assert.IsTrue(defaultMenu.Value.Item1.TrueForAll(dish => dish.Timeslot == "Lunch"));
            }
        }

        [TestMethod]
        [DataRow("1", 9)] // Assuming "1" corresponds to "Meat" or "Chicken"
        [DataRow("2", 6)] // Assuming "2" corresponds to "Chicken"
        [DataRow("3", 6)] // and so on...
        [DataRow("4", 1)]
        [DataRow("5", 0)] // Assuming "5" has no dishes
        public void TestFilterCategory(string categoryCode, int expectedCount)
        {
            List<Dish> dishes = JsonFileHandler.ReadFromFile<Dish>("Dish.json");
            var dishesMenu = new Dishes(dishes);
            var categoryList = new List<string> { categoryCode };
            var expectedCategories = ConvertCategoryCodeToNames(categoryCode);

            // Act
            var filteredDishes = dishesMenu.FilterCategory("Dinner", categoryList);

            // Assert for category numbers greater than 4
            if (int.TryParse(categoryCode, out int numb) && numb > 4)
            {
                Assert.IsTrue(filteredDishes == null);
            }
            else
            {
                Assert.AreEqual(expectedCount, filteredDishes.Count);
                if (expectedCount > 0)
                {
                    Assert.IsTrue(filteredDishes.All(dish => expectedCategories.Contains(dish.Category)));
                }
            }
        }

        private List<string> ConvertCategoryCodeToNames(string categoryCode)
        {
            switch (categoryCode)
            {
                case "1":
                    return new List<string> { "Meat", "Chicken" };
                case "2":
                    return new List<string> { "Chicken" };
                case "3":
                    return new List<string> { "Fish" };
                case "4":
                    return new List<string> { "Vegetarian" };
                case "5":
                    return new List<string>(); // No categories
                default:
                    throw new ArgumentException("Invalid category code");
            }
        }

        [TestMethod]
        [DataRow("Dinner", 25.00, 18)]
        [DataRow("Dinner", 15.00, 13)]
        [DataRow("Lunch", 12.00, 17)]
        [DataRow("Lunch", 8.00, 9)]
        public void TestFilterPrice(string menuType, double priceLimit, int expectedCount)
        {
            List<Dish> dishes = JsonFileHandler.ReadFromFile<Dish>("Dish.json");
            var dishesMenu = new Dishes(dishes);

            var filteredDishes = dishesMenu.FilterPrice(menuType, priceLimit);

            Assert.AreEqual(expectedCount, filteredDishes.Count);
            Assert.IsTrue(filteredDishes.All(dish => dish.Price <= priceLimit));
            Assert.IsTrue(filteredDishes.SequenceEqual(filteredDishes.OrderBy(dish => dish.Price)));
        }

        [TestMethod]
        [DataRow("Dinner", "Chicken, Broccoli", 8)]
        [DataRow("Dinner", "Tomato", 3)]
        [DataRow("Lunch", "Cheese, Tuna", 6)]
        [DataRow("Lunch", "Chicken, Broccoli, Tuna", 3)]
        public void TestFilterIngredients(string menuType, string inputIngredients, int expectedCount)
        {
            List<Dish> dishes = JsonFileHandler.ReadFromFile<Dish>("Dish.json");
            var dishesMenu = new Dishes(dishes);

            List<string> ingredients = inputIngredients.Split(", ").Select(i => i.ToLower()).ToList();

            var filteredDishes = dishesMenu.FilterIngredients(menuType, ingredients);

            Assert.AreEqual(expectedCount, filteredDishes.Count);
            //Assert.IsTrue(filteredDishes.All(dish => dish.Ingredients.Any(ingredient => ingredients.Contains(ingredient.ToLower()))));
            Assert.IsTrue(filteredDishes.SequenceEqual(filteredDishes.OrderBy(dish => dish.Price)));
        }

        [TestMethod]
        public void TestIfDinner()
        {
            // Arrange
            var time = new TimeOnly(19, 30);
            var result = Dishes.ifDinner(time);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestSetTime()
        {
            // Arrange & Act
            var (date, time) = Dishes.SetTime();

            // Assert
            var now = DateTime.Now;
            Assert.AreEqual(DateOnly.FromDateTime(now), date);
            Assert.AreEqual(TimeOnly.FromDateTime(now).Hour, time.Hour);
            Assert.AreEqual(TimeOnly.FromDateTime(now).Minute, time.Minute);
        }
    }
}
