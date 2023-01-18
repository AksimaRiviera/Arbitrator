using ToilettenArbitrator.Brain;
using ToilettenArbitrator.ToilettenWars.Person;
using Windows.UI.Xaml.Documents;

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
        private string cleanNotification;

        public string AttackNotification => Attack();
        public string CleanUpNotification => Clean();

        public Arena(string heroName, string enemyName)
        {
            using (MembersDataContext MemberArchive = new MembersDataContext())
            {
                heroes = MemberArchive.HeroCards.ToList();
            }
            this.heroName = heroName.ToLower();
            this.enemyName = enemyName.ToLower();
        }

        private string Clean()
        {
            hero = new Hero(heroes.Find(name => name.Name.Contains(heroName)));
            float rankExpirience;

            if (heroes.Find(person => person.Name.Contains(enemyName)) != null)
            {
                enemy = new Hero(heroes.Find(name => name.Name.Contains(enemyName)));

                if (string.IsNullOrEmpty(attackNotification) || string.IsNullOrWhiteSpace(attackNotification)) attackNotification = "";

                float atk = hero.Attack;

                enemy.CleanUp(atk, out rankExpirience);

                hero.ChangeRankExpirience((float)Math.Round(rankExpirience, 2));

                cleanNotification = @$"&#9888 <b>A C H T U N G</b> &#9888{Environment.NewLine}" +
                    $"@{hero.Name} " + "почистил" + $" {enemy.Name}{Environment.NewLine}{Environment.NewLine}" +
                    $"<i><b>Статистика чистки</b></i>{Environment.NewLine}" +
                    $"@{enemy.Name} {helloSynapse.GettingVerb} " +
                    $"{string.Format("{0:f2}", atk)} чистоты{Environment.NewLine}" +
                    $"@{hero.Name} получает {string.Format("{0:f2}", rankExpirience)} рангового опыта";
                return cleanNotification;
            }
            else
            {
                return "Твоего противника не существует";
            }

        }

        private string Attack()
        {
            hero = new Hero(heroes.Find(name => name.Name.Contains(heroName)));

            if (heroes.Find(person => person.Name.Contains(enemyName)) != null)
            {
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

                    attackNotification = @$"&#9888 <b>A C H T U N G</b> &#9888{Environment.NewLine}" +
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
                    clearDamage = atk * 0.01f;
                    enemy.AddDamage(clearDamage, out expirience, out cash);

                    hero.ChangeLevelExpirience((float)Math.Round(expirience, 2));
                    hero.TakeMoney(cash);

                    attackNotification = @$"&#9888 <b>A C H T U N G</b> &#9888{Environment.NewLine}" +
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
            }
            else
            {
                return "Твоего противника не существует";
            }
        }
    }
}