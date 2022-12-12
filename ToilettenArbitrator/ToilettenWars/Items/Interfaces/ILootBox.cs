namespace ToilettenArbitrator.ToilettenWars.Items.Interfaces
{
    public interface ILootBox
    {
        float Expirience { get; }
        long Cash { get; }
        List<Item> Items { get; }
    }
}