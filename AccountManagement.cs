public static class AccountManagement
{
    public static void RemoveAccount(Account account) => LoginSystem.Accounts.Remove(account);

}