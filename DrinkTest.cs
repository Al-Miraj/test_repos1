
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DrinkMenuTesting
{
    // Since you're now using a namespace, your test class will be in the MyApp.Tests namespace
    [TestClass]
    public class DrinksMenuTest
    {
        public void TestGetCategory(string category, int expectedCount)
        {
            // Arrange

            List<Drinks> testdrinks = JsonFileHandler.ReadFromFile<Drinks>("Drinks.json");

            var drinksMenu = new DrinksMenu(testdrinks);
            drinksMenu.SelectedOption = 0;

            // Act
            var result = drinksMenu.GetCategory(category);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(expectedCount, result.Count);
        }

        [TestMethod]
        [DataRow(0, "Full")]    
        [DataRow(1, "Soda")]     
        [DataRow(2, "Wine")]     
        [DataRow(3, "Whiskey")]  
        [DataRow(4, "Cognac")]   
        [DataRow(5, "Beer")]     
        public void TestHandleSelection(int selectedOption, string expectedCategory)
        {
            // Arrange
            var drinksMenu = new DrinksMenu(new List<Drinks>());
            drinksMenu.SelectedOption = selectedOption;

            // Act
            var result = drinksMenu.HandleSelection();

            // Assert
            Assert.AreEqual(expectedCategory, result);
        }
    }


}
