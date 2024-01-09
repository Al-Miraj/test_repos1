namespace TestDashboardRelated
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestUserRoles()
        {
            int count = Enum.GetValues(typeof(Restaurant.UserRole)).Length;
            Assert.IsTrue(count == 3);
        }

        [TestMethod]
        public void TestOptionCount()
        {
            var reservations = new List<object>()
            {
                new Reservation(),
                new Reservation(),
                new Reservation()
            };
            List<string?> options = reservations.Select(x => x.ToString()).ToList();
            options.Add("Back");
            Assert.IsTrue(options.Count == 4);
            options.Remove(options.Last());
            Assert.IsTrue(options.Count == 3);
        }

        [TestMethod]
        public void TestRating()
        {
            int rating = 2;
            // Code from Dashboard.cs
            string message = rating > 3 ? "" : "We are sorry to hear that \nWhat could we have done better to improve your experience?";
            string expected = "We are sorry to hear that \nWhat could we have done better to improve your experience?";
            Assert.AreEqual(expected, message);
        }

        [TestMethod]
        public void TestGettingUserRole()
        {
            AdminAccount admin = new();
            SuperAdminAccount super = new();
            CustomerAccount customer = new();
            List<Account> accounts = new List<Account>() { admin, super, customer };
            var roles = new List<Restaurant.UserRole>();
            foreach (var account in accounts)
            {
                Restaurant.UserRole role = Restaurant.GetUserRole(account);
                roles.Add(role);
            }
            Assert.IsTrue(roles[0] is Restaurant.UserRole.Admin);
            Assert.IsTrue(roles[1] is Restaurant.UserRole.SuperAdmin);
            Assert.IsTrue(roles[2] is Restaurant.UserRole.Customer);
        }

        [TestMethod]
        public void TestIfReservationsEmpty()
        {
            Assert.IsFalse(ReservationManagement.ReservationsIsEmpty(new List<Reservation>() { new Reservation(), new Reservation(), new Reservation() }));
            Assert.IsTrue(ReservationManagement.ReservationsIsEmpty(new List<Reservation>()));
        }

        [TestMethod]
        public void TestCommandPattern()
        {
            AdminAccount admin = new();
            Dashboard dashboard = new(admin);
            List<ICommand> expected = new()
            {
                new ReservationManagerCommand(dashboard),
                new CustomerManagementCommand(),
                new ReadFeedBackCommand(dashboard),
                new OptionMenuCommand(),
                new LogoutCommand()
            };
            List<ICommand> actual = admin.GetCommands(dashboard);
            Assert.IsTrue(expected.Count == actual.Count);
        }

        [TestMethod]
        public void TestValidEmail()
        {
            string email1 = "manu@gmail.com";
            string email2 = string.Empty;
            string email3 = "Alberto Gustavo";
            Func<string, bool> isValid = x => LoginSystem.IsValidEmail(x);
            Assert.IsTrue(isValid(email1));
            Assert.IsFalse(isValid(email2));
            Assert.IsFalse(isValid(email3));
        }

        [TestMethod]
        public void TestValidPassword()
        {
            string correct_password = "Alberto-25";
            Assert.IsTrue(LoginSystem.IsValidPassword(correct_password));
            string password1 = "Manu2000";
            string password2 = string.Empty;
            string password3 = "albertogustavo1";
            string password4 = "AlbertoGustavo";
            string password5 = "Alberto-Gustavo";
            List<string> wrong_passwords = new() { password1, password2, password3, password4, password5 };
            foreach (string password in  wrong_passwords)
            {
                Assert.IsFalse(LoginSystem.IsValidPassword(password));
            }
        }
    }
}