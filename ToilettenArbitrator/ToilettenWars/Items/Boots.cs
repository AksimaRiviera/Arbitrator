using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToilettenArbitrator.ToilettenWars.Items
{
    public class Boots : Item
    {
        public Boots()
        {
        }

        public Boots(ItemCard card) : base(card)
        {
        }

        public Boots(string itemID) : base(itemID)
        {
        }
    }
}
