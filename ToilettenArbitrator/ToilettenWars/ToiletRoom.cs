using ToilettenArbitrator.ToilettenWars.Person;

namespace ToilettenArbitrator.ToilettenWars
{
    public class ToiletRoom
    {
        private List<HeroCard> heroes;
        private Hero _hero;

        private Random random = new Random();
        private string _message, _fullName, _userName, _firstName, _lastName;

        private string[] _startMessageConstruct = new string[] {
            "В конвульсиях ", "На подкашивающихся ногах ",
            "В диком угаре ","Дико вопя ","Пылая алыми глазами ",
            "Извергая желч ", "Неся чушь ", "Бормоча что-то под нос ",
            "Метнувшись кабанчиком ", "Пятясь как краб на галерах " };

        private string[] _randomSpawnMessage = new string[] {
            "Преодолевая водные преграды ", "Отпллёвываясь и сморкаясь ", "Сквозь вонь и пыль " };

        private string[] _goVerbs = new string[] {
            "Дёрнулся ", "Перекатился ", "Пробежал ",
            "Дополз ", "Дотошнил ", "Переместился ",
            "Прошлёпал ", "Прошаркал " };

        private string[] _heroStatusIco = new string[] {
            "&#128081", "&#128519"
        };

        private string[] _heroIllsMark = new string[] {
            "&#129712 &#128315", "&#128170 &#128315", "&#129504 &#128315",
        };

        private string[] _dataMass = new string[2];

        

        private const int X_MAXIMUM = 200;
        private const int X_MINIMUM = 0;

        private const int Y_MAXIMUM = 200;
        private const int Y_MINIMUM = 0;

        private int[] _coordinates = new int[2];

        public ToiletRoom()
        {

        }

        public ToiletRoom(string userName, string firstName, string secondName)
        {
            using (MembersDataContext MemberArchive = new MembersDataContext())
            {
                heroes = MemberArchive.HeroCards.ToList();
            }

            if (userName != null)
            {
                _hero = new Hero(heroes.Find(name => name.Name.Contains(userName.ToLower())));
            }
            else
            {
                _hero = new Hero(heroes.Find(name => name.Name.Contains(firstName.ToLower())));
            }

            if (userName == null && firstName == null)
            {
                _hero = new Hero(heroes.Find(name => name.Name.Contains(secondName.ToLower())));
            }

            if (userName == null) _userName = firstName;
            if (firstName == null) _firstName = userName;
            if (secondName == null) _lastName = userName;
            
            _fullName = $"{_userName}|{_firstName}|{_lastName}";
        }

        public string GO(int[] coordinates, Hero.Directions directions)
        {
            _message = $"@{_hero.Name} {_startMessageConstruct[random.Next(_startMessageConstruct.Length)].ToLower()}" +
                $"{_goVerbs[random.Next(_goVerbs.Length)].ToLower()}";

            if (_hero.Dirty >= _hero.MaximumDirty)
            {
                return "Ты слишком грязный что бы ходить";
            }
            else
            {
                if (coordinates == null)
                {
                    switch (directions)
                    {
                        case Hero.Directions.North:
                            _hero.ChangePosition(Hero.Directions.North, coordinates: null);
                            return _message += $" <b><u>севернее</u></b>{Environment.NewLine}" +
                                $"{Environment.NewLine}<i>Координаты</i>{Environment.NewLine}( X: {_hero.PositionX} Y: {_hero.PositionY} )";

                        case Hero.Directions.East:
                            _hero.ChangePosition(Hero.Directions.East, coordinates: null);
                            return _message += $" <b><u>восточнее</u></b>{Environment.NewLine}" +
                                $"{Environment.NewLine}<i>Координаты</i>{Environment.NewLine}( X: {_hero.PositionX} Y: {_hero.PositionY} )";

                        case Hero.Directions.West:
                            _hero.ChangePosition(Hero.Directions.West, coordinates: null);
                            return _message += $" <b><u>западнее</u></b>{Environment.NewLine}" +
                                $"{Environment.NewLine}<i>Координаты</i>{Environment.NewLine}( X: {_hero.PositionX} Y: {_hero.PositionY} )";

                        case Hero.Directions.South:
                            _hero.ChangePosition(Hero.Directions.South, coordinates: null);
                            return _message += $" <b><u>южнее</u></b>{Environment.NewLine}" +
                                $"{Environment.NewLine}<i>Координаты</i>{Environment.NewLine}( X: {_hero.PositionX} Y: {_hero.PositionY} )";

                        default: return "Ты стоишь на месте";
                    }
                }
                else
                {
                    _message = "";

                    _hero.ChangePosition(directions: default, coordinates);

                    return _message = $"@{_hero.Name} {_startMessageConstruct[random.Next(_startMessageConstruct.Length)].ToLower()}" +
                        $"{_goVerbs[random.Next(_goVerbs.Length)].ToLower()} на [ {coordinates[0] + coordinates[1]} ] " +
                        $"Координаты: | X {_hero.PositionX} | Y {_hero.PositionY} |";
                }
            }
        }

        public string RandomSpawn()
        {
            _coordinates[0] = random.Next(X_MINIMUM, X_MAXIMUM);
            _coordinates[1] = random.Next(Y_MINIMUM, Y_MAXIMUM);

            _hero.ChangePosition(directions: default, _coordinates);

            return _message = $"@{_hero.Name} " +
                $"{_randomSpawnMessage[random.Next(_randomSpawnMessage.Length)]}" +
                $"появляется в общем толчке " +
                $"[ Координаты ]{Environment.NewLine}" +
                $"X: {_hero.PositionX}{Environment.NewLine}" +
                $"Y: {_hero.PositionY}";
        }

        public string HeroInfo()
        {
            if (_hero.Name == null) return "Такого героя не существует!";
            if (!_hero.Parasite.ID.Contains("E")) _hero.IllsTick();
            if (!_hero.Phisical.ID.Contains("E")) _hero.IllsTick();
            if (!_hero.Mental.ID.Contains("E")) _hero.IllsTick();


            string _weaponDamageInfo = string.Empty;
            string _heroLocationMark = string.Empty;
            string _questsInfo = string.Empty;
            string _heroMark = string.Empty;
            string _heroIllInfo = string.Empty;

            if (_hero.Weapon.Name.Contains("ничего"))
            {
                _weaponDamageInfo = $"(0,3 - 0,8)";
            }
            else
            {
                _weaponDamageInfo = $"({string.Format("{0:f2}", _hero.Weapon.MinDamage)} - {string.Format("{0:f2}", _hero.Weapon.MaxDamage)})";
            }

            switch (Zoo.WhatHeroLocated(_hero.PositionX, _hero.PositionY))
            {
                case Zoo.Zones.Red:
                    _heroLocationMark = "&#128997";
                    break;
                case Zoo.Zones.Blue:
                    _heroLocationMark = "&#128998";
                    break;
                case Zoo.Zones.Green:
                    _heroLocationMark = "&#129001";
                    break;
                case Zoo.Zones.Purple:
                    _heroLocationMark = "&#129002";
                    break;
                case Zoo.Zones.Black:
                    _heroLocationMark = "&#11035";
                    break;
                case Zoo.Zones.White:
                    _heroLocationMark = "&#11036";
                    break;
                default:
                    _heroLocationMark = "&#10071";
                    break;
            }

            if (_hero.Quests == null)
            {
                _questsInfo += $"У тебя нет квестов, возьми их!";
            }
            else
            {
                for (int i = 0; i < _hero.Quests.Count; i++)
                {

                    _questsInfo += $"{i + 1}. <i>{_hero.Quests[i].Title}</i> ({_hero.Quests[i].Progress} > {_hero.Quests[i].Total})" + Environment.NewLine +
                        $"<b>[ Q ] [</b> /quest{_hero.Quests[i].QuestID} <b>]</b>" + Environment.NewLine;
                }
            }

            if (_hero.Parasite.ID != "E") _heroIllInfo += _heroIllsMark[0] + " ";
            if (_hero.Phisical.ID != "E") _heroIllInfo += _heroIllsMark[1] + " ";
            if (_hero.Mental.ID != "E") _heroIllInfo += _heroIllsMark[2] + " ";

            if (_hero.DemiGod)
            {
                _heroMark = _heroStatusIco[1];
            }
            else
            {
                _heroMark = _heroStatusIco[0];
            }

            _message = "";

            _message = $"{_heroMark} <b><u>{_hero.Name.ToUpper()}</u></b> [ {_hero.Level} | {(int)_hero.Rank} ] " +
                $"{Environment.NewLine}" +
                $"&#127793 <b>Опыт:</b>  " +
                $"[ <i>{string.Format("{0:f2}", _hero.LevelExpirience, 2)} / {_hero.MaxLevelExpirience}</i> &#128167 ]" +
                $"{Environment.NewLine}" +
                $"&#127894 <b>Ранг:</b> " +
                $"[ <i>{string.Format("{0:F2}", _hero.RankExpirience)} / {_hero.MaxRankExpirience}</i> &#9884 ]" +
                $"{Environment.NewLine}" +
                $"{_hero.Heart} <b>Грязь:</b> ( <i>{string.Format("{0:p2}", (decimal)(_hero.Dirty / _hero.MaximumDirty))} | " +
                $"{string.Format("{0:f2}", _hero.MaximumDirty)}</i> )" +
                $"{Environment.NewLine}" +
                $"&#128176 <b>ГовноТенге:</b> <i>{_hero.Money} </i>" +
                $"{Environment.NewLine}" +
                $"{_heroIllInfo}" +
                $"{Environment.NewLine}" +
                $"<b>&#129314 Токсичность:</b> {_hero.Toxic}" +
                $"{Environment.NewLine}" +
                $"<b>&#129480 Жиры:</b> {_hero.Fats}" +
                $"{Environment.NewLine}" +
                $"<b>&#129325 Чрево:</b> {_hero.Stomach}" +
                $"{Environment.NewLine}" +
                $"<b>&#129516 Метаболизм:</b> {_hero.Metabolism}" +
                $"{Environment.NewLine}" +
                $"&#9876 <b>Урон:</b> {string.Format("{0:f2}", _hero.Weapon.MinDamage + _hero.BaseAttack())} - {string.Format("{0:f2}", _hero.Weapon.MaxDamage + _hero.BaseAttack())}" +
                $"{Environment.NewLine}" +
                $"&#128737 <b>Защита:</b> {string.Format("{0:f2}", _hero.Defence)}" +
                $"{Environment.NewLine}" +
                $"<b>Свободных очков:</b> {_hero.FreePoints}" +
                $"{Environment.NewLine}{Environment.NewLine}" +
                $"&#128481 <b>{_hero.Weapon.Name}</b> <i>{_weaponDamageInfo}</i>{Environment.NewLine}" +
                $"&#129686 <b>{_hero.Helmet.Name}</b> ( <i>{string.Format("{0:f2}", _hero.Helmet.Defence)}</i> ){Environment.NewLine}" +
                $"&#129355 <b>{_hero.Armor.Name}</b> ( <i>{string.Format("{0:f2}", _hero.Armor.Defence)}</i> ){Environment.NewLine}" +
                $"&#128737 <b>{_hero.Shield.Name}</b> ( <i>{string.Format("{0:f2}", _hero.Shield.Defence)}</i> ){Environment.NewLine}" +
                $"" +                
                $"<u>К</u> <u>В</u> <u>Е</u> <u>С</u> <u>Т</u> <u>Ы</u>{Environment.NewLine}" +
                $"{_questsInfo}" + Environment.NewLine +
                $"&#128506 _ ( X: {_hero.PositionX} ) ( Y: {_hero.PositionY} ) _ {_heroLocationMark}";
            
            return _message;
        }

        public void PlayerInfo(Hero hero)
        {
            _hero = hero;
        }

        public string HeroStatUpdate(Hero.Characteristics characteristics)
        {
            if (_hero.FreePoints == 0)
            {
                return _message = "У тебя нет свободных очков";
            }
            else
            {
                switch (characteristics)
                {
                    case Hero.Characteristics.Toxic:
                        _hero.UpdateCharacteristics(characteristics);
                        return _message = $"Желч стала токсичнее на 1";
                    case Hero.Characteristics.Fats:
                        _hero.UpdateCharacteristics(characteristics);
                        return _message = $"Увеличили твою жировую массу на 1";
                    case Hero.Characteristics.Stomach:
                        _hero.UpdateCharacteristics(characteristics);
                        return _message = $"Растянули твой желудок на 1";
                    case Hero.Characteristics.Metabolism:
                        _hero.UpdateCharacteristics(characteristics);
                        return _message = $"Разогнали твой метаболизм на 1";
                    default:
                        return "По итогу, что растягиваем?";
                }
            }
        }

        public bool SuchHeroExist()
        {
            if(heroes.Find(player => player.Name.Contains(_fullName)) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}