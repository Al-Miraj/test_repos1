using Menus;

//should be called first
public static class FoodMenuNavigator
{
    private static int selectedOption = 1;

    public static void Display()
    {
        Console.CursorVisible = false;

        while (true)
        {
            Console.Clear();
            //PrintInfo(GetDefaultMenu().timeslotMenu, GetDefaultMenu().timeslot, false);
            GetCorrectMenu();

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.UpArrow && selectedOption > 1)
            {
                selectedOption--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && selectedOption < 5)
            {
                selectedOption++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                bool exitMenu = HandleSelection();
                if (exitMenu) { break; }
            }
        }
    }

    public static void GetCorrectMenu()
    {
        for (int i = 1; i <= 5; i++)
        {
            if (i == selectedOption)
            {
                Console.Write(">");
            }
            else
            {
                Console.Write(" ");
            }

            switch (i)
            {
                case 1:
                    Console.WriteLine(" default food menu");
                    break;
                case 2:
                    Console.WriteLine(" seasonal food menu");
                    break;
                case 3:
                    Console.WriteLine(" special holiday menu (may not always be applicable)");
                    break;
                case 4:
                    Console.WriteLine(" default menu for drinks");
                    break;
                case 5:
                    Console.WriteLine(" exit menu");
                    break;
            }
        }
    }

    private static bool HandleSelection()
    {
        Console.Clear();
        bool exitMenu = false;

        switch (selectedOption)
        {
            case 1:
                FoodMenu.Display();
                break;
            case 2:
                SeasonalMenu.PrintInfo();
                break;
            case 3:
                HolidayMenu.getHoliday(DateTime.Now.Date);
                break;
            case 4:
                DrinksMenu.Display();
                break;
            case 5:
                return true;
        }
        return exitMenu;
    }
}