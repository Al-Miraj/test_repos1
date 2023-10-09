using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_1
{
    public class Player
    {
        public string Name { get; }
        public List<Quest> Quests { get; private set; }

        public Player(string name)
        {
            Name = name;
            Quests = new List<Quest>();
        }

        public void AddQuest(Quest quest)
        {
            Quests.Add(quest);
        }

        public void CompleteQuest(Quest quest)
        {
            if (Quests.Contains(quest))
            {
                quest.Complete();
                Quests.Remove(quest);
            }
        }
    }
