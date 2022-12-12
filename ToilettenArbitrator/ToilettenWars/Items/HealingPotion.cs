using ToilettenArbitrator.ToilettenWars.Items.Interfaces;
using ToilettenArbitrator.ToilettenWars.Items.Types;

namespace ToilettenArbitrator.ToilettenWars.Items
{
    internal class HealingPotion : Item, IPotion
    {
        private float _effectValue;
        private bool _isPercent;
        private PotionType _potionType = PotionType.Healing;

        public float EffectValue => _effectValue;
        public bool IsPercent => _isPercent;
        public PotionType PotionType => _potionType;

        public HealingPotion(ItemCard card) : base(card: card)
        {
            if (base.Type != ItemsType.HealPotion) return;
            if (_Options[1].Contains("1")) _potionType = PotionType.Regenerative;

            _effectValue = float.Parse(_Options[2]);
        }

        public HealingPotion(string itemID) : base(itemID)
        {

        }
    }
}
