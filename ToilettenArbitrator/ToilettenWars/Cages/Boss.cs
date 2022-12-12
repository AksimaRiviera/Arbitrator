using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToilettenArbitrator.ToilettenWars.Items;

namespace ToilettenArbitrator.ToilettenWars.Cages
{
    public class Boss : Mob
    {
        public Boss(MobCard Card) : base(Card)
        {

        }

        public override bool AddDamage(float damage, out LootBox Loot)
        {
            throw new NotImplementedException();
        }
    }
}
