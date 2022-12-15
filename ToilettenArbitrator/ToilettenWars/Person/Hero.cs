using System.Security.Cryptography;
using ToilettenArbitrator.ToilettenWars.Items;

namespace ToilettenArbitrator.ToilettenWars.Person
{
    public class Hero : BaseHero, IHero
    {
        public enum Directions
        {
            North,
            South,
            West,
            East
        }

        public enum Characteristics
        {
            Toxic,
            Fats,
            Stomach,
            Metabolism
        }


        /// <summary>
        /// Атака персонажа
        /// основанная на базовой атаке 
        /// и уроне оружия.
        /// </summary>
        /// <returns><b>BaseHero.BaseAttack + Weapon.Damage</b></returns>
        public float Attack => ClearAttack();
        public float Defence => ClearDefence();

        public int ID => _id;
        public string Name => _name;


        public int Level => _level;
        public float LevelExpirience => _levelExpirience;
        public Ranks Rank => _rank;
        public float RankExpirience => _rankExpirience;

        /// <summary>
        /// Токсичность, главный параметр, учавствует в атаке
        /// </summary>
        public int Toxic => _toxic;
        /// <summary>
        /// Жиры, главный параметр, учавствует в защите
        /// </summary>
        public int Fats => _fats;
        /// <summary>
        /// Чрево, главный параметр, учавствует в атаке
        /// </summary>
        public int Stomach => _stomach;
        /// <summary>
        /// Матаболизм, главный параметр, учавствует в защите
        /// </summary>
        public int Metabolism => _metabolism;
        /// <summary>
        /// Колличество свободных очков, для прокачки главных параметров
        /// </summary>
        public int FreePoints => _freePoints;

        public float Dirty => _dirty;

        public int PositionX => _position[0];
        public int PositionY => _position[1];
        public int MovementPoints => _movementPoints;

        public long Money => _money;

        public int MaxLevelExpirience => (Toxic + Fats + Stomach + Metabolism + Level + FreePoints) * EXPIRIENCE_LEVEL_FACTOR;

        public Weapon Weapon => _weapon;
        public Armor Armor => _armor;
        public Armor Shield => _shield;
        public Armor Helmet => _helmet;

        public Hero(HeroCard card) : base(card)
        {

        }

        public override bool AddItem(Item item)
        {
            int emptyCellsCount = 0;
            using MembersDataContext MDC = new MembersDataContext();
            for (int i = 0; i < _inventory.Length; i++)
            {
                if (_inventory[i].ToLower().Contains("e"))
                {
                    emptyCellsCount++;
                }
            }
            
            if (emptyCellsCount == 0)
            {
                return false;
            }
            else
            {
                _inventory[_inventory.Length - emptyCellsCount] = item.ItemID;
                _card.Inventory += $"|{item.ItemID}";
                MDC.Update(_card);
                MDC.SaveChanges();
                return true;
            }
        }

        protected override float ClearAttack()
        {
            if (Weapon != null) return Weapon.RealDamage + BaseAttack();
            else return BaseAttack() + new SilverDice().HandDamage;
        }

        protected override float ClearDefence()
        {
            return BaseDefense() + Armor.Defence + Shield.Defence + Helmet.Defence;
        }

        public void ChangeLevelExpirience(float expirience)
        {
            if (_levelExpirience + expirience > MaxLevelExpirience)
            {
                _levelExpirience = (_levelExpirience + expirience) - MaxLevelExpirience;
                _level += 1;
                _freePoints += 1;

                using MembersDataContext MDC = new MembersDataContext();

                _card.Expirience = $"{LevelExpirience}|{RankExpirience}";
                _card.LevelRank = $"{Level}.{(int)Rank}";
                _card.Atributes = $"{Toxic}.{Fats}.{Stomach}.{Metabolism}.{FreePoints}";

                MDC.Update(_card);
                MDC.SaveChanges();
            }
            else
            {
                _levelExpirience += expirience;

                using MembersDataContext MDC = new MembersDataContext();

                _card.Expirience = $"{LevelExpirience}|{RankExpirience}";

                MDC.Update(_card);
                MDC.SaveChanges();
            }
        }

        public void ChangeRankExpirience(float expirience)
        {
            using MembersDataContext MDC = new MembersDataContext();

            _card.Expirience = $"{LevelExpirience}|{RankExpirience}";

            MDC.Update(_card);
            MDC.SaveChanges();
        }

        public void ChangePosition(Directions directions, int[] coordinates)
        {
            if (coordinates == null) coordinates = new int[] { 0, 0 };

            switch (directions)
            {
                case Directions.North:
                    _position[1] += 1;
                    break;
                case Directions.South:
                    _position[1] -= 1;
                    break;
                case Directions.West:
                    _position[0] -= 1;
                    break;
                case Directions.East:
                    _position[0] += 1;
                    break;
                default:
                    _position = coordinates;
                    break;
            }

            if (PositionX > ROOM_MAX) _position[0] -= ROOM_MAX;
            if (PositionY > ROOM_MAX) _position[1] -= ROOM_MAX;

            if (PositionX < ROOM_MIN) _position[0] += ROOM_MAX;
            if (PositionY < ROOM_MIN) _position[1] += ROOM_MAX;

            using MembersDataContext MDC = new MembersDataContext();

            _card.Position = $"{PositionX}.{PositionY}.{MovementPoints}";

            MDC.Update(_card);
            MDC.SaveChanges();
        }

        public void AddDamage(float damage, out float expirience, out int cash)
        {
            expirience = damage * EXPIRIENCE_FACTOR;
            cash = (int)(expirience + damage * 1.2f);
            if (cash < 1) cash = 1;

            if (_dirty + damage > MaximumDirty) _dirty = MaximumDirty;

            _dirty += damage;

            using MembersDataContext MDC = new MembersDataContext();

            _card.Dirty = $"{_dirty}";

            MDC.Update(_card);
            MDC.SaveChanges();
        }

        public void TakeMoney(long cash)
        {
            _money += cash;

            using MembersDataContext MDC = new MembersDataContext();

            _card.Money = _money;

            MDC.Update(_card);
            MDC.SaveChanges();
        }

        public long GetMoney(long itemPrice)
        {
            using MembersDataContext MDC = new MembersDataContext();
            if (_money < itemPrice)
            {
                return 0;
            }
            _money -= itemPrice;

            _card.Money = _money;

            MDC.Update(_card);
            MDC.SaveChanges();
            return itemPrice;
        }

        public bool UpdateCharacteristics(Hero.Characteristics characteristics)
        {
            if (FreePoints == 0) return false;
            _freePoints -= 1;

            switch (characteristics)
            {
                case Characteristics.Toxic:
                    _toxic += 1;
                    break;
                case Characteristics.Fats:
                    _fats += 1;
                    break;
                case Characteristics.Stomach:
                    _stomach += 1;
                    break;
                case Characteristics.Metabolism:
                    _metabolism += 1;
                    break;
            }

            using MembersDataContext MDC = new MembersDataContext();

            _card.Atributes = $"{Toxic}.{Fats}.{Stomach}.{Metabolism}.{FreePoints}";
            MDC.Update(_card);
            MDC.SaveChanges();

            return true;
        }
    }
} 