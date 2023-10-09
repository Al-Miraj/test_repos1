using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject1_V1
{
    public class Monster
    {
        public int ID;
        public string Name;
        public int MaximumDamage;
        public int CurrentHitPoints;
        public int MaximumHitPoints;
        public Monster(int ID, string Name, int MaximumDamage, int CurrentHitPoints, int MaximumHitPoints)
        {
            this.ID = ID;
            this.Name = Name;
            this.MaximumDamage = MaximumDamage;
            this.CurrentHitPoints = CurrentHitPoints;
            this.MaximumHitPoints = MaximumHitPoints;
        }
    }
}
