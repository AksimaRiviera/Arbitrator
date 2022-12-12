using ToilettenArbitrator.ToilettenWars.Items;

namespace ToilettenArbitrator.ToilettenWars.Cages
{
    public class SimpleMob : Mob
    {
        public float Damage => _damage;
        public float Defence => _defence;
        public float HitPoints => _hitPoints;
        public float Expirience => _expirience;

        public long Cash => _cash;

        public int PositionX => _positionX;
        public int PositionY => _positionY;

        public SimpleMob(MobCard card) : base(card)
        {
            
        }

        public override bool AddDamage(float damage, out LootBox Loot)
        {
            if (damage > _hitPoints)
            {
                if (new SilverDice().Luck(3))
                    Loot = new LootBox(LootBox.LootType.Rich);
                else Loot = new LootBox(LootBox.LootType.Standart);
                return true;
            }
            else
            {
                _hitPoints -= damage;
                Loot = new LootBox();
                return false;
            }
        }
    }
}