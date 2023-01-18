using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Security.Cryptography;
using System.Security.Policy;
using ToilettenArbitrator.ToilettenWars.Items;
using ToilettenArbitrator.ToilettenWars.Items.Types;

namespace ToilettenArbitrator.ToilettenWars.Person
{
    public class Hero : BaseHero, IHero
    {
        private MembersDataContext MDC = new MembersDataContext();
        
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

        public int MaxRankExpirience => (int)_rank * EXPIRIENCE_RANK_FACTOR;

        public Weapon Weapon => _weapon;
        public Armor Armor => _armor;
        public Armor Shield => _shield;
        public Armor Helmet => _helmet;

        public List<QuestBox> Quests => _quests;

        public List<Item> Inventory => _inventory;


        public Hero(HeroCard card) : base(card)
        {

        }

        public override bool AddItem(Item item)
        {
            int count = 0;

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
                _card.Inventory = $"{_bag[0]}";

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
        public void TakeLootBox(LootBox lootBox)
        {
            _levelExpirience += lootBox.Expirience;
            _money += lootBox.Cash;
            _rankExpirience += lootBox.RankPoints;

            _card.Money = _money;

            _card.Expirience = $"{LevelExpirience}|{RankExpirience}";
            _card.LevelRank = $"{Level}.{(int)Rank}";
            _card.Atributes = $"{Toxic}.{Fats}.{Stomach}.{Metabolism}.{FreePoints}";

            MDC.Update(_card);
            MDC.SaveChanges();
        }
        public bool EquipItem(string ItemId)
        {
            if(_inventory.Exists(item => item.ItemID.Contains(ItemId)))
            {
                switch (_inventory.Find(item => item.ItemID.Contains(ItemId)).Type)
                {
                    case ItemsType.Weapon:
                        _equipment[0] = new Item(ItemId);
                        _inventory[_inventory.FindIndex(item => item.ItemID.Contains(ItemId))] = new Item();
                        break;

                    case ItemsType.Armor:
                        _equipment[1] = new Item(ItemId);
                        _inventory[_inventory.FindIndex(item => item.ItemID.Contains(ItemId))] = new Item();
                        break;

                    case ItemsType.Shield:
                        _equipment[2] = new Item(ItemId);
                        _inventory[_inventory.FindIndex(item => item.ItemID.Contains(ItemId))] = new Item();
                        break;

                    case ItemsType.Helmet:
                        _equipment[3] = new Item(ItemId);
                        _inventory[_inventory.FindIndex(item => item.ItemID.Contains(ItemId))] = new Item();
                        break;

                    case ItemsType.Boots:
                        _equipment[4] = new Item(ItemId);
                        _inventory[_inventory.FindIndex(item => item.ItemID.Contains(ItemId))] = new Item();
                        break;

                    case ItemsType.Gloves:
                        _equipment[5] = new Item(ItemId);
                        _inventory[_inventory.FindIndex(item => item.ItemID.Contains(ItemId))] = new Item();
                        break;

                    case ItemsType.HealPotion:
                        break;

                    case ItemsType.OtherPotion:
                        break;

                    default:
                        break;
                }

                _card.Inventory = $"{_bag[0]}";

                for (int i = 0; i < _equipment.Count; i++)
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
                for (int i = 0; i < _inventory.Count; i++)
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

                if (_inventory.Count < _inventory.Capacity)
                {
                    _card.Inventory = string.Empty;
                    _card.Inventory = $"{_bag[0]}";

                    for (int i = 0; i < _equipment.Count; i++)
                    {
                        _card.Inventory += $"|{_equipment[i].ItemID}";
                    }
                    for (int i = 0; i < _inventory.Capacity; i++)
                    {
                        if (_inventory[i].Name != "ничего")
                        {
                            _card.Inventory += $"|{_inventory[i].ItemID}";
                        }
                        else
                        {
                            _card.Inventory += "|E";
                        }
                    }
                }

                _card.Dirty = $"{_dirty}";
                MDC.Update(_card);
                MDC.SaveChanges();
                return true;

            }
            else
            {
                return false;
            }
        }
        public bool UsePotion(string ItemId)
        {
            if (_inventory.Exists(item => item.ItemID.Contains(ItemId)) == false)
            {
                return false;
            }
            else
            {
                _inventory[_inventory.FindIndex(item => item.ItemID.Contains(ItemId))] = new Item();

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

                _card.Inventory = $"{_bag[0]}";

                for (int i = 0; i < _equipment.Count; i++)
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
                for (int i = 0; i < _inventory.Count; i++)
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

                if(_inventory.Count < _inventory.Capacity)
                {
                    _card.Inventory = string.Empty;
                    _card.Inventory = $"{_bag[0]}";

                    for (int i = 0; i < _equipment.Count; i++)
                    {
                        _card.Inventory += $"|{_equipment[i].ItemID}";
                    }
                    for (int i = 0; i < _inventory.Capacity ; i++)
                    {
                        if (_inventory[i].Name != "ничего")
                        {
                            _card.Inventory += $"|{_inventory[i].ItemID}";
                        }
                        else
                        {
                            _card.Inventory += "|E";
                        }
                    }
                }

                _card.Dirty = $"{_dirty}";
                MDC.Update(_card);
                MDC.SaveChanges();
                return true;
            }
        }
        public bool Gotcha(string questId, string mobId)
        {
            _quest = _quests.Find(q => q.QuestID.Contains(questId));
            int _quest_id_in_list =_quests.FindIndex(q => q.QuestID.Contains(questId));
            string _qData = string.Empty;

            if (_quest.Managed())
            {
                QuestComplete(_quest.QuestID);

                MDC.Update(_card);
                MDC.SaveChanges();

                return true;
            }
            else
            {
                _quests[_quest_id_in_list] = _quest;

                SaveQuestData();
                MDC.Update(_card);
                MDC.SaveChanges();

                return false;
            }

        }
        public void QuestComplete(string questId)
        {
            for (int i = 0; i < _quests.Count; i++)
            {
                if (_quests[i].QuestID == questId)
                {
                    _quests.RemoveAt(i);
                }
            }
            TakeLootBox(new QuestBox(questId).Prize);
            SaveQuestData();
        }

        protected override float ClearAttack()
        {
            if (Weapon.Name != "ничего") return Weapon.RealDamage + BaseAttack();
            else return BaseAttack() + new SilverDice().HandDamage;
        }
        protected override float ClearDefence()
        {
            float fullDefence = 0.0f;
            if (Armor.Name != "ничего") fullDefence += Armor.Defence;
            if (Shield.Name != "ничего") fullDefence += Shield.Defence;
            if (Helmet.Name != "ничего") fullDefence += Helmet.Defence;

            return fullDefence + BaseDefense();
        }
        public void ChangeLevelExpirience(float expirience)
        {
            if (_levelExpirience + expirience >= MaxLevelExpirience)
            {
                _levelExpirience = (_levelExpirience + expirience) - MaxLevelExpirience;
                _level += 1;
                _freePoints += 1;

                _card.Expirience = $"{LevelExpirience}|{RankExpirience}";
                _card.LevelRank = $"{Level}.{(int)Rank}";
                _card.Atributes = $"{Toxic}.{Fats}.{Stomach}.{Metabolism}.{FreePoints}";

                MDC.Update(_card);
                MDC.SaveChanges();
            }
            else
            {
                _levelExpirience += expirience;

                _card.Expirience = $"{LevelExpirience}|{RankExpirience}";

                MDC.Update(_card);
                MDC.SaveChanges();
            }
        }
        public void ChangeRankExpirience(float expirience)
        {
            if (_rankExpirience + expirience >= MaxRankExpirience)
            {
                _rankExpirience = (_rankExpirience + expirience) - MaxRankExpirience;
                _rank += 1;

                _card.Expirience = $"{LevelExpirience}|{RankExpirience}";
                _card.LevelRank = $"{Level}.{(int)Rank}";
                _card.Atributes = $"{Toxic}.{Fats}.{Stomach}.{Metabolism}.{FreePoints}";

                MDC.Update(_card);
                MDC.SaveChanges();
            }
            else
            {
                _rankExpirience += expirience;
                _card.Expirience = $"{LevelExpirience}|{RankExpirience}";

                MDC.Update(_card);
                MDC.SaveChanges();
            }
        }
        public void ChangePosition(Directions directions, int[] coordinates)
        {
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
                    break;
            }
            if (coordinates != null)
            {
                _position[0] = coordinates[0];
                _position[1] = coordinates[1];
            }


            if (PositionX > ROOM_MAX) _position[0] -= ROOM_MAX;
            if (PositionY > ROOM_MAX) _position[1] -= ROOM_MAX;

            if (PositionX < ROOM_MIN) _position[0] += ROOM_MAX;
            if (PositionY < ROOM_MIN) _position[1] += ROOM_MAX;

            _card.Position = $"{PositionX}.{PositionY}.{MovementPoints}";

            MDC.Update(_card);
            MDC.SaveChanges();
        }
        public bool CleanUp(float clean, out float rankPoints)
        {
            rankPoints = clean * CLEAN_FACTOR;

            if (_dirty - clean < 0)
            {
                _dirty = 0.0f;
                _card.Dirty = $"{_dirty}";

                MDC.Update(_card);
                MDC.SaveChanges();
                return true;
            }
            else
            {
                _dirty -= clean;
                _card.Dirty = $"{_dirty}";

                MDC.Update(_card);
                MDC.SaveChanges();
                return false;
            }
        }
        public bool AddDamage(float damage, out float expirience, out int cash)
        {
            expirience = damage * EXPIRIENCE_FACTOR;
            cash = (int)(expirience + damage * 1.2f);
            if (cash < 1) cash = 1;

            if (_dirty + damage > MaximumDirty)
            {
                _dirty = MaximumDirty;
                _card.Dirty = $"{_dirty}";

                MDC.Update(_card);
                MDC.SaveChanges();
                return true;
            }
            else
            {
                _dirty += damage;
                _card.Dirty = $"{_dirty}";

                MDC.Update(_card);
                MDC.SaveChanges();
                return false;
            }
        }
        public bool AddDamage(float damage)
        {
            if (damage - Defence > 0) { damage -= Defence; }
            else { damage = 0; }


            if (_dirty + damage > MaximumDirty)
            {
                _dirty = MaximumDirty;
                _card.Dirty = $"{_dirty}";

                MDC.Update(_card);
                MDC.SaveChanges();
                return true;
            }
            else
            {
                _dirty += damage;
                _card.Dirty = $"{_dirty}";

                MDC.Update(_card);
                MDC.SaveChanges();
                return false;
            }
            
        }
        public void TakeMoney(long cash)
        {
            _money += cash;

            

            _card.Money = _money;

            MDC.Update(_card);
            MDC.SaveChanges();
        }
        public long GetMoney(long itemPrice)
        {
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

            _card.Atributes = $"{Toxic}.{Fats}.{Stomach}.{Metabolism}.{FreePoints}";
            MDC.Update(_card);
            MDC.SaveChanges();

            return true;
        }
        public string InventoryData()
        {
            string data = string.Empty;

            for (int i = 0; i < _inventory.Count; i++)
            {
                if (Inventory[i].Name.ToLower() != "ничего")
                {
                    switch (Inventory[i].Type)
                    {
                        case Items.Types.ItemsType.Weapon:
                            data += $"{Environment.NewLine}{i + 1}. {new Weapon(Inventory[i].ItemID).Name} ";
                            data += $"(<b>оружие: {new Weapon(Inventory[i].ItemID).MinDamage} - {new Weapon(Inventory[i].ItemID).MaxDamage}</b>) / " +
                                $"{new Weapon(Inventory[i].ItemID).CriticalFactor}{Environment.NewLine}" +
                                $"<i>Экипировать?</i> /equip{Inventory[i].ItemID}{Environment.NewLine}";
                            break;

                        case Items.Types.ItemsType.Armor:
                            data += $"{Environment.NewLine}{i + 1}. {new Armor(Inventory[i].ItemID).Name} ";
                            data += $"(<b>броня: {new Armor(Inventory[i].ItemID).Defence}</b>){Environment.NewLine}" + 
                                $"<i>Экипировать?</i> /equip{Inventory[i].ItemID}{Environment.NewLine}";
                            break;

                        case Items.Types.ItemsType.Shield:
                            data += $"{Environment.NewLine}{i + 1}. {new Armor(Inventory[i].ItemID).Name} ";
                            data += $"(<b>щит: {new Armor(Inventory[i].ItemID).Defence}</b>){Environment.NewLine}" + 
                                $"<i>Экипировать?</i> /equip{Inventory[i].ItemID}{Environment.NewLine}";
                            break;

                        case Items.Types.ItemsType.Helmet:
                            data += $"{Environment.NewLine}{i + 1}. {new Armor(Inventory[i].ItemID).Name} ";
                            data += $"(<b>шляпа: {new Armor(Inventory[i].ItemID).Defence}</b>){Environment.NewLine}" + 
                                $"<i>Экипировать?</i> /equip{Inventory[i].ItemID}{Environment.NewLine}";
                            break;

                        case Items.Types.ItemsType.Boots:
                            data += $"{Environment.NewLine}{i + 1}. {Inventory[i].Name} ";
                            data += $"(<b>тапки: {new Armor(Inventory[i].ItemID).Defence}</b>){Environment.NewLine}" + 
                                $"<i>Экипировать?</i> /equip{Inventory[i].ItemID}{Environment.NewLine}";
                            break;

                        case Items.Types.ItemsType.Gloves:
                            data += $"{Environment.NewLine}{i + 1}. {Inventory[i].Name} ";
                            data += $"(<b>варежки: {new Armor(Inventory[i].ItemID).Defence}</b>){Environment.NewLine}" + 
                                $"<i>Экипировать?</i> /equip{Inventory[i].ItemID}{Environment.NewLine}";
                            break;

                        case Items.Types.ItemsType.HealPotion:
                            data += $"{Environment.NewLine}{i + 1}. {new HealingPotion(Inventory[i].ItemID).Name} ";
                            data += $"(<b>чистящее средство: {new HealingPotion(Inventory[i].ItemID).EffectValue}</b>){Environment.NewLine}" + 
                                $"<i>Использовать?</i> /use{Inventory[i].ItemID}{Environment.NewLine}";
                            break;

                        case Items.Types.ItemsType.OtherPotion:
                            data += $"{Environment.NewLine}{i + 1}. {Inventory[i].Name} ";
                            data += $"(<b>какая-то химия, можно пить</b>){Environment.NewLine}" + 
                                $"<i>Использовать?</i> /use{Inventory[i].ItemID}{Environment.NewLine}";
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

        public void FullAttack(out float heroDamage, out float weaponDamage, out float weaponBonus)
        {
            heroDamage = ClearAttack();
            weaponDamage = _weapon.RealDamage;
            weaponBonus = 0.0f;
        }

        public void FullDefence(out float heroDefence, out float armorDefence, out float armorBonus)
        {
            heroDefence = ClearDefence();
            armorDefence = _armor.Defence;
            armorBonus = 0.0f;
        }
    }
} 