using System.Security.Cryptography;
using ToilettenArbitrator.ToilettenWars.Items;
using ToilettenArbitrator.ToilettenWars.Items.Types;

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
        public string RankDescription => _rankDescription;

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

        public List<Item> Inventory => _inventory; 

        public Hero(HeroCard card) : base(card)
        {

        }

        public override bool AddItem(Item item)
        {
            int count = 0;
            using MembersDataContext MDC = new MembersDataContext();

            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].Name.ToLower().Contains("ничего") == false)
                {
                    count++;
                }
                else
                {
                    _inventory[i] = item;
                    break;
                }
            }

            if (count == _inventory.Count)
            {
                return false;
            }
            else
            {
                _card.Inventory = $"{bag[0]}";

                for (int i = 0; i < _equipment.Capacity; i++)
                {
                    if (_equipment[i].Name.ToLower().Contains("ничего"))
                    {
                        _card.Inventory += $"|{_equipment[i].Description}";
                    }
                    else
                    {
                        _card.Inventory += $"|{_equipment[i].ItemID}";
                    }
                }
                for (int i = 0; i < _inventory.Capacity; i++)
                {
                    if (_inventory[i].Name.ToLower().Contains("ничего"))
                    {
                        _card.Inventory += $"|{_inventory[i].Description}";
                    }
                    else
                    {
                        _card.Inventory += $"|{_inventory[i].ItemID}";
                    }
                }

                MDC.Update(_card);
                MDC.SaveChanges();
                return true;
            }
        }

        public bool EquipItem(string ItemId)
        {
            int inventoryCell = 0;

            for (inventoryCell = 0; inventoryCell < _inventory.Count; inventoryCell++)
            {
                if (_inventory[inventoryCell].ItemID == ItemId)
                {
                    _inventory.Remove(_inventory[inventoryCell]);
                    continue;
                }
            }

            HealingPotion potion = new HealingPotion(ItemId);

            _dirty -= potion.EffectValue;

            return true;

        }

        public bool UsePotion(string ItemId)
        {
            if (_inventory.Find(item => item.ItemID.Contains(ItemId)) == null)
            {
                return false;
            }
            else
            {
                _inventory.Remove(_inventory.First(item => item.ItemID.Contains(ItemId)));

                HealingPotion potion = new HealingPotion(ItemId);

                if (potion.PotionType == PotionType.Healing)
                {
                    if (_dirty < potion.EffectValue)
                    {
                        _dirty = 0;
                    }
                    else
                    {
                        _dirty -= potion.EffectValue;
                    }
                }
                else
                {
                    if (_dirty < potion.EffectValue)
                    {
                        _dirty = 0;
                    }
                    else
                    {
                        _dirty -= _dirty * potion.EffectValue;
                    }
                }

                using MembersDataContext MDC = new MembersDataContext();

                _card.Inventory = $"{bag[0]}";

                for (int i = 0; i < _equipment.Capacity; i++)
                {
                    if (_equipment[i].Name.ToLower().Contains("ничего"))
                    {
                        _card.Inventory += $"|{_equipment[i].Description}";
                    }
                    else
                    {
                        _card.Inventory += $"|{_equipment[i].ItemID}";
                    }
                }
                for (int i = 0; i < _inventory.Capacity; i++)
                {
                    if (_inventory[i].Name.ToLower().Contains("ничего"))
                    {
                        _card.Inventory += $"|{_inventory[i].Description}";
                    }
                    else
                    {
                        _card.Inventory += $"|{_inventory[i].ItemID}";
                    }
                }

                _card.Dirty = $"{_dirty}";
                MDC.Update(_card);
                MDC.SaveChanges();
                return true;
            }
        }

        protected override float ClearAttack()
        {
            if (Weapon.Name != "Ничего") return Weapon.RealDamage + BaseAttack();
            else return BaseAttack() + new SilverDice().HandDamage;
        }

        protected override float ClearDefence()
        {

            return BaseDefense(); //Armor.Defence + Shield.Defence + Helmet.Defence;
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
        
        public string InventoryData()
        {
            string data = string.Empty;

            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i].Name.ToLower() != "ничего")
                {
                    switch (Inventory[i].Type)
                    {
                        case Items.Types.ItemsType.Weapon:
                            data += $"{i + 1}. {new Weapon(Inventory[i].ItemID).Name}";
                            data += $"{Environment.NewLine}<b>оружие</b>{Environment.NewLine}" + $"Экипировать?{Environment.NewLine}/equip{Inventory[i].ItemID}{Environment.NewLine}";
                            break;

                        case Items.Types.ItemsType.Armor:
                            data += $"{i + 1}. {new Armor(Inventory[i].ItemID).Name}";
                            data += $"{Environment.NewLine}<b>броня</b>{Environment.NewLine}" + $"Экипировать?{Environment.NewLine}/equip{Inventory[i].ItemID}{Environment.NewLine}";
                            break;

                        case Items.Types.ItemsType.Shield:
                            data += $"{i + 1}. {new Armor(Inventory[i].ItemID).Name}";
                            data += $"{Environment.NewLine}<b>щит</b>{Environment.NewLine}" + $"Экипировать?{Environment.NewLine}/equip{Inventory[i].ItemID}{Environment.NewLine}";
                            break;

                        case Items.Types.ItemsType.Helmet:
                            data += $"{i + 1}. {new Armor(Inventory[i].ItemID).Name}";
                            data += $"{Environment.NewLine}<b>шляпа</b> {Environment.NewLine}" + $"Экипировать?{Environment.NewLine}/equip{Inventory[i].ItemID}{Environment.NewLine}";
                            break;

                        case Items.Types.ItemsType.Boots:
                            data += $"{i + 1}. {Inventory[i].Name}";
                            data += $"{Environment.NewLine}<b>тапки</b> {Environment.NewLine}" + $"Экипировать?{Environment.NewLine}/equip{Inventory[i].ItemID}{Environment.NewLine}";
                            break;

                        case Items.Types.ItemsType.Gloves:
                            data += $"{i + 1}. {Inventory[i].Name}";
                            data += $"{Environment.NewLine}<b>варежки</b> {Environment.NewLine}" + $"Экипировать?{Environment.NewLine}/equip{Inventory[i].ItemID}{Environment.NewLine}";
                            break;

                        case Items.Types.ItemsType.HealPotion:
                            data += $"{i + 1}. {new HealingPotion(Inventory[i].ItemID).Name}";
                            data += $"{Environment.NewLine}<b>чистящее средство</b> {Environment.NewLine}" + $"Использовать?{Environment.NewLine}/use{Inventory[i].ItemID}{Environment.NewLine}";
                            break;

                        case Items.Types.ItemsType.OtherPotion:
                            data += $"{i + 1}. {Inventory[i].Name}";
                            data += $"{Environment.NewLine}<b>какая-то химия, можно пить</b> {Environment.NewLine}" + $"Использовать?{Environment.NewLine}/use{Inventory[i].ItemID}{Environment.NewLine}";
                            break;

                        default:
                            data += "Я вообще не знаю что это. Где ты это взял?...";
                            break;
                    }
                }
                else
                {
                    continue;
                }
            }

            return data;
        }
    }
} 