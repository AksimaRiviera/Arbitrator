using ToilettenArbitrator.ToilettenWars.Cages;
using ToilettenArbitrator.ToilettenWars.Person;

namespace ToilettenArbitrator.ToilettenWars
{
    public class Zoo
    {
        private List<MobCard> _mobsData = new List<MobCard>();
        private List<SimpleMob> _simples = new List<SimpleMob>();
        private List<Boss> _bosses = new List<Boss>();

        private int _heroPositionX, _heroPositionY;

        public int HeroPositionX { set => _heroPositionX = value; }
        public int HeroPositionY { set => _heroPositionY = value; }

        public SimpleMob OneMob => GetSimpleMob();
        public List<SimpleMob> NearMobs => GetNearMobs();

        public Zoo()
        {
            using MembersDataContext MDC = new MembersDataContext();
            _mobsData = MDC.MobCards.ToList();

            for (int i = 0; i < 10; i++)
            {
                _simples[i] = new SimpleMob(_mobsData[0]);
                _simples[i + 10] = new SimpleMob(_mobsData[1]);
                _simples[i + 20] = new SimpleMob(_mobsData[2]);
            }

            _bosses.Add(new Boss(_mobsData[0]));
            _bosses.Add(new Boss(_mobsData[1]));
            _bosses.Add(new Boss(_mobsData[2]));    
        }

        private SimpleMob GetSimpleMob()
        {
            return _simples[new Random().Next(_simples.Count)];
        }

        private List<SimpleMob> GetNearMobs()
        {
            List<SimpleMob> mobs = new List<SimpleMob>(5);
            int counter = 0;
            for (int i = 0; counter < mobs.Count; i++)
            {
                if (i > _simples.Count) break;

                if (_simples[i].PositionX < _heroPositionX + 5 &&
                    _simples[i].PositionX > _heroPositionX - 5 &&
                    _simples[i].PositionY < _heroPositionY + 5 &&
                    _simples[i].PositionY > _heroPositionY - 5)
                {
                    mobs.Add(_simples[i]);
                    counter++;
                } 
            }
            return mobs;
        }
    }
}
