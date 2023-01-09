using ToilettenArbitrator.ToilettenWars.Items;

namespace ToilettenArbitrator.ToilettenWars.Cages
{
    public class SimpleMob : Mob
    {
        public float Damage => _damage;
        public float Defence => _defence;
        public float HitPoints => _hitPoints;
        public int PositionX => _positionX;
        public int PositionY => _positionY;
        public float MaximumHitPoints => _maximumHitPoints;

        public SimpleMob(MobCard card) : base(card)
        {
            
        }

        public override bool AddDamage(float damage, out LootBox Loot)
        {
            _hitPoints -= damage;
            
            if (_hitPoints <= 0)
            {
                if (new SilverDice().Luck(_lootChance)) { Loot = new LootBox(LootBox.LootType.Rich, _lootArgs); }
                else { Loot = new LootBox(LootBox.LootType.Standart, _lootArgs); }
                return true;
            }
            else
            {
                Loot = new LootBox();
                return false;
            }
        }

        protected override void BossSettings()
        {
            throw new NotImplementedException();
        }
    }
}