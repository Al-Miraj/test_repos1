namespace TestRestaurantApplication
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestGetNumberOfPeople()
        {
            // Arrange
            // Set up the test scenario. In this case, you simulate user input by providing the string "4\n" as if the user entered "4" followed by the Enter key
            // Ik geef hier een aantal personen aan

            string userInput = "4\n";

            // Act
            //. Er wordt een StringReader gemaakt met.    wordt aangeroepen met de gesimuleerde gebruikersinvoer, en het resultaat wordt opgeslagen in de variabele 
            // als argument omdat we willen testen wat er gebeurt wanneer de gebruiker geen beheerder is.
            // We willen controleren of het systeem correct reageert op een aantal groter dan 6 door de juiste foutmelding weer te geven, en dat de lus niet wordt verlaten in deze situatie.
            using (System.IO.StringReader sr = new System.IO.StringReader(userInput))
            {
                Console.SetIn(sr);
                int result = ReservationSystem.GetNumberOfPeople(false);

                // Assert
                Assert.AreEqual(4, result);
            }
        }

        [TestMethod]
        public void TestGetReservationDate()
        {
            // Arrange
            // Simulate user input by providing a string
            string userInput = "20-01-2024\n";

            // Act
            //bevat geen informatie over uren, minuten, seconden of milliseconden. Het is puur bedoeld voor datumberekeningen.
            using (System.IO.StringReader sr = new System.IO.StringReader(userInput))
            {
                Console.SetIn(sr);
                DateOnly result = ReservationSystem.GetReservationDate();

                // Assert
                Assert.AreEqual(new DateOnly(2024, 01, 20              ), result);
            }
        }

        [TestMethod]
        public void TestRunMenu_UserSelectsReservationOption()
        {
            // Arrange
            string userInput = "0\n";  // Simulate user selecting the Reservation option

            // Act
            using (System.IO.StringReader sr = new System.IO.StringReader(userInput))
            {
                Console.SetIn(sr);
                using (StringWriter sw = new StringWriter())
                {
                    Console.SetOut(sw);
                    OptionMenu.RunMenu();
                    string consoleOutput = sw.ToString();

                    // Assert
                    // You can assert that the expected output (e.g., "ReservationSystem.RunSystem()") is present in the console output.
                    // You may also want to check if the menu is displayed again, depending on your specific expectations.
                    Assert.IsTrue(consoleOutput.Contains("ReservationSystem.RunSystem()"));
                }
            }
        }

        [TestMethod]
        public void TestReservationToString()
        {
            // Arrange
            Table table = new Table(1, (1, 1), 4, 10.0, false);
            DateOnly date = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
            Reservation reservation = new Reservation(1, 12345, 4, date, "12:00 - 2:00", table, 40.0);

            // Act
            string result = reservation.ToString();

            // Assert
            Assert.IsTrue(result.Contains("Customer ID: 1"));
            Assert.IsTrue(result.Contains("Reservation Number: 12345"));
            Assert.IsTrue(result.Contains("Amount of People: 4"));
            Assert.IsTrue(result.Contains($"Date: {date}"));
            Assert.IsTrue(result.Contains("Timeslot: 12:00 - 2:00"));
            Assert.IsTrue(result.Contains($"Table number: {table.TableNumber}"));
            Assert.IsTrue(result.Contains("Price: 40"));
        }

        [TestMethod]
        public void TestGetTotalPrice()
        {
            // Arrange
            Table table = new Table(1, (1, 1), 4, 10.0, false);
            DateOnly date = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
            Reservation reservation = new Reservation(1, 12345, 4, date, "12:00 - 2:00", table, 40.0);
            reservation.DiscountFactor = 0.1; // 10% discount

            // Act
            double totalPrice = reservation.GetTotalPrice();

            // Assert
            Assert.AreEqual(36.0, totalPrice); // 40 * (1 - 0.1) = 36
        }

        [TestMethod]
        public void TestDealsAppliedListInitialization()
        {
            // Arrange
            Reservation reservation = new Reservation();

            // Assert
            Assert.IsNotNull(reservation.DealsApplied);
            Assert.AreEqual(0, reservation.DealsApplied.Count);
        }


    }
}
