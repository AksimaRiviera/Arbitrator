using ToilettenArbitrator.ToilettenWars.Items;

namespace ToilettenArbitrator.ToilettenWars.Person
{
    public abstract class BaseHero
    {
        internal HeroCard _card;

        internal const int EQUIPMENT_VOLUME = 10;
        internal const float BASE_DEFENCE_MOD = 0.02f;
        internal const float MAIN_FACTOR = 1.0f;
        internal const float BASE_RANK_ATK_FACTOR = 0.9f;
        internal const float BASE_RANK_ATK_BIAS = 0.2f;
        internal const float BASE_ACCUMULATION_MOD = 0.2f;
        internal const float EXPIRIENCE_FACTOR = 0.1f;
        internal const int EXPIRIENCE_LEVEL_FACTOR = 15;
        internal const float GUANO_METABOLISM_FACTOR = 0.18f;
        internal const float GUANO_STOMACH_FACTOR = 0.25f;
        internal const int ROOM_MIN = 0;
        internal const int ROOM_MAX = 200;
        internal const int HP_FACTOR = 10;

        internal Weapon _weapon;
        internal Armor _armor, _shield, _helmet;

        protected int _id;
        protected string _name;

        protected int _level;
        protected float _levelExpirience;

        protected Ranks _rank;
        protected float _rankExpirience;

        protected int _toxic;
        protected int _fats;
        protected int _stomach;
        protected int _metabolism;
        protected int _freePoints;

        protected float _dirty;
        protected int[] _position = new int[3];
        protected int _movementPoints;
        protected long _money;

        protected string[] _equipment = new string[EQUIPMENT_VOLUME];
        protected string[] _inventory;

        public float MaximumDirty => (float)Math.Round((float)((_fats * 5) + (_level * 3)), 2); // Максимальный уровень загрязнения (то есть здоровья)

        public BaseHero(HeroCard Card)
        {
            if (Card == null) return;

            _card = Card;

            string[] NameArr = Card.Name.Split("|");
            string[] LRArr = Card.LevelRank.Split(".");
            string[] AtributesArr = Card.Atributes.Split(".");
            string[] PosArr = Card.Position.Split(".");
            string[] ExpArr = Card.Expirience.Split("|");
            string[] bag = Card.Inventory.Split("|");

            _name = NameArr[0];

            _level = int.Parse(LRArr[0]);

            _levelExpirience = float.Parse(ExpArr[0]);
            _rankExpirience = float.Parse(ExpArr[1]);

            _toxic = int.Parse(AtributesArr[0]);
            _fats = int.Parse(AtributesArr[1]);
            _stomach = int.Parse(AtributesArr[2]);
            _metabolism = int.Parse(AtributesArr[3]);
            _freePoints = int.Parse(AtributesArr[4]);


            _dirty = float.Parse(Card.Dirty);

            _position[0] = int.Parse(PosArr[0]);
            _position[1] = int.Parse(PosArr[1]);
            _movementPoints = int.Parse(PosArr[2]);

            _money = Card.Money;

            switch (int.Parse(LRArr[1]))
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

            BagSorter(bag);
        }
        private void BagSorter(string[] bag)
        {
            if (bag[0].ToLower() == "e")
            {
                _inventory = new string[5];

                for (int i = 0; i < _inventory.Length; i++)
                {
                    _inventory[i] = "e";
                }
                for (int i = 0; i < EQUIPMENT_VOLUME; i++)
                {
                    _equipment[i] = "e";
                }
            }
            else
            {
                _inventory = new string[int.Parse(bag[0])];

                for (int i = 1; i < 11; i++)
                {
                    _equipment[i - 1] = bag[i];
                }

                for (int i = 11; i < bag.Length; i++)
                {
                    _inventory[i - 11] = bag[i];
                }
            }

            if (_equipment[0].ToLower().Contains("e")) _weapon = new Weapon();
            else { _weapon = new Weapon(_equipment[0]); }

            if (_equipment[1].ToLower().Contains("e")) { _armor = new Armor(); }
            else { _armor = new Armor(_equipment[1]); }

            if (_equipment[2].ToLower().Contains("e")) { _shield = new Armor(); }
            else { _shield = new Armor(_equipment[2]); }

            if (_equipment[3].ToLower().Contains("e")) { _helmet = new Armor(); }
            else { _helmet = new Armor(_equipment[3]); }
        }
        protected float BaseAttack()
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
        protected float BaseDefense()
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

        protected abstract float ClearAttack();
        protected abstract float ClearDefence();
        public abstract bool AddItem(Item item);
    }
}