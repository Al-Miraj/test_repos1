public static class MenuSelector
{
    private static string Cursor = " >";

    public static int RunMenuNavigator<T>(List<T> options)
    {
        int selectedOption = 0;
        int x = Console.CursorLeft;
        int y = Console.CursorTop;
        string option;
        Console.CursorVisible = false;
        while (true)
        {
            Console.SetCursorPosition(x, y);
            for (int i = 0; i < options.Count; i++)
            {
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{Cursor} {options[i]}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"   {options[i]}");
                }
            }

            Console.ResetColor();

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.UpArrow && selectedOption > 0)
            {
                selectedOption--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && selectedOption < options.Count - 1)
            {
                selectedOption++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.CursorVisible = true;
                return selectedOption;
            }
            else
            {
                continue;
            }
        }
    }
}
