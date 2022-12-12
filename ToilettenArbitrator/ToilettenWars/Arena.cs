using ToilettenArbitrator.Brain;
using ToilettenArbitrator.ToilettenWars.Person;

namespace ToilettenArbitrator.ToilettenWars
{
    internal class Arena
    {
        private float expirience;
        private int cash;

        private string heroName, enemyName;
        private List<HeroCard> heroes;

        private Hero hero;
        private Hero enemy;

        private HelloSynapse helloSynapse = new HelloSynapse();
        private string attackNotification;

        public string AttackNotification => Attack();

        public Arena(string heroName, string enemyName)
        {
            using (MembersDataContext MemberArchive = new MembersDataContext())
            {
                heroes = MemberArchive.HeroCards.ToList();
            }
            this.heroName = heroName.ToLower();
            this.enemyName = enemyName.ToLower();
        }

        private string Attack()
        {
            hero = new Hero(heroes.Find(name => name.Name.Contains(heroName)));
            enemy = new Hero(heroes.Find(name => name.Name.Contains(enemyName)));

            if (string.IsNullOrEmpty(attackNotification) || string.IsNullOrWhiteSpace(attackNotification)) attackNotification = "";

            float clearDamage;
            float atk = hero.Attack, def = enemy.Defence;

            if (atk >= def)
            {
                clearDamage = atk - def;

                enemy.AddDamage(clearDamage, out expirience, out cash);

                hero.ChangeLevelExpirience((float)Math.Round(expirience, 2));
                hero.TakeMoney(cash);

                attackNotification = @$"&#9888 ACHTUNG &#9888{Environment.NewLine}" +
                    $"@{hero.Name} " + "&#9876" + $" {enemy.Name}{Environment.NewLine}{Environment.NewLine}" +
                    $"<i><b>Статистика боя</b></i>{Environment.NewLine}" +
                    $"@{enemy.Name} {helloSynapse.GettingVerb} " +
                    $"{string.Format("{0:f2}", clearDamage)} урона{Environment.NewLine}" +
                    $"{string.Format("{0:f2}", atk)} - " +
                    $"{string.Format("{0:f2}", def)} = " +
                    $"{string.Format("{0:f2}", clearDamage)}{Environment.NewLine}" +
                    $"@{hero.Name} получает {string.Format("{0:f2}", expirience)} опыта и {cash} ГовноТенге";
                return attackNotification;
            }
            else
            {
                return "Урон абсорбирован!";
            }
        }
    }
}