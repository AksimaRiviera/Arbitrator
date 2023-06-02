using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToilettenArbitrator.ToilettenWars.Items;

namespace ToilettenArbitrator.ToilettenWars
{
    public class Cell
    {
        private MembersDataContext MDC = new MembersDataContext();

        private int _x;
        private int _y;

        private string[] _itemsId;

        private List<Item> _items;

        private CellTypes _type;

        public CellTypes Type => _type;

        public enum CellTypes
        {
            Floor,
            Wall,
            Hole
        }

        public Cell(CellTypes type) 
        {
            _type = type;
            
        }

        private void RandomLoot()
        {
            if (_type == CellTypes.Wall || _type == CellTypes.Hole) return;

            for (int i = 0; i < MDC.ItemCards.Count(); i++)
            {
                _itemsId[i] = MDC.ItemCards.ToArray()[i].ItemId;
            }

            if (new SilverDice().Luck(0.008m)) _items.Add(new Item());

            if (new SilverDice().Luck(0.03m)) _items.Add(new Item());

            if (new SilverDice().Luck(0.06m)) _items.Add(new Item());

            if (new SilverDice().Luck(0.09m)) _items.Add(new Item());

        }
    }
}
