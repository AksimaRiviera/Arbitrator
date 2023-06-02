using ToilettenArbitrator.ToilettenWars.Items;
using Windows.UI.Composition.Interactions;

namespace ToilettenArbitrator.ToilettenWars.Person
{
    public abstract class BaseHero
    {
        private MembersDataContext MDC = new MembersDataContext();
        private readonly Dictionary<string, string> _heart = new Dictionary<string, string>(4) {
            { "Healthy", "&#128154" },
            { "Injured", "&#128155" },
            { "DeathDoor", "&#10084" },
            { "Dead", "&#128420" }
        };
        private const int BASE_FIELD_OF_WIEV = 3;
        private const int BASE_ATTACK_RANGE = 1;

        private int _levelCorrection;

        private bool _demiGod;

        internal HeroCard _card;

        internal const int EQUIPMENT_VOLUME = 10;
        internal const int INVENTORY_DEFAULT_VOLUME = 4;
        internal const int ROOM_MIN = 0;
        internal const int ROOM_MAX = 200;
        internal const int HP_FACTOR = 4;
        internal const int EXPIRIENCE_LEVEL_FACTOR = 10;
        internal const int EXPIRIENCE_RANK_FACTOR = 150;

        internal const float BASE_DEFENCE_MOD = 0.032f;
        internal const float MAIN_FACTOR = 1.1f;
        internal const float BASE_RANK_ATK_FACTOR = 1.0f;
        internal const float BASE_RANK_ATK_BIAS = 0.4f;
        internal const float BASE_ACCUMULATION_MOD = 0.2f;
        internal const float EXPIRIENCE_FACTOR = 0.1f;
        internal const float CLEAN_FACTOR = 0.3f;
        internal const float GUANO_METABOLISM_FACTOR = 0.18f;
        internal const float GUANO_STOMACH_FACTOR = 0.25f;

        internal Weapon _weapon;
        internal Armor _armor, _shield, _helmet;
        
        protected Ranks _rank;

        protected List<Item> _equipment = new List<Item>(EQUIPMENT_VOLUME);
        protected List<Item> _inventory;

        protected Ill _parasite = new Ill();
        protected Ill _phisical = new Ill();
        protected Ill _mental = new Ill();

        protected QuestBox _quest;
        protected List<QuestBox> _quests;

        protected int _id;
        protected int _level;
        protected int _toxic;
        protected int _fats;
        protected int _stomach;
        protected int _metabolism;
        protected int _freePoints;
        protected int _movementPoints;
        protected int _fieldOfWiev = BASE_FIELD_OF_WIEV;
        protected int _attackRange = BASE_ATTACK_RANGE;

        protected int[] _position = new int[3];

        protected long _money;

        protected string _name;
        protected string _rankDescription;

        protected string[] _bag;
        protected string[] _questList;

        protected float _levelExpirience;
        protected float _rankExpirience;
        protected float _dirty;

        protected int LevelCorrection => _levelCorrection;
        public bool DemiGod => _demiGod;
        public float MaximumDirty => (float)(Math.Round((float)((_fats * 5) + (_level * 3)), 2)) * HP_FACTOR; // Максимальный уровень загрязнения (то есть здоровья)

        public string Heart => HeartSettings();

        public BaseHero(HeroCard Card)
        {
            if (Card == null) return;

            _card = Card;

            _demiGod = _card.DemiGod;

            string[] NameArr = _card.Name.Split("|");
            string[] LRArr = _card.LevelRank.Split(".");
            string[] AtributesArr = _card.Atributes.Split(".");
            string[] PosArr = _card.Position.Split(".");
            string[] ExpArr = _card.Expirience.Split("|");
            _bag = _card.Inventory.Split("|");

            _name = NameArr[0];

            _level = int.Parse(LRArr[0]);

            _levelExpirience = float.Parse(ExpArr[0]);
            _rankExpirience = float.Parse(ExpArr[1]);

            _dirty = float.Parse(Card.Dirty);

            _position[0] = int.Parse(PosArr[0]);
            _position[1] = int.Parse(PosArr[1]);
            _movementPoints = int.Parse(PosArr[2]);

            _money = _card.Money;

            _questList = _card.TimersOne.Split("|");

            MainAtributesSet(AtributesArr);
            RankSorter(LRArr);
            BagSorter(_bag);
            QuestSorter(_questList);
            IllSettings();
        }

        private string HeartSettings()
        {
            if (_dirty >= MaximumDirty * 0.95f)
            {
                return _heart["Dead"];
            }
            else if (_dirty < MaximumDirty * 0.95f && _dirty >= MaximumDirty * 0.51f)
            {
                return _heart["DeathDoor"];
            }
            else if (_dirty < MaximumDirty * 0.51f && _dirty >= MaximumDirty * 0.12f)
            {
                return _heart["Injured"];
            }
            else
            {
                return _heart["Healthy"];
            }
        }
        private void IllSettings()
        {

            string[] illdata = _card.TimersTwo.Split('|');

            _parasite = new Ill(illdata[0]);
            _phisical = new Ill(illdata[1]);
            _mental = new Ill(illdata[2]);
        }
        private void MainAtributesSet(string[] AtributesArgs)
        {
            _toxic = int.Parse(AtributesArgs[0]);
            _fats = int.Parse(AtributesArgs[1]);
            _stomach = int.Parse(AtributesArgs[2]);
            _metabolism = int.Parse(AtributesArgs[3]);
            _freePoints = int.Parse(AtributesArgs[4]);
            _demiGod = _card.DemiGod;

            if (_level >= 5)
            {
                _levelCorrection = (_level / 5) + 1;
            }
            else
            {
                _levelCorrection = 1;
            }
        }

        private void RankSorter(string[] LRArgs)
        {
            switch (int.Parse(LRArgs[1]))
            {
                case 1:
                    _rank = Ranks.Slap;
                    break;
                case 2:
                    _rank = Ranks.Redolent;
                    break;
                case 3:
                    _rank = Ranks.Scented;
                    break;
                case 4:
                    _rank = Ranks.Smelly;
                    break;
                case 5:
                    _rank = Ranks.Stinking;
                    break;
                case 6:
                    _rank = Ranks.Addle;
                    break;
                case 7:
                    _rank = Ranks.Toxic;
                    break;
                case 8:
                    _rank = Ranks.Fetid;
                    break;
                default:
                    _rank = Ranks.Slap;
                    break;
            }
        }
        private void QuestSorter(string[] questList)
        {            
            _quests = new List<QuestBox>();

            for (int i = 0; i < questList.Length; i++)
            {
                if (questList[i] == string.Empty) continue;
                _quests.Add(new QuestBox(questList[i]));
            }
        }
        private void BagSorter(string[] bag)
        {
            if (bag[0] == "E")
            {
                _inventory = new List<Item>(INVENTORY_DEFAULT_VOLUME);

                for (int i = 0; i < _inventory.Capacity; i++)
                {
                    _inventory.Add(new Item());
                }
                for (int i = 0; i < EQUIPMENT_VOLUME; i++)
                {
                    _equipment.Add(new Item());
                }
            }
            else
            {
                _inventory = new List<Item>(int.Parse(bag[0]));

                for (int i = 1; i < 11; i++)
                {
                    _equipment.Add(new Item(bag[i]));
                }

                for (int i = 11; i < bag.Length; i++)
                {
                    _inventory.Add(new Item(bag[i]));
                }
            }

            if (_equipment[0].Name == "ничего") _weapon = new Weapon();
            else { _weapon = new Weapon(_equipment[0].ItemID); }

            if (_equipment[1].Name == "ничего") { _armor = new Armor(); }
            else { _armor = new Armor(_equipment[1].ItemID); }

            if (_equipment[2].Name == "ничего") { _shield = new Armor(); }
            else { _shield = new Armor(_equipment[2].ItemID); }

            if (_equipment[3].Name == "ничего") { _helmet = new Armor(); }
            else { _helmet = new Armor(_equipment[3].ItemID); }
        }
        protected void IllSave()
        {
            _card.TimersTwo = $"{_parasite.ID}.{_parasite.Step}|{_phisical.ID}.{_phisical.Step}|{_mental.ID}.{_mental.Step}";

            MDC.Update(_card);
            MDC.SaveChanges();
        }
        public void AddQuest(QuestBox quest)
        {
            if (_quests.Count > 0)
            { 
                _quest = quest;
                _quests.Add(_quest);
                _card.TimersOne += $"|{_quest.QuestID}.{_quest.StartCount}";
            }
            else
            {
                _quest = quest;
                _quests.Add(_quest);
                _card.TimersOne += $"{_quest.QuestID}.{_quest.StartCount}";
            }

            MDC.Update(_card);
            MDC.SaveChanges();
        }
        public bool AddIll(Ill ill)
        {
            if (ill.ID != "E")
            {
                switch (ill.Type)
                {
                    case IllTypes.IllType.Parasite:
                        _parasite = ill;
                        IllSave();
                        return true;

                    case IllTypes.IllType.Phisical:
                        _phisical = ill;
                        IllSave();
                        return true;

                    case IllTypes.IllType.Mental:
                        _mental = ill;
                        IllSave();
                        return true;

                    default:
                        _parasite = new Ill();
                        _phisical = new Ill();
                        _mental = new Ill();
                        IllSave();
                        return false;
                }
            }
            else
            {
                return false;
            }
        }

        internal void SaveQuestData()
        {
            string _qData = string.Empty;
            if (_quests.Count > 0)
            {
                for (int i = 0; i < _quests.Count; i++)
                {
                    if (_quests[i].QuestID == string.Empty || _quests[i].QuestID == "E" ) continue;
                    _qData += $"{_quests[i].QuestID}.{_quests[i].Progress}|";
                }
            }
            _card.TimersOne = _qData;
            MDC.Update(_card);
            MDC.SaveChanges();
        }

        internal float BaseAttack()
        {
            switch (_rank)
            {
                case Ranks.Slap:
                    return _toxic * GuanoAccumulation() * (BASE_RANK_ATK_FACTOR + BASE_RANK_ATK_BIAS * (int)_rank);
                case Ranks.Redolent:
                    return _toxic * GuanoAccumulation() * (BASE_RANK_ATK_FACTOR + BASE_RANK_ATK_BIAS * (int)_rank);
                case Ranks.Scented:
                    return _toxic * GuanoAccumulation() * (BASE_RANK_ATK_FACTOR + BASE_RANK_ATK_BIAS * (int)_rank);
                case Ranks.Smelly:
                    return _toxic * GuanoAccumulation() * (BASE_RANK_ATK_FACTOR + BASE_RANK_ATK_BIAS * (int)_rank);
                case Ranks.Stinking:
                    return _toxic * GuanoAccumulation() * (BASE_RANK_ATK_FACTOR + BASE_RANK_ATK_BIAS * (int)_rank);
                case Ranks.Addle:
                    return _toxic * GuanoAccumulation() * (BASE_RANK_ATK_FACTOR + BASE_RANK_ATK_BIAS * (int)_rank);
                case Ranks.Toxic:
                    return _toxic * GuanoAccumulation() * (BASE_RANK_ATK_FACTOR + BASE_RANK_ATK_BIAS * (int)_rank);
                case Ranks.Fetid:
                    return _toxic * GuanoAccumulation() * (BASE_RANK_ATK_FACTOR + BASE_RANK_ATK_BIAS * (int)_rank);
                default:
                    return _toxic * GuanoAccumulation() * (BASE_RANK_ATK_FACTOR + BASE_RANK_ATK_BIAS);
            }
        }
        internal float BaseDefense()
        {
            switch (_rank)
            {
                case Ranks.Slap:
                    return _fats * ((float)Math.Sqrt(BASE_DEFENCE_MOD * (int)_rank) + MAIN_FACTOR);
                case Ranks.Redolent:
                    return _fats * ((float)Math.Sqrt(BASE_DEFENCE_MOD * (int)_rank) + MAIN_FACTOR);
                case Ranks.Scented:
                    return _fats * ((float)Math.Sqrt(BASE_DEFENCE_MOD * (int)_rank) + MAIN_FACTOR);
                case Ranks.Smelly:
                    return _fats * ((float)Math.Sqrt(BASE_DEFENCE_MOD * (int)_rank) + MAIN_FACTOR);
                case Ranks.Stinking:
                    return _fats * ((float)Math.Sqrt(BASE_DEFENCE_MOD * (int)_rank) + MAIN_FACTOR);
                case Ranks.Addle:
                    return _fats * ((float)Math.Sqrt(BASE_DEFENCE_MOD * (int)_rank) + MAIN_FACTOR);
                case Ranks.Toxic:
                    return _fats * ((float)Math.Sqrt(BASE_DEFENCE_MOD * (int)_rank) + MAIN_FACTOR);
                case Ranks.Fetid:
                    return _fats * ((float)Math.Sqrt(BASE_DEFENCE_MOD * (int)_rank) + MAIN_FACTOR);
                default:
                    return _fats * ((float)Math.Sqrt(BASE_DEFENCE_MOD * (int)_rank) + MAIN_FACTOR);
            }

        }
        protected float CalculateStomachVolume()
        {
            return (float)Math.Round((float)((_stomach * 5) + (_level * 2)), 2);
        }
        protected float GuanoAccumulation()
        {
            return MAIN_FACTOR + ((_metabolism * GUANO_METABOLISM_FACTOR) * (_stomach * GUANO_STOMACH_FACTOR));
        }
        protected int GetFieldOfWiev()
        {
            return _fieldOfWiev;// + _helmet.WievBonus;
        }
        protected abstract float ClearAttack();
        protected abstract float ClearDefence();
        public abstract bool AddItem(Item item);

        public void IllsTick()
        {
            if (!_parasite.ID.Contains('E'))
            {
                switch (_parasite.SufferingParameter)
                {
                    case "money":
                        if (_parasite.IllProcess()) _money = (long)(_money - _parasite.MainFactor);
                        _card.Money = _money;
                        break;
                    case "exp":
                        if (_parasite.IllProcess()) _levelExpirience -= _parasite.MainFactor;
                        _card.Expirience = $"{_levelExpirience}|{_rankExpirience}";
                        break;
                    case "rank":
                        if (_parasite.IllProcess()) _rankExpirience -= _parasite.MainFactor;
                        _card.Expirience = $"{_levelExpirience}|{_rankExpirience}";
                        break;
                    default:
                        break;
                }
            }

            if (!_phisical.ID.Contains('E')) _phisical.IllProcess();
            if (!_mental.ID.Contains('E')) _mental.IllProcess();
            IllSave();
        }
    }
}