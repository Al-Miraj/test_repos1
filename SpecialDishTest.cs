    namespace SpecialDishTest
{
    [TestClass]
    public class SpecialDishTest
    {
        [TestMethod]
        [DataRow(2024, 12, 25, true)] // Christmas
        [DataRow(2024, 10, 31, true)] // Halloween
        [DataRow(2024, 1, 15, false)] // Not a holiday
        public void TestCheckDate(int year, int month, int day, bool expected)
        {
            DateTime testDate = new DateTime(year, month, day);
            bool result = SpecialDishes.CheckDate(testDate);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestFindNextHoliday()
        {
            DateTime testDate = new DateTime(2023, 2, 27); // One day before "Carnaval"
            string expectedHoliday = "Carnaval";
            string actualHoliday = SpecialDishes.FindNextHoliday(testDate);
            Assert.AreEqual(expectedHoliday, actualHoliday);
        }

        [TestMethod]
        [DataRow(1, "Winter")]
        [DataRow(4, "Spring")]
        [DataRow(7, "Summer")]
        [DataRow(10, "Autumn")]
        public void TestGetSeason(int month, string expectedSeason)
        {
            string season = SpecialDishes.GetSeason(month);
            Assert.AreEqual(expectedSeason, season);
        }

        [TestMethod]
        [DataRow("Winter")]
        [DataRow("Spring")]
        [DataRow("Summer")]
        [DataRow("Autumn")]
        public void TestSeasonNavigator(string season)
        {
            List<Dish> dishes = JsonFileHandler.ReadFromFile<Dish>("SeasonalDishes.json");
            SpecialDishes.rawSeasonalMenu = dishes;
            var result = SpecialDishes.SeasonNavigator(season);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.All(dish => dish.Season == season));
        }
    }
}