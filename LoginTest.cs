using System.Reflection;
using System.Security.Cryptography;

namespace AdminTest
{
    [TestClass]
    class LoginTest
    {
        [TestMethod]
       void TestUserIsNotNull()
        {
            Dashboard dashboard = SetAdmin();
            Assert.IsNotNull(OptionMenu.CurrentUser);
            Assert.IsNotNull(dashboard.CurrentUser);
            Reset();
        }

        [TestMethod]
        void TestAdminReservationData()
        {
            Dashboard dashboard = SetAdmin();
            // (int customerID, int reservationNumber, int numberOfPeople, DateOnly date, string timeSlot, Table selectedTable, double nonDiscountedPrice)
            List<Reservation> reservations = new List<Reservation>()
            {
               new(1, 1, 2, DateOnly.FromDateTime(DateTime.Now), "The Afternoon (14:00-16:00)", new Table(1, (1, 1), 2, 0, true), 0),
                new(2, 2, 3, DateOnly.FromDateTime(DateTime.Now), "The Afternoon (14:00-16:00)", new Table(2, (1, 1), 2, 0, true), 0),
                new(3, 3, 4, DateOnly.FromDateTime(DateTime.Now), "The Afternoon (14:00-16:00)", new Table(3, (1, 1), 2, 0, true), 0)
            };
            foreach (var r in reservations) 
                Restaurant.Reservations.Add(r);
            Assert.IsNotNull(Restaurant.Reservations);
            Restaurant.Reservations[0].NumberOfPeople = 1;
            Restaurant.Reservations[0].Date = DateOnly.FromDateTime(DateTime.Now);
            Restaurant.Reservations[0].TimeSlot = "Lunch (12:00-14:00)";
            Assert.AreEqual(Restaurant.Reservations[0].NumberOfPeople, 1);
            Assert.AreEqual(Restaurant.Reservations[0].Date, DateOnly.FromDateTime(DateTime.Now));
            Assert.AreEqual(Restaurant.Reservations[0].TimeSlot, "Lunch (12:00-14:00)");
            Restaurant.Reservations.RemoveAt(0);
            Assert.AreEqual(Restaurant.Reservations.Count, 2);
            Reset();
        }

        [TestMethod]
        void TestAdminCustomerData()
        {
            Dashboard dashboard = SetAdmin();
            // (int customerID, string name, string email, string password)
            List<CustomerAccount> customers = new List<CustomerAccount>()
            {
                new(1, "John", "john@mail.com", "JohnPassword_2"),
                new(2, "Jane", "jane@mail.com", "JanePassword_2"),
                new(3, "Joe", "joe@mail.com", "JoePassword_2")
            };
            foreach (var c in customers)
                Restaurant.Accounts.Add(c);
            Assert.IsNotNull(Restaurant.Accounts);
            Restaurant.Accounts[0].Name = "John Deere";
            Restaurant.Accounts[0].Email = "john@outlook.com";
            Restaurant.Accounts[0].Password = "JohnPassword_5";
            Assert.AreEqual(Restaurant.Accounts[0].Name, "John Deere");
            Assert.AreEqual(Restaurant.Accounts[0].Email, "john@outlook.com");
            Assert.AreEqual(Restaurant.Accounts[0].Password, "JohnPassword_5");
            Restaurant.Accounts.RemoveAt(0);
            Assert.AreEqual(Restaurant.Accounts.Count, 2);
            Reset();
        }

        [TestMethod]
        void TestCustomer()
        {
            string name = "John Deere";
            string email = "john@mail.com";
            string password = "JohnPassword_2";
            CustomerAccount customer = new(99, name, email, password);
            Dashboard dashboard = new(customer);
            LoginSystem.ConnectUser(customer, dashboard);
            List<Reservation> reservations = new List<Reservation>()
            {
               new(1, 1, 2, DateOnly.FromDateTime(DateTime.Now), "The Afternoon (14:00-16:00)", new Table(1, (1, 1), 2, 0, true), 0),
                new(2, 2, 3, DateOnly.FromDateTime(DateTime.Now), "The Afternoon (14:00-16:00)", new Table(2, (1, 1), 2, 0, true), 0),
                new(3, 3, 4, DateOnly.FromDateTime(DateTime.Now), "The Afternoon (14:00-16:00)", new Table(3, (1, 1), 2, 0, true), 0)
            };
            foreach (var r in reservations)
            {
                Restaurant.Reservations.Add(r);
                customer.Reservations.Add(r);
            }
            Assert.IsNotNull(Restaurant.Reservations);
            Assert.IsNotNull(customer.Reservations);
            Assert.AreEqual(Restaurant.Reservations.Count, 3);
            Assert.AreEqual(customer.Reservations.Count, 3);
            Reset();
        }

        void TestLogout()
        {

            string name = "John Deere";
            string email = "john@mail.com";
            string password = "JohnPassword_2";
            CustomerAccount customer = new(99, name, email, password);
            Dashboard dashboard = new(customer);
            LoginSystem.ConnectUser(customer, dashboard);
            dashboard.Logout();
            Assert.IsNull(OptionMenu.CurrentUser);
            Assert.IsNull(OptionMenu.UserDashboard);
            Assert.IsFalse(OptionMenu.IsUserLoggedIn);
            Reset();
        }

        void Reset()
        {
            Restaurant.Reservations.Clear();
            Restaurant.Accounts.Clear();
            OptionMenu.CurrentUser = null;
            OptionMenu.UserDashboard = null;
            OptionMenu.IsUserLoggedIn = false;
        }

        Dashboard SetAdmin()
        {
            string name = "Admin 3";
            string email = "adminmail@mail.com";
            string password = "AdminPassword_2";
            AdminAccount admin = new(99, name, email, password);
            Dashboard dashboard = new(admin);
            LoginSystem.ConnectUser(admin, dashboard);
            return dashboard;
        }

        public Dashboard SetCustomer()
        {
            string name = "John Deere";
            string email = "john@mail.com";
            string password = "JohnPassword_2";
            CustomerAccount customer = new(99, name, email, password);
            Dashboard dashboard = new(customer);
            OptionMenu.CurrentUser = customer;
            OptionMenu.UserDashboard = dashboard;
            OptionMenu.IsUserLoggedIn = true;
            return dashboard;
        }
    }
}