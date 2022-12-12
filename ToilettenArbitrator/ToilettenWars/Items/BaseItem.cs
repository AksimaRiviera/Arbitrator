﻿namespace ToilettenArbitrator.ToilettenWars.Items
{
    public abstract class BaseItem
    {
        private ItemCard _itemCard;
        private List<ItemCard> _itemCards;

        protected long _ID;
        protected string _itemID;
        protected string _Name;
        protected string _Description;
        protected long _Coast;
        protected string[] _Options;

        public string ItemID => _itemID;

        public BaseItem()
        {
            _Name = "Ничего";
            _Description = "";
            _Coast = 0;
            /*
             *     ! ВНИМАНИЕ !
             * Объявить сигнатуру _Options необходимо
             * в конструкторе класса по-умолчанию
             * Отдельно под каждый тип предмета
            */
        }

        public BaseItem(ItemCard card)
        {
            _ID = card.Id;
            _itemID = card.ItemId;
            _Name = card.Name;
            _Description = card.Description;
            _Coast = card.Cash;
            _Options = card.Options.Split("|");
        }

        public BaseItem(string itemID)
        {
            if (itemID == null) return;

            using (MembersDataContext MemberArchive = new MembersDataContext())
            {
                _itemCards = MemberArchive.ItemCards.ToList();
            }

            if (_itemCards == null) return;

            _itemCard = _itemCards.Find(i => i.ItemId.Contains(itemID));

            if (_itemCard == null) return;

            new Item(_itemCard);
        }
    }
}
