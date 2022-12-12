using ToilettenArbitrator.ToilettenWars.Items.Interfaces;
using ToilettenArbitrator.ToilettenWars.Items.Types;

namespace ToilettenArbitrator.ToilettenWars.Items
{
    public class Armor : Item, IArmor
    {
        private const int MAX_BONUS_COUNT = 4;

        private float[] _bonus = new float[MAX_BONUS_COUNT];

        public float Defence => float.Parse(_Options[2]);
        public ArmorType Type { get; protected set; }

        public float FirstBonus;

        public Armor() : base()
        {
            _Options = new string[] { "armor", "normal", "0,0" };
        }

        public Armor(ItemCard card) : base(card)
        {
            WhatType();
            if (_Options == null) return;
            if (base.Type != ItemsType.Armor) return;

            for (int i = 3; i < _Options.Length; i++)
            {
                _bonus[i - 3] = float.Parse(_Options[i]);
            }
        }

        public Armor(string itemID) : base(itemID)
        {
            WhatType();
            if (_Options == null) return;

            if (base.Type != ItemsType.Armor) return;

            for (int i = 3; i < _Options.Length; i++)
            {
                _bonus[i - 3] = float.Parse(_Options[i]);
            }
        }

        protected override void WhatType()
        {
            base.WhatType();
            switch (_Options[1])
            {
                case "normal":
                    Type = ArmorType.Normal;
                    break;

                case "heavy":
                    Type = ArmorType.Heavy;
                    break;

                case "regeneration":
                    Type = ArmorType.Regeneration;
                    break;

                case "statup":
                    Type = ArmorType.StatUp;
                    break;

                default:
                    Type = ArmorType.Normal;
                    break;
            }
        }
    }
}
