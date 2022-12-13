using ToilettenArbitrator.ToilettenWars.Items;

namespace ToilettenArbitrator.ToilettenWars.Cages
{
    public abstract class Mob
    {
        private string _id;
        private string _name;
        protected float _damage;
        protected float _defence;
        protected float _hitPoints;
        protected int _positionX;
        protected int _positionY;
        protected LootBox _lootBox;
        protected AgeRanks _ageRank;
        protected LevelRanks _levelRank;

        public string Id => _id;
        public string Name => _name;


        public Mob(MobCard card)
        {
            _id = card.Id;
            _name = card.Name;
            string[] data;

            data = card.Data.Split("|");

            _damage = (float)card.Attack;
            _defence = (float)card.Defence;
            _hitPoints = (float)card.HitPoints;
            _positionX = new SilverDice().GetCoordinate;
            _positionY = new SilverDice().GetCoordinate;
        }

        public abstract bool AddDamage(float damage, out LootBox Loot);

        private class Namenator
        {
            
            public Namenator()
            {
                
            }
        }
    }
}