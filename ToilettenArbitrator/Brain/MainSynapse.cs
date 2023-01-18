using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ToilettenArbitrator.ToilettenWars;
using ToilettenArbitrator.ToilettenWars.Person;
using ToilettenArbitrator.ToilettenWars.Items;
using System.ComponentModel;
using System;
using ToilettenArbitrator.ToilettenWars.Quests;

namespace ToilettenArbitrator.Brain
{
    public class MainSynapse
    {
        private MembersDataContext MDC = new MembersDataContext();

        private HelloSynapse Hi = new HelloSynapse();

        private int year, month, day, hour, minute, second, millisecond, messageID;

        private long chatID = 0, userID, mainChatID = -1001719641552; 
        private string answer, messageText, firstName, lastName, userName, enemyName, botName = "ToilettenArbitratorBot", _mobName = string.Empty;

        private Arena arena, botArena;
        private ToiletRoom Room;
        private DeviceShop Shop = new DeviceShop();
        private ShitBank Bank;
        private Zoo zoo;
        private GiveMeQuest giveMeQuest = new GiveMeQuest();

        private Hero hero, enemy;
        private List<HeroCard> heroes;

        private Message answerMessage;

        public WhatKeyboardNeed WhatKeyboard { get; }

        public long ChatID => chatID;

        ITelegramBotClient _botClient;
        Update _update;
        CancellationToken _cancellationToken;

        public enum WhatKeyboardNeed
        {
            InLine,
            RePlay,
            Remove,
        }

        public MainSynapse(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            _botClient = botClient;
            _update = update;
            _cancellationToken = cancellationToken;

            userName = update.Message.From.Username;
            firstName = update.Message.From.FirstName;
            lastName = update.Message.From.LastName;

            if (update.Message.From.Username == null) userName = firstName;
            if (update.Message.From.FirstName == null) firstName = userName;
            if (update.Message.From.LastName == null) lastName = userName;

            chatID = update.Message.Chat.Id;
            messageID = update.Message.MessageId;

            messageText = update.Message.Text;

            userID = update.Message.From.Id;

            heroes = MDC.HeroCards.ToList();
            hero = new Hero(heroes.Find(h => h.Name.Contains(userName)));
        }

        public async void SynapseAnswer()
        {
            if(new ToiletRoom(userName, firstName, lastName).SuchHeroExist())
            {
                answer = string.Empty;
                answer += $"Ты кто такой ЕСТЬ?" + Environment.NewLine + Environment.NewLine +
                    $"Создай героя!{Environment.NewLine}" +
                    $"[ /createhero ]";
                Answer(answer, chatID);                
                return;
            }

            if (messageText.ToLower().Contains("/createhero"))
            {
                SilverDice money = new SilverDice();

                using (ToilettenArbitrator.MembersDataContext MDC = new ToilettenArbitrator.MembersDataContext())
                {
                    string allNames = $"{userName.ToLower()}|{firstName.ToLower()}|{lastName.ToLower()}";
                    heroes = MDC.HeroCards.ToList();
                    ToilettenArbitrator.HeroCard card = heroes.Find(name => name.Name.Contains(allNames.ToLower()));

                    if (card != null)
                    {
                        Answer("Такой персонаж уже есть!", chatID);
                    }
                    else
                    {
                        ToilettenArbitrator.HeroCard NewHero = new ToilettenArbitrator.HeroCard()
                        {
                            Name = $"" +
                            $"{userName.ToLower()}|" +
                            $"{firstName.ToLower()}|" +
                            $"{lastName.ToLower()}",
                            LevelRank = "1.1",
                            Atributes = "1.1.1.1.3",
                            Position = $"{new SilverDice().GetCoordinate}.{new SilverDice().GetCoordinate}.5",
                            Expirience = "0|0",
                            Dirty = "0",
                            Inventory = "5|E|E|E|E|E|E|E|E|E|E|E|E|E|E|E",
                            EntryDate = $"{year}.{month}.{day}/{hour}:{minute}:{second}",
                            TimersOne = $"",
                            TimersTwo = $"E|E|E",
                            Skills = $"E",
                            Talents = $"E",
                            Money = money.JustRandom(100, 300)
                        };

                        answer = "Персонаж создан";
                        MDC.HeroCards.Add(NewHero);
                        MDC.SaveChanges();

                        Answer(answer, chatID);
                    }
                }
            }

            if (messageText.ToLower().Contains("/shitshop"))
            {
                Answer(Shop.ShopInfo, chatID);
            }

            if (messageText.ToLower().Contains(DeviceShop.WSHOP))
            {
                Answer(Shop.WeaponBar, chatID);
            }

            if (messageText.ToLower().Contains(DeviceShop.ASHOP))
            {
                Answer(Shop.ArmorBar, chatID);
            }

            if (messageText.ToLower().Contains(DeviceShop.HSHOP))
            {
                Answer(Shop.HelmetBar, chatID);
            }

            if (messageText.ToLower().Contains(DeviceShop.PSHOP))
            {
                Answer(Shop.PotionBar, chatID);
            }

            if (messageText.ToLower().Contains("/gmq"))
            {
                answer = string.Empty;
                giveMeQuest = new GiveMeQuest();

                answer += $"{giveMeQuest.Hello}{giveMeQuest.AllQuestsInfo}";

                Answer(answer, chatID);
            }

            if (messageText.ToLower().Contains("/quest"))
            {
                string questId = string.Empty;
                answer = string.Empty;

                giveMeQuest = new GiveMeQuest();

                if (messageText.Contains("@ToilettenArbitratorBot"))
                {
                    questId = messageText.Replace("@ToilettenArbitratorBot", "");
                    questId = questId.Replace("/quest", "");

                }
                else
                {
                    questId = messageText.Replace("/quest", "");

                }

                answer += $"{giveMeQuest.WhatQuest(questId)}";

                Answer(answer, chatID);
            }

            if (messageText.ToLower().Contains("/buy"))
            {
                string itemId = TrimmingMessage(messageText, "buy");

                Bank = new ShitBank();

                heroes = MDC.HeroCards.ToList();
                
                Shop = new DeviceShop();

                Shop.WhatItemBought = itemId;

                hero = new Hero(heroes.Find(name => name.Name.Contains(userName.ToLower())));

                if(hero.Money < Shop.PurchasedItem.Coast)
                {
                    Answer("У тебя нет ГОВНОТЕНГЕ!", chatID);
                }
                else
                {
                    Bank.PayFromProduct(hero.GetMoney(Shop.PurchasedItem.Coast));

                    if (hero.AddItem(Shop.PurchasedItem))
                    {
                        Answer("С обновкой!!!", chatID);
                    }
                    else
                    {
                        Answer("Твоя сумка полна!", chatID);
                    }
                }
            }

            if (messageText.Contains("/upstat"))
            {
                answer = $"Ты отвлёк меня... Для чего?{Environment.NewLine}" +
                    $"А-а-а-а-а, давай раскинем пару характеристик!!!";

                await _botClient.SendTextMessageAsync(
                    chatId: chatID,
                    text: answer,
                    parseMode: ParseMode.Html,
                    replyMarkup: CharacteristicsUpdateButtons,
                    cancellationToken: _cancellationToken);
            }

            if (messageText.ToLower().Contains("токсичность +1"))
            {
                Room = new ToiletRoom(userName, firstName, lastName);

                answer = $"Записываю{Environment.NewLine}{Room.HeroStatUpdate(Hero.Characteristics.Toxic)}";
                await _botClient.SendTextMessageAsync(
                    chatId: chatID,
                    text: answer,
                    parseMode: ParseMode.Html,
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: _cancellationToken);
            }

            if (messageText.ToLower().Contains("жиры +1"))
            {
                Room = new ToiletRoom(userName, firstName, lastName);

                answer = $"Записываю{Environment.NewLine}{Room.HeroStatUpdate(Hero.Characteristics.Fats)}";
                await _botClient.SendTextMessageAsync(
                    chatId: chatID,
                    text: answer,
                    parseMode: ParseMode.Html,
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: _cancellationToken);
            }

            if (messageText.ToLower().Contains("чрево +1"))
            {
                Room = new ToiletRoom(userName, firstName, lastName);

                answer = $"Записываю{Environment.NewLine}{Room.HeroStatUpdate(Hero.Characteristics.Stomach)}";
                await _botClient.SendTextMessageAsync(
                    chatId: chatID,
                    text: answer,
                    parseMode: ParseMode.Html,
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: _cancellationToken);
            }

            if (messageText.ToLower().Contains("метаболизм +1"))
            {
                Room = new ToiletRoom(userName, firstName, lastName);

                answer = $"Записываю{Environment.NewLine}{Room.HeroStatUpdate(Hero.Characteristics.Metabolism)}";
                await _botClient.SendTextMessageAsync(
                    chatId: chatID,
                    text: answer,
                    parseMode: ParseMode.Html,
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: _cancellationToken);
            }

            if (messageText.ToLower().Contains("привет @" + botName.ToLower()))
            {
                Answer(Hi.HelloSmile, chatID);
            }

            if (messageText.ToLower().Contains("атака"))
            {
                if (messageText.Contains("@"))
                {
                    string[] messageAttackArr = messageText.Split('@');
                    enemyName = messageAttackArr[1];

                    arena = new Arena(userName, enemyName);
                    botArena = new Arena(botName, userName);

                    if (enemyName.ToLower() == botName.ToLower())
                    {
                        answer = arena.AttackNotification + Environment.NewLine + Environment.NewLine +
                            "ПРЕДУПРЕЖДЕНИЕ" + Environment.NewLine +
                            "Арбитр всегда атакует в ответ" + Environment.NewLine +
                            botArena.AttackNotification;
                    }
                    else
                    {
                        answer = arena.AttackNotification;
                    }
                }
                else
                {
                    answer = $"А кого ты пытаешься бить...? @{userName}";
                }
                Answer(Hi.AttackSmile, chatID);
                Answer(answer, chatID);
                
            }

            if (messageText.ToLower().Contains("/heroinfo"))
            {
                Room = new ToiletRoom(userName, firstName, lastName);
                answer = Room.HeroInfo();

                Answer(answer, chatID);
            }

            if (messageText.ToLower().Contains("/go"))
            {
                answer = "Куда направимся?";

                await _botClient.SendTextMessageAsync(
                    chatId: chatID,
                    text: answer,
                    parseMode: ParseMode.Html,
                    replyMarkup: DestinationButtons,
                    cancellationToken: _cancellationToken);
            }

            if (messageText.ToLower().Contains("север"))
            {
                Room = new ToiletRoom(userName, firstName, lastName);
                answer = Room.GO(coordinates: null, directions: ToilettenArbitrator.ToilettenWars.Person.Hero.Directions.North);

                answerMessage = await _botClient.SendTextMessageAsync(
                   chatId: chatID,
                   text: answer,
                   parseMode: ParseMode.Html,
                   replyMarkup: new ReplyKeyboardRemove(),
                   cancellationToken: _cancellationToken);
            }

            if (messageText.ToLower().Contains("запад"))
            {

                Room = new ToiletRoom(userName, firstName, lastName);
                answer = Room.GO(coordinates: null, directions: ToilettenArbitrator.ToilettenWars.Person.Hero.Directions.West);

                await _botClient.SendTextMessageAsync(
                    chatId: chatID,
                    text: answer,
                    parseMode: ParseMode.Html,
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: _cancellationToken);
            }

            if (messageText.ToLower().Contains("восток"))
            {
                Room = new ToiletRoom(userName, firstName, lastName);
                answer = Room.GO(coordinates: null, directions: ToilettenArbitrator.ToilettenWars.Person.Hero.Directions.East);

                await _botClient.SendTextMessageAsync(
                    chatId: chatID,
                    text: answer,
                    parseMode: ParseMode.Html,
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: _cancellationToken);
            }

            if (messageText.ToLower().Contains("юг"))
            {
                Room = new ToiletRoom(userName, firstName, lastName);
                answer = Room.GO(coordinates: null, directions: ToilettenArbitrator.ToilettenWars.Person.Hero.Directions.South);

                await _botClient.SendTextMessageAsync(
                    chatId: chatID,
                    text: answer,
                    parseMode: ParseMode.Html,
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: _cancellationToken);
            }

            if (messageText.ToLower().Contains("/inventory"))
            {
                hero = new Hero(heroes.Find(person => person.Name.Contains(userName.ToLower())));

                answer = $"У @{hero.Name} в сумке:" +
                    $"{Environment.NewLine}";

                if(hero.InventoryData() == string.Empty)
                {
                    answer += $"{Environment.NewLine}ШАРОМ ПОКАТИ";
                }
                else
                {
                    answer += hero.InventoryData();
                }

                Answer(answer, chatID);
            }

            if (messageText.ToLower().Contains("/use"))
            {
                string useItemId = TrimmingMessage(messageText, "use");

                hero = new Hero(heroes.Find(person => person.Name.Contains(userName.ToLower())));

                answer = string.Empty;
                answer = $"Ты применил{Environment.NewLine}{new Item(useItemId).Name}{Environment.NewLine}" +
                    $"{new Item(useItemId).Description}{Environment.NewLine}";

                if (hero.UsePotion(useItemId))
                {
                    answer += $"Ты стал чище на: {new HealingPotion(useItemId).EffectValue}";
                }
                else
                {
                    answer = $"У тебя нет такого предмета";
                }
                Answer(answer, chatID);
            }

            if (messageText.ToLower().Contains("/equip"))
            {
                string equipItemId = TrimmingMessage(messageText, "equip");

                hero = new Hero(heroes.Find(person => person.Name.Contains(userName.ToLower())));

                answer = string.Empty;
                answer = $"Ты применил{Environment.NewLine}{new Item(equipItemId).Name}{Environment.NewLine}" +
                    $"{new Item(equipItemId).Description}{Environment.NewLine}";

                hero.EquipItem(equipItemId);

                answer += $"Ты стал чище на: {new HealingPotion(equipItemId).EffectValue}";

                Answer(answer, chatID);
            }

            if (messageText.ToLower().Contains("/give"))
            {
                hero = new Hero(heroes.Find(person => person.Name.Contains(userName.ToLower())));

                string questId = TrimmingMessage(messageText, "give");

                answer = string.Empty;

                bool isQuestTaken = false;

                for (int i = 0; i < hero.Quests.Count; i++)
                {
                    if (hero.Quests[i].QuestID == questId)
                    {
                        isQuestTaken = true;
                        break;
                    }
                    else
                    {
                        isQuestTaken = false;
                    }
                }

                if (giveMeQuest.Quests.Find(q => q.QuestID.Contains(questId)) == null)
                {
                    answer = "Такого квеста не существует";
                }

                if (isQuestTaken == true)
                {
                    answer = "&#128519 Выполняй выполняй &#128519";
                }
                else
                {
                    answer += $"<B>{Hi.GreatWords.ToUpper()}!</b>" + Environment.NewLine + Environment.NewLine;
                    answer += $"Ты взял КВЕСТ:" + Environment.NewLine;
                    answer += $"<b>{new QuestBox(questId).Title}</b>";
                    hero.AddQuest(new QuestBox(questId));
                }

                Answer(answer, chatID);
            }

            if (messageText.ToLower().Contains("/randomteleport"))
            {
                answer = string.Empty;

                hero = new Hero(heroes.Find(person => person.Name.Contains(userName.ToLower())));

                int[] coordinates = new int[2] { new SilverDice().GetCoordinate, new SilverDice().GetCoordinate };

                hero.ChangePosition(directions: default, coordinates);

                answer += $"{Hi.ScreemWords} {Hi.ScreemModulateWords.ToLower()} {hero.Name} переместился " +
                    $"в точку:{Environment.NewLine}" +
                    $"Координаты [ X: {hero.PositionX} ] [ Y: {hero.PositionY} ]";

                Answer(answer, chatID);
            }

            if (messageText.ToLower().Contains("/lookaround"))
            {
                hero = new Hero(heroes.Find(person => person.Name.Contains(userName.ToLower())));
                
                answer = string.Empty;

                zoo.HeroLocated(hero.PositionX, hero.PositionY);

                answer += $"Ты находишься в {zoo.LocationMark} области{Environment.NewLine}Вокруг тебя:{Environment.NewLine}{Environment.NewLine}";
                answer += zoo.MobsAround;

                Answer(answer, chatID);
            }

            if (messageText.ToLower().Contains("/atk"))
            {
                _mobName = string.Empty;
                hero = new Hero(heroes.Find(person => person.Name.Contains(userName.ToLower())));
                LootBox loot;
                answer = string.Empty;
                string battleResult;

                _mobName = TrimmingMessage(messageText, "atk");

                if (zoo.MobFight(hero, _mobName, out loot, out battleResult))
                {
                    answer += $"&#127881 <b>! П О Б Е Д А !</b> &#127881{Environment.NewLine}";
                    answer += $"&#128230 ( " +
                        $"&#128176 {loot.Cash} | " +
                        $"&#128167 {string.Format("{0:F3}", loot.Expirience)} | " +
                        $"&#9884 {string.Format("{0:F3}", loot.RankPoints)} ) {Environment.NewLine}{Environment.NewLine}";
                    answer += battleResult + $"был забит оружием \"{hero.Weapon.Name}\"{Environment.NewLine}";
                    hero.TakeLootBox(loot);
                }
                else
                {
                    answer += battleResult;
                }
                Answer(Hi.AttackSmile, chatID);
                Answer(answer, chatID);
            }

            if (messageText.ToLower().Contains("/mob"))
            {
                hero = new Hero(heroes.Find(hero => hero.Name.Contains(userName.ToLower())));

                answer = string.Empty;
                
                _mobName = TrimmingMessage(messageText, "mob");
                
                answer += zoo.MobInfo(_mobName, hero);

                Answer(answer, chatID);
            }

            if (messageText.ToLower().Contains("/info"))
            {
                string itemId = TrimmingMessage(messageText, "info");

                answer = string.Empty;

                answer += $"{new Item(itemId).Name} ";
                answer += $"( &#128176 {new Item(itemId).Coast} )" + Environment.NewLine + Environment.NewLine;
                answer += $"{new Item(itemId).Description}";

                Answer(answer, chatID);
            }

            if (messageText.ToLower().Contains("/abouttoilet"))
            {
                answer = string.Empty;
                answer += $"<i><b>Я оставлю это здесь, что-бы ты не забыл</b></i>{Environment.NewLine}" +
                    $"" +
                    $"{Environment.NewLine}" +
                    $"{Zoo.RED_ZONE} <b>Red Zone</b> {Environment.NewLine}[ X (0, 80) Y (0, 100) ]{Environment.NewLine}" +
                    $"{Zoo.BLUE_ZONE} <b>Blue Zone</b> {Environment.NewLine}[ X (120, 200) Y (100, 200)]{Environment.NewLine}" +
                    $"{Zoo.GREEN_ZONE} <b>Green Zone</b> {Environment.NewLine}[ X (80, 200) Y (0, 55) ]{Environment.NewLine}" +
                    $"{Zoo.BLACK_ZONE} <b>Black Zone</b> {Environment.NewLine}[ X (0, 120) Y (100, 145) ]{Environment.NewLine}" +
                    $"{Zoo.PURPLE_ZONE} <b>Purple Zone</b> {Environment.NewLine}[ X (0, 120) Y (145, 200) ]{Environment.NewLine}" +
                    $"{Zoo.WHITE_ZONE} <b>White Zone</b> {Environment.NewLine}[ X (80, 200) Y (55, 100) ]{Environment.NewLine}" +
                    $"{Environment.NewLine}" +
                    $"<b>Список команд:</b>{Environment.NewLine}" +
                    $"/heroinfo - Показать параметры героя{Environment.NewLine}" +
                    $"/inventory - Чё у меня в сумке?{Environment.NewLine}" +
                    $"/go - Прогуляться{Environment.NewLine}" +
                    $"/lookaround - Поглядеть по сторонам{Environment.NewLine}" +
                    $"/randomteleport - Прыгни куда-нибудь{Environment.NewLine}" +
                    $"/shitshop - Говномагаз{Environment.NewLine}" +
                    $"/gmq - ГивМиЭКвэст{Environment.NewLine}" +
                    $"/upstat - Раскачать характеристики{Environment.NewLine}" +
                    $"/createhero - Создать героя{Environment.NewLine}" +
                    $"/abouttoilet - Про туалет{Environment.NewLine}" +
                    $"{Environment.NewLine}" +
                    $"Сокращения команд{Environment.NewLine}" +
                    $"<i>Квесты</i>{Environment.NewLine}" +
                    $"<code>[ Q ]</code> - инфа о квестах{Environment.NewLine}" +
                    $"<code>[ G ]</code> - взять квест{Environment.NewLine}" +
                    $"<i>Бой с мобами</i>{Environment.NewLine}" +
                    $"<code>[ А ]</code> - атака по выбранному мобу{Environment.NewLine}" +
                    $"<code>[ M ]</code> - инфа о выбранном мобе{Environment.NewLine}" +
                    $"<i>Магазин</i>{Environment.NewLine}" +
                    $"<code>[ B ]</code> - купить предмет{Environment.NewLine}" +
                    $"<code>[ E ]</code> - экипировать предмет{Environment.NewLine}" +
                    $"<code>[ U ]</code> - использовать предмет{Environment.NewLine}" +
                    $"<code>[ I ]</code> - инфа о предмете{Environment.NewLine}" +
                    $"<b>ХОРОШЕЙ ИГРЫ</b>{Environment.NewLine}" +
                    $"<b>Helmut Schlosser</b> <i>v.0.1b</i>";

                await _botClient.SendPhotoAsync(
                    chatId: chatID,
                    photo: "https://disk.yandex.ru/i/yeCLlgN7UD5RAw",
                    caption: answer,
                    parseMode: ParseMode.Html,
                    cancellationToken: _cancellationToken
                    );
            }

            if (messageText.ToLower().Contains("чистить"))
            {
                if (messageText.Contains("@"))
                {
                    string[] messageAttackArr = messageText.Split('@');
                    enemyName = messageAttackArr[1];

                    if (userName == enemyName)
                    {
                        Answer($"Себя можно почистить Чистящим средством!{Environment.NewLine}" +
                            $"Оно есть в магазине{Environment.NewLine}" +
                            $"[ /shitshop ]", chatID);
                        return;
                    }

                    arena = new Arena(userName, enemyName);
                    botArena = new Arena(botName, userName);

                    if (enemyName.ToLower() == botName.ToLower())
                    {
                        answer = arena.AttackNotification + Environment.NewLine + Environment.NewLine +
                            "ПРЕДУПРЕЖДЕНИЕ" + Environment.NewLine +
                            "Арбитр всегда чистит ответ!!" + Environment.NewLine +
                            botArena.CleanUpNotification;
                    }
                    else
                    {
                        answer = arena.CleanUpNotification;
                    }
                }
                else
                {
                    answer = $"А кого ты пытаешься чистить...? @{userName}";
                }
                Answer(answer, chatID);
            }

            DemiGodSpells();
        }

        public async void Answer(string message)
        {
            await _botClient.SendTextMessageAsync(
                chatId: mainChatID,
                text: message,
                parseMode: ParseMode.Html,
                cancellationToken: _cancellationToken);

            new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Save);
        }

        public async void Answer(string message, long ChatId)
        {
            await _botClient.SendTextMessageAsync(
                chatId: ChatId,
                text: message,
                parseMode: ParseMode.Html,
                cancellationToken: _cancellationToken);
        }

        private void ChanceSick()
        {

        }

        public async void DemiGodSpells()
        {
            if (messageText.ToLower().Contains("/hcw@"))
            {
                if (hero.DemiGod)
                {
                    Room = new ToiletRoom();
                    string playerId = TrimmingMessage(messageText, "hcw@");
                    hero = new Hero(MDC.HeroCards.ToList().Find(player => player.Name.Contains(playerId.ToLower())));
                    Room.PlayerInfo(hero);
                    Answer(Room.HeroInfo(), chatID);
                }
                else
                {
                    Answer("Хозяин! Тут кто не поподя, руки свои засовывает туда, куда не стоит! Может его ТОКОМ?!", chatID);
                }
            }

            if (messageText.ToLower().Contains("/ham@"))
            {
                if (hero.DemiGod)
                {
                    long coins = 0;
                    string[] data = TrimmingMessage(messageText, "ham@").Split('.');
                    string playerId = data[0];
                    coins = long.Parse(data[1]);

                    hero = new Hero(MDC.HeroCards.ToList().Find(player => player.Name.Contains(playerId.ToLower())));
                    hero.TakeMoney(coins);
                    Answer($"Озолотили {hero.Name.ToUpper()} на {coins} ГовноТенге", chatID);
                }
                else
                {
                    Answer("Хозяин! Тут кто не поподя, руки свои засовывает туда, куда не стоит! Может его ТОКОМ?!", chatID);
                }
            }

            if (messageText.ToLower().Contains("/hrt@"))
            {
                if (hero.DemiGod)
                {
                    int x = 0;
                    int y = 0;
                    string[] data = TrimmingMessage(messageText, "hrt@").Split('.');
                    string playerId = data[0];
                    x = int.Parse(data[1]);
                    y = int.Parse(data[2]);
                    hero = new Hero(MDC.HeroCards.ToList().Find(player => player.Name.Contains(playerId.ToLower())));
                    hero.ChangePosition(default, new int[] { x, y });
                    Answer($"Переместили {hero.Name.ToUpper()} [ {x} : {y} ]", chatID);
                }
                else
                {
                    Answer("Хозяин! Тут кто не поподя, руки свои засовывает туда, куда не стоит! Может его ТОКОМ?!", chatID);
                }
            }

        }

        public string TrimmingMessage(string message, string trimWord)
        {
            if (message.Contains($"@{botName}"))
            {
                message = message.Replace($"@{botName}", "");
                message = message.Replace($"/{trimWord}", "");
            }
            else
            {
                message = message.Replace($"/{trimWord}", "");
            }
            return message;
        }

        public void GetZooInfo(Zoo zoo)
        {
            this.zoo = zoo;
        }

        private InlineKeyboardMarkup StatUpButtons = new InlineKeyboardMarkup(new[]
        {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData ( "🤢 Токсичность +1", callbackData: "toxic+1" ),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData ( "🧈 Жиры +1", callbackData: "fats+1" ),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData( "🤭 Чрево +1", callbackData: "stomach+1" )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData ( "🧬 Метаболизм +1", callbackData: "metabolism+1" )
                }
        });

        private InlineKeyboardMarkup DestinationButtons2 = new InlineKeyboardMarkup(new[]
        {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData ( "⬆ Север", callbackData: "north" ),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData ( "⬅ Запад", callbackData : "west" ),
                    InlineKeyboardButton.WithCallbackData( "Восток ➡", callbackData : "east" )
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData ( "⬇ Юг", callbackData : "south" )
                }
            });

        private ReplyKeyboardMarkup DestinationButtons = new(new[]
        {
                new KeyboardButton[] { "⬆ Север" },
                new KeyboardButton[] { "⬅ Запад", "Восток ➡" },
                new KeyboardButton[] { "⬇ Юг" },
        })
        {
            ResizeKeyboard = true
        };

        private ReplyKeyboardMarkup CharacteristicsUpdateButtons = new(new[]
        {
                new KeyboardButton[] { "🤢 Токсичность +1" },
                new KeyboardButton[] { "🧈 Жиры +1" },
                new KeyboardButton[] { "🤭 Чрево +1" },
                new KeyboardButton[] { "🧬 Метаболизм +1" },
        })
        {
            ResizeKeyboard = true
        };
    }
}