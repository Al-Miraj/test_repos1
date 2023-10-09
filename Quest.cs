using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_1
{
    public class Quest
    {
        public string Name { get; }
        public string Description { get; }
        public bool IsCompleted { get; private set; }

        public Quest(string name, string description)
        {
            Name = name;
            Description = description;
            IsCompleted = false; // Een nieuwe quest is standaard onvoltooid.
        }

        public void Complete()
        {
            IsCompleted = true;
        }
    }
