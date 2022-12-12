using ToilettenArbitrator.ToilettenWars.Items.Interfaces;

namespace ToilettenArbitrator.ToilettenWars.Items
{
    public class LootBox : ILootBox
    {
        private const float NON_EXP_FACTOR = 0.0f;
        private const float STANDART_EXP_FACTOR = 1.0f;
        private const int STANDART_CASH_FACTOR = 0;

        private float _expFactor;
        private int _cashFactor;


        private float _exp;
        private long _cash;
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
        public long Cash => _cash;
        public List<Item> Items => _items;
        public LootType Type => _type;

        public LootBox(LootType type, string[] mobArgs)
        {
            _type = type;
            RandomizeItem();
        }

        public LootBox(LootType type)
        {
            _type = type;
            RandomizeItem();
        }

        public LootBox()
        {
            _type = default;
            RandomizeItem();
        }

        private void RandomizeItem()
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
    }
}