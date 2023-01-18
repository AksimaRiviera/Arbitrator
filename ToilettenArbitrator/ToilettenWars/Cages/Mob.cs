using ToilettenArbitrator.ToilettenWars.Items;
using ToilettenArbitrator.Brain;
using ToilettenArbitrator.ToilettenWars.Person;

namespace ToilettenArbitrator.ToilettenWars.Cages
{
    public abstract class Mob
    {
        private readonly Dictionary<string, string> _heart = new Dictionary<string, string>(4) {
            { "Healthy", "&#128154" },
            { "Injured", "&#128155" },
            { "DeathDoor", "&#10084" },
            { "Dead", "&#128420" }
        };

        private SilverDice _dice = new SilverDice();
        private MembersDataContext MDC = new MembersDataContext();
        private MobCard _card;

        private string _id;
        private string _name;
        private string _subName;
        private string _description;
        private string[] _data;

        // Хранит аргументы моба
        // [0] - тип моба (simple, boss)
        // [1] - возраст моба (young, acient, relict)
        // [2] - градация опыта моба (ordinary, expirienced, mature)
        // [3] - шанс на повышеный лут
        // Далее ячейки должны передаваться string[] > LootBox()</b>
        // [4] - базовый опыт за моба 
        // [5] - базовые бабки за моба
        // [6] - базовый ранговый опыт
        // [7] - количество предметов в луте
        internal string[] _arguments;

        internal int _ageArg;
        internal int _levelArg;

        protected float _damage;
        protected float _defence;
        protected float _hitPoints;
        protected float _maximumHitPoints;

        protected int _rank;
        protected int _lootChance;
        protected int _positionX;
        protected int _positionY;

        protected LootBox _lootBox;
        protected MobTypes.MobType _mobType;

        public string Id => _id;
        public string Name => _name;
        public string SubName => _subName;
        public string Description => _description;
        public string Heart => HeartSettings();

        public Mob(MobCard card)
        {
            _card = card;
            ApplySettings(_card);
        }

        public Mob(string id)
        {
            _card = MDC.MobCards.ToList().Find(mob => mob.Id.Contains(id));
            ApplySettings(_card);
        }

        protected abstract void MobSettings();
        protected abstract void LootSettings();

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

            _arguments = _data[1].Split('.');
            SettingsMain();
        }

        private void SettingsMain()
        {
            _mobType = (MobTypes.MobType)int.Parse(_arguments[0]);
            _ageArg = int.Parse(_arguments[1]);
            _levelArg = int.Parse(_arguments[2]);

        }

        private string HeartSettings()
        {
            if (_hitPoints >= _maximumHitPoints * 0.88f)
            {
                return _heart["Healthy"];
            }
            else if (_hitPoints < _maximumHitPoints * 0.88f && _hitPoints >= _maximumHitPoints * 0.51f)
            {
                return _heart["Injured"];
            }
            else if (_hitPoints < _maximumHitPoints * 0.51f && _hitPoints >= _maximumHitPoints * 0.06f)
            {
                return _heart["DeathDoor"];
            }
            else
            {
                return _heart["Dead"];
            }
        }

        public void Step()
        {
            int _side = _dice.D4;
            switch (_side)
            {
                case 1:
                    _positionX++;
                    break;

                case 2:
                    _positionX--;
                    break;

                case 3:
                    _positionY++;
                    break;

                case 4:
                    _positionY--;
                    break;

                default:
                    _positionX = _positionX;
                    _positionY = _positionY;
                    break;
            }
        }

        public abstract Ill Infect();
    }
}