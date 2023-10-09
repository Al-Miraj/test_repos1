using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject1_V1
{
    public class Location
    {
        public int ID;
        public string Name;
        public string Description;
        public Quest QuestAvailableHere;
        public Monster MonsterLivingHere; // depending on how many, maybe use a list
        public Location LocationToNorth;
        public Location LocationToSouth;
        public Location LocationToWest;
        public Location LocationToEast;
        public Location(int ID, string Name, string Description, Quest quest, Monster monsters)
        {
            this.ID = ID;
            this.Name = Name;
            this.Description = Description;
            QuestAvailableHere = quest;
            MonsterLivingHere = monsters;
        }

        public void PrintCurrentCompass()
        {
            if (LocationToNorth != null)
            {
                Console.Write("    N\n    |\n");
            }
            if (LocationToWest != null)
            {
                Console.Write("W---|");
            }
            else
            {
                Console.Write("    |");
            }
            if (LocationToEast != null)
            {
                Console.Write("---E");
            }
            Console.WriteLine();
            if (LocationToSouth != null)
            {
                Console.Write("    |\n    S\n");
            }
        }
    }
}
