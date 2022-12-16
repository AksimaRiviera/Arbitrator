﻿using ToilettenArbitrator.ToilettenWars.Items.Interfaces;
using ToilettenArbitrator.ToilettenWars.Items.Types;

namespace ToilettenArbitrator.ToilettenWars.Items
{
    public class Item : BaseItem, IItem
    {

        public ItemsType Type { get; protected set; }
        public long ID => _ID;
        public string Name => _Name;
        public string Description => _Description;
        public long Coast => _Coast;

        public Item() : base()
        {

        }

        public Item(ItemCard card) : base(card)
        {
            WhatType();
        }

        public Item(string itemID) : base(itemID)
        {
            WhatType();
        }


        protected virtual void WhatType()
        {
            if (_Options == null) return;
            switch (_Options[0].ToLower())
            {
                case "weapon":
                    Type = ItemsType.Weapon;
                    break;

                case "armor":
                    Type = ItemsType.Armor;
                    break;

                case "shield":
                    Type = ItemsType.Shield;
                    break;

                case "helmet":
                    Type = ItemsType.Helmet;
                    break;

                case "boots":
                    Type = ItemsType.Boots;
                    break;

                case "gloves":
                    Type = ItemsType.Gloves;
                    break;

                case "heal":
                    Type = ItemsType.HealPotion;
                    break;

                case "other":
                    Type = ItemsType.OtherPotion;
                    break;

                default:
                    Type = ItemsType.OtherPotion;
                    break;
            }
        }
    }
}