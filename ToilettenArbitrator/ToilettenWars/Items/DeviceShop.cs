using System.Text;
using ToilettenArbitrator.ToilettenWars.Items.Interfaces;
using ToilettenArbitrator.ToilettenWars.Items.Types;

namespace ToilettenArbitrator.ToilettenWars.Items
{
    public class DeviceShop
    {
        private List<Item> AllItems;
        private List<ItemCard> itemCards;

        public Item OneItem { get; }
        public string ShopInfo => ShopCreate();

        public DeviceShop()
        {
            AllItems = new List<Item>();

            using (MembersDataContext MemberArchive = new MembersDataContext())
            {
                itemCards = MemberArchive.ItemCards.ToList();
            }

            for (int i = 0; i < itemCards.Count; i++)
            {
                AllItems.Add(new Item(itemCards[i]));
            }

        }

        private string ShopCreate()
        {
            StringBuilder ShopLog = new StringBuilder("ВАС ПРИВЕТСТВУЕТ\r\n\n\r    <b>SHIT SHOP</b>\r\n\r\nВ асортементе:\r\n\r\n");
            int count = 1;

            ShopLog.AppendLine($"Чем ударить:{Environment.NewLine}");

            for (int i = 0; i < AllItems.Count; i++)
            {
                if (AllItems[i].Type != ItemsType.Weapon) continue;

                ShopLog.AppendLine($"{count++}. {AllItems[i].Name}{Environment.NewLine}" +
                    $"Урон: {new Weapon(itemCards[i]).MinDamage} - {new Weapon(itemCards[i]).MaxDamage}" +
                    $"{Environment.NewLine}{AllItems[i].Coast} Говнотенге{Environment.NewLine}" +
                    $"/buy{AllItems[i].ItemID}{Environment.NewLine}");
            }

            ShopLog.AppendLine($"Чем прикрыться:{Environment.NewLine}");
            count = 1;

            for (int i = 0; i < AllItems.Count; i++)
            {
                if (AllItems[i].Type != ItemsType.Armor && AllItems[i].Type != ItemsType.Helmet && AllItems[i].Type != ItemsType.Shield) continue;
                
                ShopLog.AppendLine($"{count++}. {AllItems[i].Name}{Environment.NewLine}" +
                    $"Защита: {new Armor(itemCards[i]).Defence}" +
                    $"{Environment.NewLine}{AllItems[i].Coast} Говнотенге{Environment.NewLine}" +
                    $"/buy{AllItems[i].ItemID}{Environment.NewLine}");
            }

            ShopLog.AppendLine($"Чем полечиться:{Environment.NewLine}");
            count = 1;

            for (int i = 0; i < AllItems.Count; i++)
            {
                if (AllItems[i].Type != ItemsType.HealPotion) continue;

                ShopLog.AppendLine($"{count++}. {AllItems[i].Name}{Environment.NewLine}" +
                    $"Будеш чище на: {new HealingPotion(itemCards[i]).EffectValue}{Environment.NewLine}" +
                    $"{AllItems[i].Coast} Говнотенге{Environment.NewLine}" +
                    $"/buy{AllItems[i].ItemID}{Environment.NewLine}");
            }

            ShopLog.AppendLine("<i>SHIT SHOP - ГОВНА НЕ ДЕРЖИМ!</i>");

            return ShopLog.ToString();
        }
        //public string ShopInfo(ItemsType type)
        //{
        //    switch (type)
        //    {
        //        case ItemsType.Weapon:
        //            break;
        //        case ItemsType.Armor:
        //            break;
        //        case ItemsType.Shield:
        //            break;
        //        case ItemsType.Helmet:
        //            break;
        //        case ItemsType.Boots:
        //            break;
        //        case ItemsType.Gloves:
        //            break;
        //        case ItemsType.HealPotion:
        //            break;
        //        case ItemsType.OtherPotion:
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}
