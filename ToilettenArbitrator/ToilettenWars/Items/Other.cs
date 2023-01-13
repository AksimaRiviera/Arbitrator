using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToilettenArbitrator.ToilettenWars.Items
{
    public class Other : Item
    {
        private const int MAXIMUM_STACK_VALUE = 14;

        private int _stack;

        
        public Other() : base ()
        {
        }

        public Other(ItemCard card) : base(card)
        {

        }

        public Other(string itemID) : base(itemID)
        {

        }
    }
}
