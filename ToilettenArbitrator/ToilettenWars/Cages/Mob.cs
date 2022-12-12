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
        protected float _expirience;
        protected long _cash;
        protected int _positionX;
        protected int _positionY;
        protected LootBox _lootBox;

        public string Id => _id;
        public string Name => _name;


        public Mob(MobCard card)
        {
            _id = card.Id;
            string[] data;

            data = card.Data.Split("|");

            _damage = (float)card.Attack;
            _defence = (float)card.Defence;
            _hitPoints = (float)card.HitPoints;
            _expirience = float.Parse(data[3]);
        }

        public abstract bool AddDamage(float damage, out LootBox Loot);
    }
}