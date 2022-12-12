using ToilettenArbitrator.ToilettenWars.Items.Types;

namespace ToilettenArbitrator.ToilettenWars.Items.Interfaces
{
    internal interface IArmor : IItem
    {
        float Defence { get; }
        ArmorType Type { get; }
    }
}
