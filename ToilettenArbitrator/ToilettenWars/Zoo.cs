using System.Linq;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using ToilettenArbitrator.Brain;
using ToilettenArbitrator.ToilettenWars.Cages;
using ToilettenArbitrator.ToilettenWars.Cages.MobTypes;
using ToilettenArbitrator.ToilettenWars.Items;
using ToilettenArbitrator.ToilettenWars.Person;
using Windows.Devices.PointOfService;
using Windows.System.Profile;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ToilettenArbitrator.ToilettenWars
{
    public class Zoo
    {
        // Zones Data   [ X(xmin,xmax) Y(ymin,ymax) ]
        // Red Zone     [ X (  0,  80) Y (  0, 100) ]
        // Black Zone   [ X (  0, 120) Y (100, 145) ]
        // Purple Zone  [ X (  0, 120) Y (145, 200) ]
        // Green Zone   [ X ( 80, 200) Y (  0,  55) ]
        // White Zone   [ X ( 80, 200) Y ( 55, 100) ]
        // Blue Zone    [ X (120, 200) Y (100, 200) ]

        private MembersDataContext MDC = new MembersDataContext();

        private const float MOBS_POPULATION_FACTOR = 0.28f;
        private const float BOSS_POPULATION_FACTOR = 0.02f;

        public readonly static int[] RED_ZONE_COORDINATES = new int[5] { 0, 80, 0, 100, 8000 };
        public readonly static int[] BLUE_ZONE_COORDINATES = new int[5] { 120, 200, 100, 200, 8000 };
        public readonly static int[] GREEN_ZONE_COORDINATES = new int[5] { 80, 200, 0, 55, 6600 };
        public readonly static int[] PURPLE_ZONE_COORDINATES = new int[5] { 0, 120, 145, 200, 6600 };
        public readonly static int[] BLACK_ZONE_COORDINATES = new int[5] { 0, 120, 100, 145, 5400 };
        public readonly static int[] WHITE_ZONE_COORDINATES = new int[5] { 80, 200, 55, 100, 5400 };

        public const string RED_ZONE = "&#128997";
        public const string BLUE_ZONE = "&#128998";
        public const string GREEN_ZONE = "&#129001";
        public const string PURPLE_ZONE = "&#129002";
        public const string BLACK_ZONE = "&#11035";
        public const string WHITE_ZONE = "&#11036";

        private List<MobCard> _mobsData = new List<MobCard>();

        private List<MobCard> _bossData = new List<MobCard>();

        private List<Mob> _mobsAround = new List<Mob>();
        private List<Mob> _blueZoneMobs = new List<Mob>();
        private List<Mob> _redZoneMobs = new List<Mob>();
        private List<Mob> _greenZoneMobs = new List<Mob>();
        private List<Mob> _purpleZoneMobs = new List<Mob>();
        private List<Mob> _blackZoneMobs = new List<Mob>();
        private List<Mob> _whiteZoneMobs = new List<Mob>();

        private List<Boss> _redBosses = new List<Boss>();
        private List<Boss> _blueBosses = new List<Boss>();
        private List<Boss> _greenBosess = new List<Boss>();
        private List<Boss> _purpleBoses = new List<Boss>();
        private List<Boss> _blackBosess = new List<Boss>();
        private List<Boss> _whiteBosess = new List<Boss>();

        private Mob _attackingMob;
        private Boss _attackingBoss;

        private int _heroPositionX, _heroPositionY;
        private int[] _mobsTypeData;
        private string _locationMark, _mobsAroundInfo, _mobInfo, _bossAroundInfo, _bossInfo;
        private Zones _heroLocated;

        public enum Zones
        {
            Red,
            Blue,
            Green,
            Purple,
            Black,
            White
        }

        public int HeroPositionX { set => _heroPositionX = value; }
        public int HeroPositionY { set => _heroPositionY = value; }
        public string LocationMark => _locationMark;
        public string MobsAround => WhatsMobsAround();

        //public SimpleMob OneMob => GetSimpleMob();
        //public List<SimpleMob> NearMobs => GetNearMobs();

        public Zoo()
        {
            for (int i = 0; i < MDC.MobCards.ToList().Count; i++)
            {
                if (new Mob(MDC.MobCards.ToList()[i]).Type == MobType.Boss)
                {
                    _bossData.Add(MDC.MobCards.ToList()[i]);            
                }
                else
                {
                    _mobsData.Add(MDC.MobCards.ToList()[i]);
                }
            }

            _mobsTypeData = new int[_mobsData.Count];

            for (int i = 0; i < RED_ZONE_COORDINATES[4] * MOBS_POPULATION_FACTOR; i++)
            {
                _redZoneMobs.Add(new Mob(_mobsData[new Random().Next(_mobsData.Count)]));
                _redZoneMobs[i].GoCoordinate(
                    new SilverDice().JustRandom(RED_ZONE_COORDINATES[0], RED_ZONE_COORDINATES[1]),
                    new SilverDice().JustRandom(RED_ZONE_COORDINATES[2], RED_ZONE_COORDINATES[3]));
                WhatMobsCounter(_redZoneMobs[i]);
            }

            for (int i = 0; i < BLUE_ZONE_COORDINATES[4] * MOBS_POPULATION_FACTOR; i++)
            {
                _blueZoneMobs.Add(new Mob(_mobsData[new Random().Next(_mobsData.Count)]));
                _blueZoneMobs[i].GoCoordinate(
                    new SilverDice().JustRandom(BLUE_ZONE_COORDINATES[0], BLUE_ZONE_COORDINATES[1]),
                    new SilverDice().JustRandom(BLUE_ZONE_COORDINATES[2], BLUE_ZONE_COORDINATES[3]));
                WhatMobsCounter(_blueZoneMobs[i]);
            }

            for (int i = 0; i < GREEN_ZONE_COORDINATES[4] * MOBS_POPULATION_FACTOR; i++)
            {
                _greenZoneMobs.Add(new Mob(_mobsData[new Random().Next(_mobsData.Count)]));
                _greenZoneMobs[i].GoCoordinate(
                    new SilverDice().JustRandom(GREEN_ZONE_COORDINATES[0], GREEN_ZONE_COORDINATES[1]),
                    new SilverDice().JustRandom(GREEN_ZONE_COORDINATES[2], GREEN_ZONE_COORDINATES[3]));
                WhatMobsCounter(_greenZoneMobs[i]);
            }

            for (int i = 0; i < PURPLE_ZONE_COORDINATES[4] * MOBS_POPULATION_FACTOR; i++)
            {
                _purpleZoneMobs.Add(new Mob(_mobsData[new Random().Next(_mobsData.Count)]));
                _purpleZoneMobs[i].GoCoordinate(
                    new SilverDice().JustRandom(PURPLE_ZONE_COORDINATES[0], PURPLE_ZONE_COORDINATES[1]),
                    new SilverDice().JustRandom(PURPLE_ZONE_COORDINATES[2], PURPLE_ZONE_COORDINATES[3]));
                WhatMobsCounter(_purpleZoneMobs[i]);
            }

            for (int i = 0; i < BLACK_ZONE_COORDINATES[4] * MOBS_POPULATION_FACTOR; i++)
            {
                _blackZoneMobs.Add(new Mob(_mobsData[new Random().Next(_mobsData.Count)]));
                _blackZoneMobs[i].GoCoordinate(
                    new SilverDice().JustRandom(BLACK_ZONE_COORDINATES[0], BLACK_ZONE_COORDINATES[1]),
                    new SilverDice().JustRandom(BLACK_ZONE_COORDINATES[2], BLACK_ZONE_COORDINATES[3]));
                WhatMobsCounter(_blackZoneMobs[i]);
            }

            for (int i = 0; i < WHITE_ZONE_COORDINATES[4] * MOBS_POPULATION_FACTOR; i++)
            {
                _whiteZoneMobs.Add(new Mob(_mobsData[new Random().Next(_mobsData.Count)]));
                _whiteZoneMobs[i].GoCoordinate(
                    new SilverDice().JustRandom(WHITE_ZONE_COORDINATES[0], WHITE_ZONE_COORDINATES[1]),
                    new SilverDice().JustRandom(WHITE_ZONE_COORDINATES[2], WHITE_ZONE_COORDINATES[3]));
                WhatMobsCounter(_whiteZoneMobs[i]);
            }

        }

        public Zones HeroLocated(int HeroPositionX, int HeroPositionY)
        {
            _heroPositionX = HeroPositionX;
            _heroPositionY = HeroPositionY;

            if (_heroPositionX >= RED_ZONE_COORDINATES[0] && _heroPositionX < RED_ZONE_COORDINATES[1] &&
                _heroPositionY >= RED_ZONE_COORDINATES[2] && _heroPositionY < RED_ZONE_COORDINATES[3])
            {
                _locationMark = RED_ZONE;
                _heroLocated = Zones.Red;
                return Zones.Red;
            }
            else if (_heroPositionX >= BLUE_ZONE_COORDINATES[0] && _heroPositionX < BLUE_ZONE_COORDINATES[1] &&
                     _heroPositionY >= BLUE_ZONE_COORDINATES[2] && _heroPositionY < BLUE_ZONE_COORDINATES[3])
            {
                _locationMark = BLUE_ZONE;
                _heroLocated = Zones.Blue;
                return Zones.Blue;
            }
            else if (_heroPositionX >= GREEN_ZONE_COORDINATES[0] && _heroPositionX < GREEN_ZONE_COORDINATES[1] &&
                    _heroPositionY >= GREEN_ZONE_COORDINATES[2] && _heroPositionY < GREEN_ZONE_COORDINATES[3])
            {
                _locationMark = GREEN_ZONE;
                _heroLocated = Zones.Green;
                return Zones.Green;
            }
            else if (_heroPositionX >= PURPLE_ZONE_COORDINATES[0] && _heroPositionX < PURPLE_ZONE_COORDINATES[1] &&
                    _heroPositionY >= PURPLE_ZONE_COORDINATES[2] && _heroPositionY < PURPLE_ZONE_COORDINATES[3])
            {
                _locationMark = PURPLE_ZONE;
                _heroLocated = Zones.Purple;
                return Zones.Purple;
            }
            else if (_heroPositionX >= BLACK_ZONE_COORDINATES[0] && _heroPositionX < BLACK_ZONE_COORDINATES[1] &&
                    _heroPositionY >= BLACK_ZONE_COORDINATES[2] && _heroPositionY < BLACK_ZONE_COORDINATES[3])
            {
                _locationMark = BLACK_ZONE;
                _heroLocated = Zones.Black;
                return Zones.Black;
            }
            else
            {
                _locationMark = WHITE_ZONE;
                _heroLocated = Zones.White;
                return Zones.White;
            };
        }

        public bool MobFight(Hero hero, string mobID, out LootBox lootBox, out string battleResult)
        {
            string mobXLine = $"{mobID[0]}{mobID[1]}{mobID[2]}", mobYLine = $"{mobID[3]}{mobID[4]}{mobID[5]}";
            int mobX = int.Parse(mobXLine), mobY = int.Parse(mobYLine);

            string _questCompleteText = string.Empty;

            string mobRealID = string.Empty;
            int iDMobInList = 0, idInCards = 0;

            lootBox = new LootBox();
            battleResult = string.Empty;

            float heroAttack = hero.Attack;

            for (int i = 0; i < _mobsData.Count; i++)
            {
                if (mobID.Contains(_mobsData[i].Id))
                {
                    idInCards = i;
                    mobRealID = _mobsData[i].Id;
                    break;
                }
                else
                {
                    continue;
                }
            }

            Zones whatZone = new Zones();

            switch (WhatHeroLocated(hero.PositionX, hero.PositionY))
            {
                case Zones.Red:
                    _attackingMob = _redZoneMobs.Find(redmob => redmob.Id.Contains(mobRealID) && redmob.PositionX == mobX && redmob.PositionY == mobY);
                    iDMobInList = _redZoneMobs.FindIndex(redmob => redmob.Id.Contains(mobRealID) && redmob.PositionX == mobX && redmob.PositionY == mobY);
                    whatZone = Zones.Red;
                    break;

                case Zones.Blue:
                    _attackingMob = _blueZoneMobs.Find(bluemob => bluemob.Id.Contains(mobRealID) && bluemob.PositionX == mobX && bluemob.PositionY == mobY);
                    iDMobInList = _blueZoneMobs.FindIndex(bluemob => bluemob.Id.Contains(mobRealID) && bluemob.PositionX == mobX && bluemob.PositionY == mobY);
                    whatZone = Zones.Blue;
                    break;

                case Zones.Green:
                    _attackingMob = _greenZoneMobs.Find(greenmob => greenmob.Id.Contains(mobRealID) && greenmob.PositionX == mobX && greenmob.PositionY == mobY);
                    iDMobInList = _greenZoneMobs.FindIndex(greenmob => greenmob.Id.Contains(mobRealID) && greenmob.PositionX == mobX && greenmob.PositionY == mobY);
                    whatZone = Zones.Green;
                    break;

                case Zones.Purple:
                    _attackingMob = _purpleZoneMobs.Find(purplemob => purplemob.Id.Contains(mobRealID) && purplemob.PositionX == mobX && purplemob.PositionY == mobY);
                    iDMobInList = _purpleZoneMobs.FindIndex(purplemob => purplemob.Id.Contains(mobRealID) && purplemob.PositionX == mobX && purplemob.PositionY == mobY);
                    whatZone = Zones.Purple;
                    break;

                case Zones.Black:
                    _attackingMob = _blackZoneMobs.Find(blackmob => blackmob.Id.Contains(mobRealID) && blackmob.PositionX == mobX && blackmob.PositionY == mobY);
                    iDMobInList = _blackZoneMobs.FindIndex(blackmob => blackmob.Id.Contains(mobRealID) && blackmob.PositionX == mobX && blackmob.PositionY == mobY);
                    whatZone = Zones.Black;
                    break;

                case Zones.White:
                    _attackingMob = _whiteZoneMobs.Find(whitemob => whitemob.Id.Contains(mobRealID) && whitemob.PositionX == mobX && whitemob.PositionY == mobY);
                    iDMobInList = _whiteZoneMobs.FindIndex(whitemob => whitemob.Id.Contains(mobRealID) && whitemob.PositionX == mobX && whitemob.PositionY == mobY);
                    whatZone = Zones.White;
                    break;

                default:
                    _attackingMob = new Mob(_mobsData[_mobsData.Count]);
                    break;
            }

            if (_attackingMob == null)
            {
                battleResult += "В том месте никого НЕТ!";
                return false;
            }

            if (_attackingMob.AddDamage(heroAttack, out lootBox))
            {
                switch (whatZone)
                {
                    case Zones.Red:
                        _redZoneMobs.RemoveAt(iDMobInList);
                        break;
                    case Zones.Blue:
                        _blueZoneMobs.RemoveAt(iDMobInList);
                        break;
                    case Zones.Green:
                        _greenZoneMobs.RemoveAt(iDMobInList);
                        break;
                    case Zones.Purple:
                        _purpleZoneMobs.RemoveAt(iDMobInList);
                        break;
                    case Zones.Black:
                        _blackZoneMobs.RemoveAt(iDMobInList);
                        break;
                    case Zones.White:
                        _whiteZoneMobs.RemoveAt(iDMobInList);
                        break;
                    default:
                        break;
                }

                for (int i = 0; i < hero.Quests.Count; i++)
                {
                    string _questID = hero.Quests[i].QuestID;

                    if (_questID.Contains("E")) break;

                    if (_attackingMob.Id == hero.Quests[i].FirstSuspectID)
                    {                        
                        if (hero.Gotcha(hero.Quests[i].QuestID, _attackingMob.Id))
                        {
                            _questCompleteText += $"{new HelloSynapse().GreatWords.ToUpper()}" + Environment.NewLine;
                            _questCompleteText += $"Квест: <u><i>{new QuestBox(_questID).Title}</i></u> ЗАВЕРШЁН!" + Environment.NewLine;
                            _questCompleteText += $"&#128230 ( &#128176 {new QuestBox(_questID).Prize.Cash} | " +
                                $"&#128167 {string.Format("{0:f2}", new QuestBox(_questID).Prize.Expirience)} | " +
                                $"&#9884 {string.Format("{0:f2}", new QuestBox(_questID).Prize.RankPoints)} )" + Environment.NewLine;
                        }
                        else
                        {
                            _questCompleteText += $"Квест: {hero.Quests[i].Title}" + Environment.NewLine;
                            _questCompleteText += $"{_attackingMob.Name} уничтожено {hero.Quests[i].Progress}/{hero.Quests[i].Total}" + Environment.NewLine;
                        }
                    } 
                }

                battleResult += _questCompleteText + Environment.NewLine;
                battleResult += $"\"{_attackingMob.SubName}\" {_attackingMob.Name}{Environment.NewLine}";
                return true;
            }
            else
            {
                battleResult += $"&#9888 <b>A C H T U N G</b> &#9888{Environment.NewLine}" +
                        $"@{hero.Name} ( {hero.Heart} | {string.Format("{0:f2}", hero.Dirty)} ){Environment.NewLine}&#9876 &#9876 &#9876 &#9876 ";
                battleResult += $"{Environment.NewLine}<i>\"{_attackingMob.SubName}\" {_attackingMob.Name}</i> " + Environment.NewLine +
                    $"({string.Format("{0:f2}", _attackingMob.HitPoints)} / {string.Format("{0:f1}", _attackingMob.MaximumHitPoints)}){Environment.NewLine}{Environment.NewLine}";
                    
                hero.AddDamage(_attackingMob.Damage);
                battleResult += $"<b>{_attackingMob.Name}</b> ударил в ответ {string.Format("{0:f2}", _attackingMob.Damage)} - {string.Format("{0:f2}", hero.Defence)}";
                battleResult += Environment.NewLine + _questCompleteText + Environment.NewLine;
                if (hero.AddIll(_attackingMob.Infect())) battleResult += "+";
                return false;
            }
        }

        public static Zones WhatHeroLocated(int HeroPositionX, int HeroPositionY)
        {
            if (HeroPositionX >= RED_ZONE_COORDINATES[0] && HeroPositionX < RED_ZONE_COORDINATES[1] &&
                HeroPositionY >= RED_ZONE_COORDINATES[2] && HeroPositionY < RED_ZONE_COORDINATES[3])
            {
                return Zones.Red;
            }
            else if (HeroPositionX >= BLUE_ZONE_COORDINATES[0] && HeroPositionX < BLUE_ZONE_COORDINATES[1] &&
                     HeroPositionY >= BLUE_ZONE_COORDINATES[2] && HeroPositionY < BLUE_ZONE_COORDINATES[3])
            {
                return Zones.Blue;
            }
            else if (HeroPositionX >= GREEN_ZONE_COORDINATES[0] && HeroPositionX < GREEN_ZONE_COORDINATES[1] &&
                     HeroPositionY >= GREEN_ZONE_COORDINATES[2] && HeroPositionY < GREEN_ZONE_COORDINATES[3])
            {
                return Zones.Green;
            }
            else if (HeroPositionX >= PURPLE_ZONE_COORDINATES[0] && HeroPositionX < PURPLE_ZONE_COORDINATES[1] &&
                     HeroPositionY >= PURPLE_ZONE_COORDINATES[2] && HeroPositionY < PURPLE_ZONE_COORDINATES[3])
            {
                return Zones.Purple;
            }
            else if (HeroPositionX >= BLACK_ZONE_COORDINATES[0] && HeroPositionX < BLACK_ZONE_COORDINATES[1] &&
                     HeroPositionY >= BLACK_ZONE_COORDINATES[2] && HeroPositionY < BLACK_ZONE_COORDINATES[3])
            {
                return Zones.Black;
            }
            else
            {
                return Zones.White;
            };
        }

        private string WhatsMobsAround()
        {
            switch (_heroLocated)
            {
                case Zones.Red:
                    MobsSorter(Zones.Red);
                    return InfoSorter(_mobsAround);

                case Zones.Blue:
                    MobsSorter(Zones.Blue);
                    return InfoSorter(_mobsAround);

                case Zones.Green:
                    MobsSorter(Zones.Green);
                    return InfoSorter(_mobsAround);

                case Zones.Purple:
                    MobsSorter(Zones.Purple);
                    return InfoSorter(_mobsAround);

                case Zones.Black:
                    MobsSorter(Zones.Black);
                    return InfoSorter(_mobsAround);

                case Zones.White:
                    MobsSorter(Zones.White);
                    return InfoSorter(_mobsAround);

                default:
                    return "НИКОГО РЯДОМ!!!";
            }
        }

        private string InfoSorter(List<Mob> mobs)
        {
            _mobsAroundInfo = string.Empty;
            if (mobs.Count > 7)
            {
                for (int i = 0; i < 7; i++)
                {
                    _mobsAroundInfo += $"{i + 1}. <b><i>\"{mobs[i].SubName}\" {mobs[i].Name}</i></b>{Environment.NewLine}" +
                        $"{mobs[i].Status}{Environment.NewLine}" +
                        $"{mobs[i].Heart} <b>(</b> <i>{string.Format( "{0:F2}", mobs[i].HitPoints)} / {string.Format("{0:F2}", mobs[i].MaximumHitPoints)}</i> <b>)</b>{Environment.NewLine}" +
                        $"<code>[ A ] [</code> /atk{string.Format("{0:d3}", mobs[i].PositionX)}{string.Format("{0:d3}", mobs[i].PositionY)}{mobs[i].Id} <code>]</code>{Environment.NewLine}" +
                        $"<code>[ M ] [</code> /mob{string.Format("{0:d3}", mobs[i].PositionX)}{string.Format("{0:d3}", mobs[i].PositionY)}{mobs[i].Id} <code>]</code>{Environment.NewLine}{Environment.NewLine}";
                }
                return _mobsAroundInfo;
            }
            else
            {
                for (int i = 0; i < mobs.Count; i++)
                {
                    _mobsAroundInfo += $"{i + 1}. <b><i>\"{mobs[i].SubName}\" {mobs[i].Name}</i></b>{Environment.NewLine}" +
                        $"{mobs[i].Status}{Environment.NewLine}" +
                        $"{mobs[i].Heart} <b>(</b> <i>{string.Format("{0:F2}", mobs[i].HitPoints)} / {string.Format("{0:F2}", mobs[i].MaximumHitPoints)}</i> <b>)</b>{Environment.NewLine}" +
                        $"<code>[ A ] [</code> /atk{string.Format("{0:d3}", mobs[i].PositionX)}{string.Format("{0:d3}", mobs[i].PositionY)}{mobs[i].Id} <code>]</code>{Environment.NewLine}" +
                        $"<code>[ M ] [</code> /mob{string.Format("{0:d3}", mobs[i].PositionX)}{string.Format("{0:d3}", mobs[i].PositionY)}{mobs[i].Id} <code>]</code>{Environment.NewLine}{Environment.NewLine}";
                }
                return _mobsAroundInfo;
            }
        }

        private void MobsSorter(Zones zones)
        {
            _mobsAround.Clear();
            
            switch (zones)
            {
                case Zones.Red:
                    for (int i = 0; i < _redZoneMobs.Count; i++)
                    {
                        if (NearPosition(_redZoneMobs[i], _heroPositionX, _heroPositionY) == true)
                        {
                            _mobsAround.Add(_redZoneMobs[i]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    break;

                case Zones.Blue:
                    for (int i = 0; i < _blueZoneMobs.Count; i++)
                    {
                        if (NearPosition(_blueZoneMobs[i], _heroPositionX, _heroPositionY) == true)
                        {
                            _mobsAround.Add(_blueZoneMobs[i]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    break;

                case Zones.Green:
                    for (int i = 0; i < _greenZoneMobs.Count; i++)
                    {
                        if (NearPosition(_greenZoneMobs[i], _heroPositionX, _heroPositionY) == true)
                        {
                            _mobsAround.Add(_greenZoneMobs[i]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    break;

                case Zones.Purple:
                    for (int i = 0; i < _purpleZoneMobs.Count; i++)
                    {
                        if (NearPosition(_purpleZoneMobs[i], _heroPositionX, _heroPositionY) == true)
                        {
                            _mobsAround.Add(_purpleZoneMobs[i]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    break;

                case Zones.Black:
                    for (int i = 0; i < _blackZoneMobs.Count; i++)
                    {
                        if (NearPosition(_blackZoneMobs[i], _heroPositionX, _heroPositionY) == true)
                        {
                            _mobsAround.Add(_blackZoneMobs[i]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    break;

                case Zones.White:
                    for (int i = 0; i < _whiteZoneMobs.Count; i++)
                    {
                        if (NearPosition(_whiteZoneMobs[i], _heroPositionX, _heroPositionY) == true)
                        {
                            _mobsAround.Add(_whiteZoneMobs[i]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private bool NearPosition(Mob mob, int heroX, int heroY)
        {
            if (mob.PositionX <= heroX + 5 && mob.PositionX >= heroX - 5 &&
                mob.PositionY <= heroY + 5 && mob.PositionY >= heroY - 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string MobInfo(string mobName, Hero hero)
        {
            string mobXLine = $"{mobName[0]}{mobName[1]}{mobName[2]}", mobYLine = $"{mobName[3]}{mobName[4]}{mobName[5]}";
            int mobX = int.Parse(mobXLine), mobY = int.Parse(mobYLine);

            string heart = string.Empty;
            string mobRealID = string.Empty;
            mobRealID = mobName.Remove(0, 6);
            int iDMobInList = 0, idInCards = 0;

            MobsSorter(HeroLocated(hero.PositionX, hero.PositionY));

            _attackingMob = _mobsAround.Find(mob => mob.Id.Contains(mobRealID) && 
                                                    mob.PositionX == mobX && 
                                                    mob.PositionY == mobY);

            _mobInfo = string.Empty;

            if (_attackingMob == null)
            {
                _mobInfo += "В том месте никого НЕТ!";
                return _mobInfo;
            }

            _mobInfo += $"\"{_attackingMob.SubName}\" {_attackingMob.Name} {_attackingMob.Status}{Environment.NewLine}" +
                $"{_attackingMob.Heart} {string.Format("{0:F2}", _attackingMob.HitPoints)}  |  " +
                $"&#128481 {string.Format("{0:F2}", _attackingMob.Damage)}  |  " +
                $"&#128737 {string.Format("{0:F2}", _attackingMob.Defence)}{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"<b>Выдержка из Большого Туалетного Атласа Монстров</b>{Environment.NewLine}{Environment.NewLine}" +
                $"&#128218 <i>{_attackingMob.Description}</i>";
            return _mobInfo;
        }

        public void CagesData()
        {
            //List<string, int> MobsSeparator = new List<string, int>();
            //for (int i = 0; i < _mobsData.Count; i++)
            //{
            //    MobsSeparator.Add(_mobsData[i].Id, 0);
            //}
        
            string infoLine = string.Empty;
            int redMobs = _redZoneMobs.Count;
            int blueMobs = _blueZoneMobs.Count;
            int greenMobs = _greenZoneMobs.Count;
            int purpleMobs = _purpleZoneMobs.Count;
            int blackMobs = _blackZoneMobs.Count;
            int whiteMobs = _whiteZoneMobs.Count;
            
            infoLine += $"> Красная зона: {redMobs}" + Environment.NewLine;
            infoLine += $"> Синяя зона: {blueMobs}" + Environment.NewLine;
            infoLine += $"> Зелёная зона: {greenMobs}" + Environment.NewLine;
            infoLine += $"> Фиолетовая зона: {purpleMobs}" + Environment.NewLine;
            infoLine += $"> Чёрная зона: {blackMobs}" + Environment.NewLine;
            infoLine += $"> Белая зона: {whiteMobs}" + Environment.NewLine + Environment.NewLine;
            
            for (int i = 0; i < _mobsData.Count; i++)
            {
                infoLine += $"> {i + 1}. " + _mobsData[i].Name + ": " + _mobsTypeData[i] + Environment.NewLine;
            }
            infoLine += Environment.NewLine;
            infoLine += $"> Всего мобов: {redMobs + blueMobs + greenMobs + purpleMobs + blackMobs + whiteMobs}" + Environment.NewLine;


            Console.WriteLine(infoLine);
        }


        private void WhatMobsCounter(Mob mob)
        {
            for (int i = 0; i < _mobsData.Count; i++)
            {
                if (mob.Id == _mobsData[i].Id)
                {
                    _mobsTypeData[i] += 1;
                }
                else
                {
                    continue;
                }
            }
        }
        //public void WalkingMobs(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        //{
        //    for (int i = 0; i < RED_ZONE_COORDINATES[4] * MOBS_POPULATION_FACTOR; i++)
        //    {
        //        _redZoneMobs[i].Step();
        //        _blueZoneMobs[i].Step();
        //    }
        //    for (int i = 0; i < GREEN_ZONE_COORDINATES[4] * MOBS_POPULATION_FACTOR; i++)
        //    {
        //        _greenZoneMobs[i].Step();
        //        _purpleZoneMobs[i].Step();
        //    }
        //    for (int i = 0; i < WHITE_ZONE_COORDINATES[4] * MOBS_POPULATION_FACTOR; i++)
        //    {
        //        _blueZoneMobs[i].Step();
        //        _whiteZoneMobs[i].Step();
        //    }
        //    new MainSynapse(botClient, update, cancellationToken).Answer("Смещение");
        //}        
    }
}
