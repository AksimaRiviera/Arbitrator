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
        public const string PSHOP = "/pshop";
        private string _itemID;
        private List<Item> _allItems;
        private List<ItemCard> _itemCards;

        public string WhatItemBought { set => _itemID = value; }
        public Item PurchasedItem => GetItem();
        public string ShopInfo => ShopCreate();

        public DeviceShop()
        {
            _allItems = new List<Item>();

            _itemCards = MemberArchive.ItemCards.ToList();

            for (int i = 0; i < _itemCards.Count; i++)
            {
                _allItems.Add(new Item(_itemCards[i]));
            }

        }

        private string ShopCreate()
        {
            StringBuilder ShopLog = new StringBuilder("ВАС ПРИВЕТСТВУЕТ\r\n\n\r" + 
            $"   <b>{SHOP_NAME}</b>\r\n\r\nВ ассортименте:\r\n\r\n");

            ShopLog.AppendLine($"&#128481 >> " + "<b>[ </b>" + WSHOP + "<b> ]</b>" + Environment.NewLine +
                $"&#128737 >> " + "<b>[ </b>" + ASHOP + "<b> ]</b>" + Environment.NewLine +
                $"&#128138 >> " + "<b>[ </b>" + PSHOP + "<b> ]</b>" + Environment.NewLine);

            int count = 1;

            ShopLog.AppendLine($"Чем ударить:{Environment.NewLine}");

            for (int i = 0; i < _allItems.Count; i++)
            {
                if (_allItems[i].Type != ItemsType.Weapon) continue;

                ShopLog.AppendLine($"{count++}. {_allItems[i].Name}{Environment.NewLine}" +
                    $"( &#128176: {_allItems[i].Coast} ){Environment.NewLine}" +
                    $"<b>[ BUY</b> /buy{_allItems[i].ItemID} <b>]</b>{Environment.NewLine}" +
                    $"<b>[ INFO</b> /info{_allItems[i].ItemID} <b>]</b>");
            }

            ShopLog.AppendLine($"{Environment.NewLine}Чем прикрыться:{Environment.NewLine}");
            count = 1;

            for (int i = 0; i < _allItems.Count; i++)
            {
                if (_allItems[i].Type != ItemsType.Armor && _allItems[i].Type != ItemsType.Helmet && _allItems[i].Type != ItemsType.Shield) continue;

                ShopLog.AppendLine($"{count++}. {_allItems[i].Name}{Environment.NewLine}" + 
                    $"( &#128176: {_allItems[i].Coast} ){Environment.NewLine}" +
                    $"<b>[ BUY</b> /buy{_allItems[i].ItemID} <b>]</b>{Environment.NewLine}" + 
                    $"<b>[ INFO</b> /info{_allItems[i].ItemID} <b>]</b>");
            }

            ShopLog.AppendLine($"{Environment.NewLine}Чем полечиться:{Environment.NewLine}");
            count = 1;

            for (int i = 0; i < _allItems.Count; i++)
            {
                if (_allItems[i].Type != ItemsType.HealPotion) continue;

                ShopLog.AppendLine($"{count++}. {_allItems[i].Name}{Environment.NewLine}" +
                    $"( &#128176: {_allItems[i].Coast} ){Environment.NewLine}" +
                    $"<b>[ BUY</b> /buy{_allItems[i].ItemID} <b>]</b>{Environment.NewLine}" +
                    $"<b>[ INFO</b> /info{_allItems[i].ItemID} <b>]</b>");
            }

            ShopLog.AppendLine("<b><i>SHIT SHOP - ГОВНА НЕ ДЕРЖИМ!</i></b>");

            return ShopLog.ToString();
        }

        private Item GetItem()
        {
            return _allItems.Find(product => product.ItemID.Contains(_itemID));
        }

    }
}