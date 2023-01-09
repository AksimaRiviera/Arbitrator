using ToilettenArbitrator.ToilettenWars.Items;

namespace ToilettenArbitrator.ToilettenWars.Person
{
    public interface IHero
    {
        int ID { get; }
        string Name { get; }
        int Level { get; }
        int Toxic { get; }
        int Fats { get; }
        int Stomach { get; }
        int Metabolism { get; }
        int FreePoints { get; }
        float Dirty { get; }
        int PositionX { get; }
        int PositionY { get; }
        int MovementPoints { get; }
        float Attack { get; }
        float Defence { get; }
        float LevelExpirience { get; }
        float RankExpirience { get; }
        int MaxLevelExpirience { get; }
        long Money { get; }

        Ranks Rank { get; }
        Weapon Weapon { get; }
        Armor Armor { get; }
        Armor Helmet { get; }
        Armor Shield { get; }

        void TakeMoney(long cash);
        bool UpdateCharacteristics(Hero.Characteristics characteristics);
        void ChangeLevelExpirience(float expirience);
        void ChangeRankExpirience(float expirience);
        void ChangePosition(Hero.Directions directions, int[] coordinates);
        bool AddDamage(float damage, out float expirience, out int cash);
        bool AddDamage(float damage);
        void FullAttack(out float heroDamage, out float weaponDamage, out float weaponBonus);
        void FullDefence(out float heroDefence, out float armorDefence, out float armorBonus);
    }
}