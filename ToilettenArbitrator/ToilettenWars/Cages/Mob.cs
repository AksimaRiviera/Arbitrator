using ToilettenArbitrator.ToilettenWars.Items;
using ToilettenArbitrator.Brain;
using ToilettenArbitrator.ToilettenWars.Person;

namespace ToilettenArbitrator.ToilettenWars.Cages
{
    public class Mob
    {
        private SilverDice _dice = new SilverDice();
        private MembersDataContext MDC = new MembersDataContext();

        private readonly Dictionary<string, string> _heart = new Dictionary<string, string>(4) {
            { "Healthy", "&#128154" },
            { "Injured", "&#128155" },
            { "DeathDoor", "&#10084" },
            { "Dead", "&#128420" }
        };

        private readonly Dictionary<string, float> Factors = new Dictionary<string, float>(6) {
            { "Young", 1.3f },
            { "Acient", 1.8f },
            { "Relict", 2.3f },
            { "Ordinary", 1.8f },
            { "Experienced", 2.23f },
            { "Mature", 2.78f }
        };

        private readonly Dictionary<string, string> Icons = new Dictionary<string, string>(6) {
            { "A_Brain", "&#129504" },
            { "B_Brain", "&#129504 &#129504" },
            { "C_Brain", "&#129504 &#129504 &#129504" },
            { "Young", "&#128118" },
            { "Acient", "&#128104" },
            { "Relict", "&#128116" }
        };

        private string _id;
        private string _name;
        private string _subName;
        private string _description;
        private string _status;
        private string[] _data;
        private string[] _illData = new string[] {
            "golova", "zlato"
        };

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
        protected string[] _arguments;

        // _lootArgs
        // [0] - тип возраста
        // [1] - тип опытности
        // [2] - базовый опыт за моба 
        // [3] - базовые бабки за моба
        // [4] - базовый ранговый опыт
        // [5] - кол-во возможных вещей
        // всё остальное id-шники возможных вещей
        private string[] _lootArgs;

        private float _ageFactor;
        private float _levelFactor;

        protected int _ageArg;
        protected int _levelArg;

        protected float _damage;
        protected float _defence;
        protected float _hitPoints;
        protected float _maximumHitPoints;

        protected int _rank;
        protected int _lootChance;
        protected int _positionX;
        protected int _positionY;

        protected AgeRanks _ageRank;
        protected LevelRanks _levelRank;
        protected LootBox _lootBox;
        protected MobTypes.MobType _mobType;

        public string Id => _id;
        public string Name => _name;
        public string SubName => _subName;
        public string Description => _description;
        public string Status => _status;
        public string Heart => HeartSettings();
        public float Damage => _damage;
        public float Defence => _defence;
        public float HitPoints => _hitPoints;
        public float MaximumHitPoints => _maximumHitPoints;
        public int PositionX => _positionX;
        public int PositionY => _positionY;
        public MobTypes.MobType Type => _mobType;

        public Mob(MobCard card)
        {
            ApplySettings(card);
        }

        public Mob(string id)
        {
            ApplySettings(MDC.MobCards.ToList().Find(mob => mob.Id.Contains(id)));
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
            _lootChance = int.Parse(_arguments[3]);
            SettingsMain();
        }

        private void SettingsMain()
        {
            _mobType = (MobTypes.MobType)int.Parse(_arguments[0]);
            _ageArg = int.Parse(_arguments[1]);
            _levelArg = int.Parse(_arguments[2]);
            MobSettings();
            StatusSettings();
            LootSettings();
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

        private void MobSettings()
        {
            switch (_ageArg)
            {
                case 1:
                    _ageRank = AgeRanks.Young;
                    break;

                case 2:
                    if (new SilverDice().Luck(13))
                    {
                        _ageRank = AgeRanks.Acient;
                    }
                    else
                    {
                        _ageRank = AgeRanks.Young;
                    }
                    break;

                case 3:
                    if (new SilverDice().Luck(7))
                    {
                        _ageRank = AgeRanks.Relict;
                    }
                    else if (new SilverDice().Luck(13))
                    {
                        _ageRank = AgeRanks.Acient;
                    }
                    else
                    {
                        _ageRank = AgeRanks.Young;
                    }

                    break;

                default:
                    _ageRank = AgeRanks.Young;
                    break;
            }

            switch (_levelArg)
            {
                case 3:
                    if (new SilverDice().Luck(15))
                    {
                        _levelRank = LevelRanks.Mature;
                    }
                    else if (new SilverDice().Luck(30))
                    {
                        _levelRank = LevelRanks.Experienced;
                    }
                    else
                    {
                        _levelRank = LevelRanks.Ordinary;
                    }
                    break;

                case 2:
                    if (new SilverDice().Luck(30))
                    {
                        _levelRank = LevelRanks.Experienced;
                    }
                    else
                    {
                        _levelRank = LevelRanks.Ordinary;
                    }
                    break;

                case 1:
                    _levelRank = LevelRanks.Ordinary;
                    break;
                default:
                    _levelRank = LevelRanks.Ordinary;
                    break;
            }

            switch (_ageRank)
            {
                case AgeRanks.Young:
                    _ageFactor = Factors["Young"];
                    break;
                case AgeRanks.Acient:
                    _ageFactor = Factors["Acient"];
                    break;
                case AgeRanks.Relict:
                    _ageFactor = Factors["Relict"];
                    break;
                default:
                    _ageFactor = Factors["Young"];
                    break;
            }

            switch (_levelRank)
            {
                case LevelRanks.Ordinary:
                    _levelFactor = Factors["Ordinary"];
                    break;
                case LevelRanks.Experienced:
                    _levelFactor = Factors["Experienced"];
                    break;
                case LevelRanks.Mature:
                    _levelFactor = Factors["Mature"];
                    break;
                default:
                    _levelFactor = Factors["Ordinary"];
                    break;
            }

            _damage = (_damage + _levelFactor) * _ageFactor;
            _defence = (_defence + _levelFactor) * _ageFactor;
            _hitPoints = (_hitPoints + _levelFactor) * _ageFactor;
            _maximumHitPoints = (_maximumHitPoints + _levelFactor) * _ageFactor;
        }
        
        private void LootSettings()
        {
            _lootArgs = new string[6 + (_arguments.Length - 8)];
            _lootArgs[0] = $"{(int)_ageRank}";
            _lootArgs[1] = $"{(int)_levelRank}";
            _lootArgs[2] = $"{_arguments[4]}";
            _lootArgs[3] = $"{_arguments[5]}";
            _lootArgs[4] = $"{_arguments[6]}";
            _lootArgs[5] = $"{_arguments[7]}";

            for (int i = 6; i < _lootArgs.Length; i++)
            {
                _lootArgs[i] = _arguments[i + 2];
            }
        }
        
        private void StatusSettings()
        {
            _status = "[ ";

            switch (_ageRank)
            {
                case AgeRanks.Young:
                    _status += $"{Icons["Young"]}<b> > </b>";
                    break;
                case AgeRanks.Acient:
                    _status += $"{Icons["Acient"]}<b> > </b>";

                    break;
                case AgeRanks.Relict:
                    _status += $"{Icons["Relict"]}<b> > </b>";

                    break;
                default:
                    _status += $"{Icons["Young"]}<b> > </b>";
                    break;
            }

            switch (_levelRank)
            {
                case LevelRanks.Ordinary:
                    _status += $"{Icons["A_Brain"]} ]";
                    break;

                case LevelRanks.Experienced:
                    _status += $"{Icons["B_Brain"]} ]";
                    break;

                case LevelRanks.Mature:
                    _status += $"{Icons["C_Brain"]} ]";
                    break;

                default:
                    _status += $"{Icons["A_Brain"]} ]";
                    break;
            }
        }

        public bool AddDamage(float damage, out LootBox Loot)
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
        
        public Ill Infect()
        {
            if (new SilverDice().Luck(8)) return new Ill(_illData[new Random().Next(_illData.Length)]);
            else return new Ill();
        }

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
    }
}