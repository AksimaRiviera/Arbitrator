using System.Text;
using ToilettenArbitrator.ToilettenWars.Items.Interfaces;
using ToilettenArbitrator.ToilettenWars.Items.Types;

namespace ToilettenArbitrator.ToilettenWars.Items
{
    public class DeviceShop
    {

        private MembersDataContext MemberArchive = new MembersDataContext();

        public const string SHOP_NAME = "SHIT SHOP";
        public const string WSHOP = "/wshop";
        public const string ASHOP = "/ashop";
        public const string HSHOP = "/hshop";
        public const string PSHOP = "/pshop";
        private string _itemID;
        private StringBuilder _weaponBarInfo = new StringBuilder();
        private StringBuilder _armorBarInfo = new StringBuilder();
        private StringBuilder _helmetBarInfo = new StringBuilder();
        private StringBuilder _potionBarInfo = new StringBuilder();
        private List<Item> _allItems;
        private List<ItemCard> _itemCards;

        public string WhatItemBought { set => _itemID = value; }
        public Item PurchasedItem => GetItem();
        public string ShopInfo => ShopCreate();

        public string WeaponBar => _weaponBarInfo.ToString();
        public string ArmorBar => _armorBarInfo.ToString();
        public string HelmetBar => _helmetBarInfo.ToString();
        public string PotionBar => _potionBarInfo.ToString();

        public DeviceShop()
        {
            _allItems = new List<Item>();

            _itemCards = MemberArchive.ItemCards.ToList();

            for (int i = 0; i < _itemCards.Count; i++)
            {
                _allItems.Add(new Item(_itemCards[i]));
            }
            WeaponBarInfo();
            ArmorBarInfo();
            HelmetBarInfo();
            PotionBarInfo();
        }

        private string ShopCreate()
        {
            StringBuilder ShopLog = new StringBuilder("ВАС ПРИВЕТСТВУЕТ\r\n\n\r" + 
            $"   <b>{SHOP_NAME}</b>\r\n\r\nВ ассортименте:\r\n\r\n");

            ShopLog.AppendLine($"&#128481 >> " + "<b>[ </b>" + WSHOP + "<b> ]</b>" + Environment.NewLine +
                $"&#128737 >> " + "<b>[</b> " + ASHOP + " <b>]</b>" + Environment.NewLine +
                $"&#129686 >> " + "<b>[</b> " + HSHOP + " <b>]</b>" + Environment.NewLine +
                $"&#128138 >> " + "<b>[</b> " + PSHOP + " <b>]</b>" + Environment.NewLine);

            ShopLog.AppendLine("<b><i>SHIT SHOP - ГОВНА НЕ ДЕРЖИМ!</i></b>");

            return ShopLog.ToString();
        }

        private Item GetItem()
        {
            return _allItems.Find(product => product.ItemID.Contains(_itemID));
        }

        private void WeaponBarInfo()
        {
            _weaponBarInfo.Clear();
            int count = 1;

            _weaponBarInfo.AppendLine($"В воздухе повисают предметы{Environment.NewLine}");

            for (int i = 0; i < _allItems.Count; i++)
            {
                if (_allItems[i].Type != ItemsType.Weapon) continue;

                _weaponBarInfo.AppendLine($"{count++}. {_allItems[i].Name}{Environment.NewLine}" +
                    $"( &#128176: {_allItems[i].Coast} ){Environment.NewLine}" +
                    $"<code>[ B ] [</code> /buy{_allItems[i].ItemID} <code>]</code>{Environment.NewLine}" +
                    $"<code>[ I ] [</code> /info{_allItems[i].ItemID} <code>]</code>");
            }
        }
        private void ArmorBarInfo()
        {
            _armorBarInfo.Clear();
            int count = 1;

            _armorBarInfo.AppendLine($"В воздухе повисают предметы{Environment.NewLine}");

            for (int i = 0; i < _allItems.Count; i++)
            {
                if (_allItems[i].Type != ItemsType.Armor) continue;

                _armorBarInfo.AppendLine($"{count++}. {_allItems[i].Name}{Environment.NewLine}" +
                    $"( &#128176: {_allItems[i].Coast} ){Environment.NewLine}" +
                    $"<code>[ B ] [</code> /buy{_allItems[i].ItemID} <code>]</code>{Environment.NewLine}" +
                    $"<code>[ I ] [</code> /info{_allItems[i].ItemID} <code>]</code>");
            }
        }

        private void HelmetBarInfo()
        {
            _helmetBarInfo.Clear();
            int count = 1;

            _helmetBarInfo.AppendLine($"В воздухе повисают предметы{Environment.NewLine}");

            for (int i = 0; i < _allItems.Count; i++)
            {
                if (_allItems[i].Type != ItemsType.Helmet) continue;

                _helmetBarInfo.AppendLine($"{count++}. {_allItems[i].Name}{Environment.NewLine}" +
                    $"( &#128176: {_allItems[i].Coast} ){Environment.NewLine}" +
                    $"<code>[ B ] [</code> /buy{_allItems[i].ItemID} <code>]</code>{Environment.NewLine}" +
                    $"<code>[ I ] [</code> /info{_allItems[i].ItemID} <code>]</code>");
            }
        }
        private void PotionBarInfo()
        {
            _potionBarInfo.Clear();
            int count = 1;

            _potionBarInfo.AppendLine($"В воздухе повисают предметы{Environment.NewLine}");

            for (int i = 0; i < _allItems.Count; i++)
            {
                if (_allItems[i].Type != ItemsType.HealPotion) continue;

                _potionBarInfo.AppendLine($"{count++}. {_allItems[i].Name}{Environment.NewLine}" +
                    $"( &#128176: {_allItems[i].Coast} ){Environment.NewLine}" +
                    $"<code>[ B ] [</code> /buy{_allItems[i].ItemID} <code>]</code>{Environment.NewLine}" +
                    $"<code>[ I ] [</code> /info{_allItems[i].ItemID} <code>]</code>");
            }
        }

    }
}