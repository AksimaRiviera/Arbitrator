using ToilettenArbitrator.ToilettenWars.Items.Types;

namespace ToilettenArbitrator.ToilettenWars.Items.Interfaces
{
    internal interface IPotion : IItem
    {
        float EffectValue { get; }
        bool IsPercent { get; }
        PotionType PotionType { get; }
    }
}
