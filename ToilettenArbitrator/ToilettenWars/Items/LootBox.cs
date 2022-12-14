using ToilettenArbitrator.ToilettenWars.Items.Interfaces;
using ToilettenArbitrator.ToilettenWars.Items.Types;

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
        private const float STANDART_RANK_POINTS = 0.25f;

        private readonly string[] _standartWeaponID = { 
            "vetosh" };

        private readonly string[] _standartArmorID = { 
            "rabrob" };

        private float _exp; 
        private float _expFactor;
        private float _rankPoints;

        private long _cash;
        private int _cashFactor;

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

        public LootBox(LootType type, string[] mobArgs)
        {
            _type = type;
            RandomizeItems(mobArgs);
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
            _exp = float.Parse(mobArgs[0]);
            _cash = long.Parse(mobArgs[1]);
            _rankPoints = float.Parse(mobArgs[2]);
            _items = new List<Item>(int.Parse(mobArgs[3]));

            for (int i = 0; i < int.Parse(mobArgs[0]); i++)
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

        private Item RandomItem(ItemsType type)
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
    }
}