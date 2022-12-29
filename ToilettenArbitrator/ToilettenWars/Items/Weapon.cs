using ToilettenArbitrator.ToilettenWars.Items.Interfaces;
using ToilettenArbitrator.ToilettenWars.Items.Types;

namespace ToilettenArbitrator.ToilettenWars.Items
{
    public class Weapon : Item, IWeapon
    {
        private const int WEAPON_BASE_MOD = 100;
        private bool _isCritical;

        public float MinDamage => float.Parse(_Options[2]);
        public float MaxDamage => float.Parse(_Options[3]);
        public float RealDamage => DamageCalculate();
        public float CriticalFactor => float.Parse(_Options[4]);
        public bool Critical => CriticalON();
        public int Fractions => FractionCalculate();
        public WeaponType Type { get; protected set; }

        public Weapon() : base()
        {
            _Name = "Ничего";
            _Description = "E";
            _itemID = "E";
        }

        public Weapon(ItemCard card) : base(card)
        {
            WhatType();
            if (base.Type != ItemsType.Weapon) return;
        }

        public Weapon(string itemID) : base(itemID)
        {
            WhatType();
            if (base.Type != ItemsType.Weapon) return;
        }


        private float DamageCalculate()
        {
            if (_Options == null) { return 0.0f; }
            else
            {
                return (float)(new Random().Next((int)(MinDamage * WEAPON_BASE_MOD), (int)(MaxDamage * WEAPON_BASE_MOD))) / WEAPON_BASE_MOD;
            }
        }

        protected override void WhatType()
        {
            base.WhatType();
            if(_Options == null) return; 

            switch (_Options[1])
            {
                case "critical":
                    Type = WeaponType.Critical;
                    break;

                case "piercing":
                    Type = WeaponType.Piercing;
                    break;

                case "cells":
                    Type = WeaponType.Cells;
                    break;

                case "simple":
                    Type = WeaponType.Simple;
                    break;

                default:
                    Type = WeaponType.Simple;
                    break;
            }
        }

        private int FractionCalculate()
        {
            return new Random().Next(
                int.Parse(_Options[5]),
                int.Parse(_Options[6]));
        }

        private bool CriticalON()
        {
            _isCritical = false;

            int critValue = new SilverDice().D20;

            if (critValue == 19 || critValue == 20) _isCritical = true;

            return _isCritical;
        }
    }
}
