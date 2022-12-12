namespace ToilettenArbitrator.Brain
{
    internal class HelloSynapse
    {
        private string[] _helloWords = {
            "Привет ", "Здорово ", "Вечер в хату ", "Доброго времени суток ",
            "Дороу ", "И вам не хворать ", "Моё уважение "
        };

        private string[] _gettingVerbs = {
            "получил", "всосал", "сожрал",
            "сглотнул", "принял", "нахватал"
        };

        private string[] _attackVerbs = {
            "нанёс", "шмякнул", "залепил", "обмазал",
            "закинул", "вколотил", "отвесил"
        };

        private string[] _startMessageConstruct = new string[] {
            "В конвульсиях ", "На подкашивающихся ногах ",
            "В диком угаре ","Дико вопя ","Пылая алыми глазами ",
            "Извергая желч ", "Неся чушь ", "Бормоча что-то под нос ",
            "Метнулся кабанчиком ", "Пятится как краб на галерах " };

        private string[] _randomSpawnMessage = new string[] {
            "Преодолевая водные преграды ", "Отпллёвываясь и сморкаясь ", "Сквозь вонь и пыль "};

        private string[] _goVerbs = new string[] {
            "Дёрнулся ", "Перекатился ", "Пробежал ",
            "Дополз ", "Дотошнил ", "Переместился ",
            "Прошлёпал ", "Прошаркал " };

        private Random random = new Random();

        public HelloSynapse()
        {
        }

        public string Hello => GetHello();
        public string AttackVerb => GetAttackVerb();
        public string GettingVerb => GetGettingVerb();

        private string GetHello()
        {
            return _helloWords[new Random().Next(_helloWords.Length)];
        }

        private string GetAttackVerb()
        {
            return _attackVerbs[new Random().Next(_attackVerbs.Length)];
        }

        private string GetGettingVerb()
        {
            return _gettingVerbs[new Random().Next(_gettingVerbs.Length)];
        }
    }
}
