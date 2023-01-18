using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToilettenArbitrator.ToilettenWars.Items
{
    public class Shield : Item
    {
        public Shield()
        {
        }

        public Shield(ItemCard card) : base(card)
        {
        }

        public Shield(string itemID) : base(itemID)
        {
        }
    }
}
