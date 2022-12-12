using ToilettenArbitrator.ToilettenWars.Items.Types;

namespace ToilettenArbitrator.ToilettenWars.Items.Interfaces
{
    public interface IWeapon
    {
        float MinDamage { get; }
        float MaxDamage { get; }
        float RealDamage { get; }
        float CriticalFactor { get; }
        WeaponType Type { get; }
        bool Critical { get; }
        int Fractions { get; }

    }
}
