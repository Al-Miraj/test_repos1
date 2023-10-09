using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject1_V1
{
    public class Weapon
    {
        public int ID;
        public string Name;
        public int MaximumDamage;

        public Weapon(int ID, string Name, int MaximumDamage)
        {
            this.ID = ID;
            this.Name = Name;
            this.MaximumDamage = MaximumDamage;
        }
    }
}
