using ToilettenArbitrator.ToilettenWars.Items.Interfaces;
using ToilettenArbitrator.ToilettenWars.Items.Types;
using Uno.UI.DataBinding;

namespace ToilettenArbitrator.ToilettenWars.Items
{
    public class LootBox : ILootBox
    {
        private const float NON_EXP_FACTOR = 0.0f;
        private const int NON_CASH_FACTOR = 0;
        private const float STANDART_EXP_FACTOR = 1.0f;
        private const int STANDART_CASH_FACTOR = 1;
        private const float STANDART_EXP_VALUE = 0.01f;
        private const int STANDART_CASH_VALUE = 1;
        private const float STANDART_RANK_POINTS = 0.01f;

        private readonly string[] _standartWeaponID = {
            "vetosh" };

        private readonly string[] _standartArmorID = {
            "rabrob" };

        private float _exp;
        private float _expFactor;
        private float _rankPoints;

        private long _cash;
        private int _cashFactor;
        private int _totalMobs;
        private int _ageRank;
        private int _levelRank;

        // [0] - тип возраста
        // [1] - тип опытности
        // [2] - базовый опыт за моба 
        // [3] - базовые бабки за моба
        // [4] - базовый ранговый опыт
        // [5] - кол-во возможных вещей
        // всё остальное id-шники возможных вещей
        private string[] _mobsArgs;
        private List<Item> _itemsForWin;

        private List<Item> _items;
        private LootType _type;

        public enum LootType
        {
            Standart,
            Rich,
            Rare,
            Unique,
            Money,
            Expirience,
            MobFactor
        }

        public float Expirience => _exp;
        public float RankPoints => _rankPoints;
        public long Cash => _cash;
        public List<Item> Items => _items;
        public LootType Type => _type;

        public LootBox(string[] questPrize, int total)
        {
            _type = LootType.Standart;
            _items = new List<Item>();
            // questPrize
            // [0] - экспа (float)
            // [1] - бабло (long)
            // [2] - ранговая экспа (float)
            // [3] - колличество мобов
            _exp = (float.Parse(questPrize[0]) * total);
            _cash = (long)(float.Parse(questPrize[1]) * total);
            _rankPoints = (float.Parse(questPrize[2]) * total);
        }

        public LootBox(LootType type, string[] mobArgs)
        {
            _type = type;
            _mobsArgs = mobArgs;
            RandomizePrize(_mobsArgs);
        }

        public LootBox(LootType type)
        {
            _rankPoints = STANDART_RANK_POINTS;
            _exp = STANDART_EXP_VALUE;
            _cash = STANDART_CASH_VALUE;
            _type = type;
            RandomizePrize();
        }

        public LootBox()
        {
            _rankPoints = STANDART_RANK_POINTS;
            _exp = STANDART_EXP_VALUE - 0.04f;
            _cash = STANDART_CASH_VALUE;
            _type = default;
            RandomizePrize();
        }

        private void RandomizePrize()
        {
            _items = new List<Item>();

            switch (_type)
            {
                case LootType.Standart:
                    _exp *= STANDART_EXP_FACTOR;
                    _cash *= STANDART_CASH_FACTOR;
                    break;

                case LootType.Rich:
                    _exp *= STANDART_EXP_FACTOR + 1.5f;
                    _cash *= STANDART_CASH_FACTOR + 1;
                    break;

                case LootType.Rare:
                    _exp *= STANDART_EXP_FACTOR + 3;
                    _cash *= STANDART_CASH_FACTOR + 2;
                    break;

                case LootType.Unique:
                    _exp *= STANDART_EXP_FACTOR + 5;
                    _cash *= STANDART_CASH_FACTOR + 4;
                    break;

                case LootType.Money:
                    _exp = NON_EXP_FACTOR;
                    _cash *= STANDART_CASH_FACTOR + 3;
                    break;

                case LootType.Expirience:
                    _exp *= STANDART_EXP_FACTOR + 2.5f;
                    _cash = STANDART_CASH_FACTOR;
                    break;

                case LootType.MobFactor:
                    break;

                default:
                    _exp = NON_EXP_FACTOR;
                    _cash = STANDART_CASH_FACTOR;
                    break;
            }
        }

        private void RandomizePrize(string[] mobArgs)
        {
            _ageRank = int.Parse(mobArgs[0]);
            _levelRank = int.Parse(mobArgs[1]);
            _exp = float.Parse(mobArgs[2]);
            _cash = long.Parse(mobArgs[3]);
            _rankPoints = float.Parse(mobArgs[4]);
            _items = new List<Item>(int.Parse(mobArgs[5]));

            _expFactor = STANDART_EXP_FACTOR;
            _cashFactor = STANDART_CASH_FACTOR;
            _rankPoints = STANDART_RANK_POINTS;

            switch (_levelRank)
            {
                case 1:
                    _expFactor += 0.1f;
                    _cashFactor += 1;
                    _rankPoints *= 1.1f;
                    break;
                case 2:
                    _expFactor += 0.17f;
                    _cashFactor += 2;
                    _rankPoints *= 1.12f;
                    break;
                case 3:
                    _expFactor += 0.2f;
                    _cashFactor += 3;
                    _rankPoints *= 1.16f;
                    break;
                default:
                    _expFactor += 0.1f;
                    _cashFactor += 1;
                    _rankPoints *= 1.1f;

                    break;
            }

            switch (_ageRank)
            {
                case 1:
                    _expFactor += 0.2f;
                    _cashFactor += 2;
                    _rankPoints *= 1.25f;
                    break;
                case 2:
                    _expFactor += 0.53f;
                    _cashFactor += 4;
                    _rankPoints *= 1.35f;
                    break;
                case 3:
                    _expFactor += 0.98f;
                    _cashFactor += 6;
                    _rankPoints *= 1.4f;
                    break;
                default:
                    _expFactor += 0.2f;
                    _cashFactor += 2;
                    _rankPoints *= 1.25f;
                    break;
            }

            _exp *= _expFactor; 
            _cash *= _cashFactor;

            for (int i = 0; i < _items.Count; i++)
            {
                switch (mobArgs[i + 1])
                {
                    case "1":
                        _itemsForWin.Add(new Item("vlasal"));
                        break;

                    case "2":
                        _itemsForWin.Add(new Item("svejok"));
                        break;

                    default:
                        break;
                }
            }
        }

        private Item RandomizeItem(ItemsType type)
        {
            switch (type)
            {
                case ItemsType.Weapon:
                    return new Item(_standartWeaponID[new Random().Next(_standartWeaponID.Length)]);

                case ItemsType.Armor:
                    return new Item(_standartArmorID[new Random().Next(_standartArmorID.Length)]);

                case ItemsType.Shield:
                    return new Item(_standartArmorID[new Random().Next(_standartArmorID.Length)]);

                case ItemsType.Helmet:
                    return new Item(_standartArmorID[new Random().Next(_standartArmorID.Length)]);

                case ItemsType.Boots:
                    return new Item(_standartArmorID[new Random().Next(_standartArmorID.Length)]);

                case ItemsType.Gloves:
                    return new Item(_standartArmorID[new Random().Next(_standartArmorID.Length)]);

                case ItemsType.HealPotion:
                    return new Item(_standartArmorID[new Random().Next(_standartArmorID.Length)]);

                case ItemsType.OtherPotion:
                    return new Item(_standartArmorID[new Random().Next(_standartArmorID.Length)]);

                default:
                    return new Item(_standartArmorID[new Random().Next(_standartArmorID.Length)]);
            }
        }

        private void ItemBox(string[] Ids)
        {
            if (Ids == null) _items.Add(new Item());

            for (int i = 0; i < Ids.Length; i++)
            {
                _items.Add(new Item(Ids[i]));
            }
        }
    }
}