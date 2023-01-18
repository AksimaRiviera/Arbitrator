using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToilettenArbitrator.ToilettenWars.Items;
using ToilettenArbitrator.ToilettenWars.Person;
using Windows.UI.Xaml.Controls;

namespace ToilettenArbitrator.ToilettenWars.Cages
{
    public class Boss : Mob
    {
        protected AgeRanks _ageRank;
        protected LevelRanks _levelRank;

        private Zoo.Zones _bossDirection;
        private string _zoneMark;
        private string[] _illData;
        private BossZoneMod _bossZoneMod;

        protected enum BossZoneMod
        {
            Hot,
            Cold,
            Acid,
            Jivex,
            Light,
            Dark

        }

        public Boss(MobCard Card) : base(Card)
        {

        }

        public Boss(string id) : base(id)
        {

        }

        protected override void MobSettings()
        {
            if (_positionX >= Zoo.RED_ZONE_COORDINATES[0] && _positionX < Zoo.RED_ZONE_COORDINATES[1] &&
                _positionY >= Zoo.RED_ZONE_COORDINATES[2] && _positionY < Zoo.RED_ZONE_COORDINATES[3])
            {
                _bossZoneMod = BossZoneMod.Hot;
                _zoneMark = Zoo.RED_ZONE;
                _bossDirection = Zoo.Zones.Red;
                
            }
            else if (_positionX >= Zoo.BLUE_ZONE_COORDINATES[0] && _positionX < Zoo.BLUE_ZONE_COORDINATES[1] &&
                     _positionY >= Zoo.BLUE_ZONE_COORDINATES[2] && _positionY < Zoo.BLUE_ZONE_COORDINATES[3])
            {
                _bossZoneMod = BossZoneMod.Cold;
                _zoneMark = Zoo.BLUE_ZONE;
                _bossDirection = Zoo.Zones.Blue;
            }
            else if (_positionX >= Zoo.GREEN_ZONE_COORDINATES[0] && _positionX < Zoo.GREEN_ZONE_COORDINATES[1] &&
                    _positionY >= Zoo.GREEN_ZONE_COORDINATES[2] && _positionY < Zoo.GREEN_ZONE_COORDINATES[3])
            {
                _bossZoneMod = BossZoneMod.Acid;
                _zoneMark = Zoo.GREEN_ZONE;
                _bossDirection = Zoo.Zones.Green;
            }
            else if (_positionX >= Zoo.PURPLE_ZONE_COORDINATES[0] && _positionX < Zoo.PURPLE_ZONE_COORDINATES[1] &&
                    _positionY >= Zoo.PURPLE_ZONE_COORDINATES[2] && _positionY < Zoo.PURPLE_ZONE_COORDINATES[3])
            {
                _bossZoneMod = BossZoneMod.Jivex;
                _zoneMark = Zoo.PURPLE_ZONE;
                _bossDirection = Zoo.Zones.Purple;
            }
            else if (_positionX >= Zoo.BLACK_ZONE_COORDINATES[0] && _positionX < Zoo.BLACK_ZONE_COORDINATES[1] &&
                    _positionY >= Zoo.BLACK_ZONE_COORDINATES[2] && _positionY < Zoo.BLACK_ZONE_COORDINATES[3])
            {
                _bossZoneMod = BossZoneMod.Dark;
                _zoneMark = Zoo.BLACK_ZONE;
                _bossDirection = Zoo.Zones.Black;
            }
            else
            {
                _bossZoneMod = BossZoneMod.Light;
                _zoneMark = Zoo.WHITE_ZONE;
                _bossDirection = Zoo.Zones.White;
            };
            
        }

        protected override void LootSettings()
        {
            throw new NotImplementedException();
        }

        public override bool AddDamage(float damage, out LootBox Loot)
        {

            throw new NotImplementedException();
        }

        public override Ill Infect()
        {
            if (new SilverDice().Luck(15)) return new Ill(_illData[new Random().Next(_illData.Length)]);
            else return new Ill();
        }
    }
}