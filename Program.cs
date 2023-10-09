namespace MiniProject1_V1
{
    class Program
    {
        public static Player player = new Player("player", World.Weapons[0], 50, 50, World.Locations[0]); // normal practice?

        public static void Main()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("[M] Move");
                Console.WriteLine("[Q] Quit game");
                string choice = Console.ReadLine();
                Console.WriteLine();

                if (choice == "M")
                {
                    PrintMap();

                    while (true)
                    {
                        Console.WriteLine($"Your current location is: {player.CurrentLocation.Name}. From here you can go:\n");
                        player.CurrentLocation.PrintCurrentCompass();
                        string direction = GetNextDirection();

                        if (new List<string> { "N", "E", "S", "W" }.Contains(direction) == true)
                        {
                            if (CheckDirection(direction))
                            {
                                player.MakeMove(direction);
                                Console.WriteLine($"you moved to {player.CurrentLocation.Name}.");
                                break;
                            }
                            else { Console.WriteLine($"You hear a howl deep in that direction. Beter choose a different direction. . ."); }
                        }
                        else { Console.WriteLine($"Invalid input \"{direction}\""); }
                    }
                    
                }
                else if (choice == "Q")
                { Console.WriteLine("quiting..."); break; }
                else
                { Console.WriteLine($"Invalid input: \"{choice}\""); }
            }
        }

        public static void PrintMap()
        {
            Console.WriteLine("+--------+   [H] Home (start)        [G] Guard post");
            Console.WriteLine("|   P    |   [T] Town square         [B] Bridge");
            Console.WriteLine("|   A    |   [F] Farmer              [S] Spider forest");
            Console.WriteLine("| VFTGBS |   [V] Farmer's field");
            Console.WriteLine("|   H    |   [A] Alchemist's hut");
            Console.WriteLine("+--------+   [P] Alchemist's garden");
        }

        public static string GetNextDirection()
        {
            Console.WriteLine("What direction would you like to go? (N/E/S/W)");
            string direction = Console.ReadLine();
            return direction;
        }

        public static bool CheckDirection(string direction)
        {
            Location locationInDirection;
            if (direction == "N") { locationInDirection = player.CurrentLocation.LocationToNorth; }
            else if (direction == "E") { locationInDirection = player.CurrentLocation.LocationToEast; }
            else if (direction == "S") { locationInDirection = player.CurrentLocation.LocationToSouth; }
            else { locationInDirection = player.CurrentLocation.LocationToWest; } // when direction == "W"

            if (locationInDirection == null) { return false; }
            else { return true; }
        }
    }
}