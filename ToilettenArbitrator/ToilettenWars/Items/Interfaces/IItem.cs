using ToilettenArbitrator.ToilettenWars.Items.Types;

namespace ToilettenArbitrator.ToilettenWars.Items.Interfaces
{
    public interface IItem
    {
        long ID { get; }
        string Name { get; }
        string Description { get; }
        long Coast { get; }
        ItemsType Type { get; }
    }
}
