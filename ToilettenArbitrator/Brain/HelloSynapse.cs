namespace ToilettenArbitrator.Brain
{
    internal class HelloSynapse
    {
        private string[] _greatWords = {
            "Великолепно", "Замечательно", "Прекрасно", "Восторг" 
        };

        private string[] _helloWords = {
            "Привет ", "Здорово ", "Вечер в хату ", "Доброго времени суток ",
            "Дороу ", "И вам не хворать ", "Моё уважение "
        };

        private string[] _helloSmiles = {
            "&#129321","&#128521","&#129303","&#128075",
            "&#128400","&#9996","&#128080","&#10024","&#127881"
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
            "Метнулся кабанчиком ", "Пятится как краб на галерах " 
        };

        private string[] _randomSpawnMessage = new string[] {
            "Преодолевая водные преграды ", "Отпллёвываясь и сморкаясь ", "Сквозь вонь и пыль "
        };

        private string[] _goVerbs = new string[] {
            "Дёрнулся ", "Перекатился ", "Пробежал ",
            "Дополз ", "Дотошнил ", "Переместился ",
            "Прошлёпал ", "Прошаркал " 
        };

        private string[] _screemWords = new string[] {
            "С криком", "Булькая", "Причмокивая", "Переливая воду",
            "Встав в позу буквы Т", "Доставая зайца из шляпы" 
        };

        private string[] _screemModulateWords = new string[] {
            "Рассыпаясь на атомы", "Превращаясь в песок", "Дезинтегрируясь",
            "Уменьшившись в размерах"
        };

        private Random random = new Random();

        public HelloSynapse()
        {
        }

        public string Hello => GetHello();
        public string HelloSmile => GetHelloSmile();
        public string AttackVerb => GetAttackVerb();
        public string GettingVerb => GetGettingVerb();
        public string ScreemWords => GetScreemWords();
        public string ScreemModulateWords => GetScreemModulateWords();
        public string GreatWords => GetGreatWords();

        private string GetHello()
        {
            return _helloWords[new Random().Next(_helloWords.Length)];
        }

        private string GetHelloSmile()
        {
            return _helloSmiles[new Random().Next(_helloSmiles.Length)];
        }

        private string GetAttackVerb()
        {
            return _attackVerbs[new Random().Next(_attackVerbs.Length)];
        }

        private string GetGettingVerb()
        {
            return _gettingVerbs[new Random().Next(_gettingVerbs.Length)];
        }

        private string GetScreemWords()
        {
            return _screemWords[new Random().Next(_screemWords.Length)];
        }

        private string GetScreemModulateWords()
        {
            return _screemModulateWords[new Random().Next(_screemModulateWords.Length)];
        }

        private string GetGreatWords()
        {
            return _greatWords[new Random().Next(_greatWords.Length)];
        }
    }
}
