using ToilettenArbitrator.ToilettenWars.Items;
using ToilettenArbitrator.Brain;

namespace ToilettenArbitrator.ToilettenWars.Cages
{
    public abstract class Mob
    {
        private SilverDice _dice = new SilverDice();
        private MembersDataContext MDC = new MembersDataContext();
        private MobCard _card;
        private string _id;
        private string _name;
        private string _subName;
        private string _description;
        private string[] _data;
        protected float _damage;
        protected float _defence;
        protected float _hitPoints;
        protected float _maximumHitPoints;

        protected int _positionX;
        protected int _positionY;

        protected LootBox _lootBox;
        protected AgeRanks _ageRank;
        protected LevelRanks _levelRank;

        public string Id => _id;
        public string Name => _name;
        public string SubName => _subName;
        public string Description => _description;

        public Mob(MobCard card)
        {
            _card = card;
            ApplySettings(_card);
        }

        public Mob(string id)
        {
            _card = MDC.MobCards.ToArray().Find(m => m.iD.Contains(id));
            ApplySettings(_card);
        }

        protected abstract void BossSettings();

        public abstract bool AddDamage(float damage, out LootBox Loot);
        public bool GoCoordinate(int X, int Y)
        {
            if (X != null && Y != null)
            {
                _positionX = X;
                _positionY = Y;
                return true;
            }
            else
            {
                return false;
            }
        }
        private void ApplySettings(MobCard card)
        {
            _id = card.Id;
            _name = card.Name;
            _subName = new NamerSynapse().OneName;

            _damage = (float)card.Attack;
            _defence = (float)card.Defence;
            _hitPoints = (float)card.HitPoints;
            _maximumHitPoints = (float)card.HitPoints;
            _positionX = new SilverDice().GetCoordinate;
            _positionY = new SilverDice().GetCoordinate;
            _data = card.Data.Split('|');
            _description = _data[0];
        }
        public void Step()
        {
            int _side = _dice.D4;
            switch (_side) 
            {
                case 1:
                _positionX ++;
                break;

                case 2:
                _positionX --;
                break;

                case 3:
                _positionY ++;
                break;

                case 4:
                _positionY --;
                break;

                default :
                _positionX = _positionX;
                _positionY = _positionY;
                break;
           }
        }
    }
}