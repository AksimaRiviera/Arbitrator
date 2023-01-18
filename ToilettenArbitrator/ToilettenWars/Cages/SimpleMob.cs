using ToilettenArbitrator.ToilettenWars.Items;
using ToilettenArbitrator.ToilettenWars.Person;

namespace ToilettenArbitrator.ToilettenWars.Cages
{
    public class SimpleMob : Mob
    {
        private float _ageFactor;
        private float _levelFactor;

        private string _status;
        private string[] _illData = new string[] {
            "golova", "zlato"
        };

        // _lootArgs
        // [0] - тип возраста
        // [1] - тип опытности
        // [2] - базовый опыт за моба 
        // [3] - базовые бабки за моба
        // [4] - базовый ранговый опыт
        // [5] - кол-во возможных вещей
        // всё остальное id-шники возможных вещей
        private string[] _lootArgs;

        protected AgeRanks _ageRank;
        protected LevelRanks _levelRank;

        private readonly Dictionary<string, float> Factors = new Dictionary<string, float>(6) {
            { "Young", 1.25f },
            { "Acient", 1.75f },
            { "Relict", 2.30f },
            { "Ordinary", 1.80f },
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

        public string Status => _status;
        public float Damage => _damage;
        public float Defence => _defence;
        public float HitPoints => _hitPoints;
        public int PositionX => _positionX;
        public int PositionY => _positionY;
        public float MaximumHitPoints => _maximumHitPoints;

        public SimpleMob(MobCard card) : base(card)
        {
            MobSettings();
            LootSettings();
            StatusSettings();
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

        protected override void MobSettings()
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
                    else if(new SilverDice().Luck(13))
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
                    else if(new SilverDice().Luck(30))
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

        protected override void LootSettings()
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

        public override Ill Infect()
        {
            if (new SilverDice().Luck(8)) return new Ill(_illData[new Random().Next(_illData.Length)]);
            else return new Ill();
        }
    }
}