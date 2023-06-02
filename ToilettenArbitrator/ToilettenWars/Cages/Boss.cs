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
        private float _ageFactor;
        private float _levelFactor;
        protected AgeRanks _ageRank;
        protected LevelRanks _levelRank;

        private string _status;
        private string[] _illData = new string[] {
            "golova", "zlato"
        };

        // _lootArgs
        // [0] - тип возраста
        // [1] - тип опытности
        // [2] - базовый опыт за моба 
        // [3] - базовые бабки за моба
        // [4] - базовый ранговый опыт
        // [5] - кол-во возможных вещей
        // всё остальное id-шники возможных вещей
        private string[] _lootArgs;

        private Zoo.Zones _bossDirection;
        private string _zoneMark;
        private BossZoneMod _bossZoneMod;

        private readonly Dictionary<string, float> Factors = new Dictionary<string, float>(6) {
            { "Young", 2.3f },
            { "Acient", 3.0f },
            { "Relict", 3.9f },
            { "Ordinary", 1.9f },
            { "Experienced", 2.3f },
            { "Mature", 2.8f }
        };

        private readonly Dictionary<string, string> Icons = new Dictionary<string, string>(6) {
            { "A_Brain", "&#129504" },
            { "B_Brain", "&#129504 &#129504" },
            { "C_Brain", "&#129504 &#129504 &#129504" },
            { "Young", "&#128118" },
            { "Acient", "&#128104" },
            { "Relict", "&#128116" }
        };

        protected enum BossZoneMod
        {
            Hot,
            Cold,
            Acid,
            Jivex,
            Light,
            Dark

        }

        public string Status => _status;
        public float Damage => _damage;
        public float Defence => _defence;
        public float HitPoints => _hitPoints;
        public int PositionX => _positionX;
        public int PositionY => _positionY;
        public float MaximumHitPoints => _maximumHitPoints;

        public Boss(MobCard Card) : base(Card)
        {
        }

        public Boss(string id) : base(id)
        {

        }
    }
}