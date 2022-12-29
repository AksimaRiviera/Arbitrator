using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ToilettenArbitrator.ToilettenWars;
using ToilettenArbitrator.ToilettenWars.Person;
using ToilettenArbitrator.ToilettenWars.Items;
using System.ComponentModel;
using System;

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
        private DeviceShop Shop;
        private ShitBank Bank;
        private Zoo zoo;

        private Hero hero, enemy;
        private List<HeroCard> heroes;

        private Message answerMessage;

        public WhatKeyboardNeed WhatKeyboard { get; }
        public string Answer => answer;

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
        }

        public async void SynapseAnswer()
        {
            using (ToilettenArbitrator.MembersDataContext MemberArchive = new ToilettenArbitrator.MembersDataContext())
            {
                heroes = MemberArchive.HeroCards.ToList();
            }
            
            if(new ToiletRoom(userName, firstName, lastName).SuchHeroExist())
            {
                await _botClient.SendTextMessageAsync(
                    chatId: chatID,
                    text: "Ты кто такой ЕСТЬ?",
                    parseMode: ParseMode.Html,
                    cancellationToken: _cancellationToken);
                
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
                        answer = "Такой персонаж уже есть";
                        await _botClient.SendTextMessageAsync(
                            chatId: chatID,
                            text: answer,
                            parseMode: ParseMode.Html,
                            replyMarkup: new ReplyKeyboardRemove(),
                            cancellationToken: _cancellationToken);
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
                            TimersOne = $"E",
                            TimersTwo = $"E",
                            Skills = $"E",
                            Talents = $"E",
                            Money = money.JustRandom(7, 14)
                        };

                        answer = "Персонаж создан";
                        MDC.HeroCards.Add(NewHero);
                        MDC.SaveChanges();

                        await _botClient.SendTextMessageAsync(
                            chatId: chatID,
                            text: answer,
                            parseMode: ParseMode.Html,
                            replyMarkup: new ReplyKeyboardRemove(),
                            cancellationToken: _cancellationToken);
                    }
                }
            }

            if (messageText.ToLower().Contains("/shitshop"))
            {
                Shop = new DeviceShop();

                await _botClient.SendTextMessageAsync(
                    chatId: ChatID,
                    text: Shop.ShopInfo,
                    parseMode: ParseMode.Html,
                    cancellationToken: _cancellationToken);

                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Nope);

            }

            if (messageText.ToLower().Contains("/buy"))
            {
                Bank = new ShitBank();
                heroes = MDC.HeroCards.ToList();
                
                Shop = new DeviceShop();
                if (messageText.Contains("@ToilettenArbitratorBot"))
                {
                    string itemID = messageText.Replace("@ToilettenArbitratorBot", "");
                    Shop.WhatItemBought = itemID.Substring(4);
                }
                else
                {
                    Shop.WhatItemBought = messageText.Substring(4);
                }

                hero = new Hero(heroes.Find(name => name.Name.Contains(userName.ToLower())));

                if(hero.Money < Shop.PurchasedItem.Coast)
                {
                    await _botClient.SendTextMessageAsync(
                        chatId: ChatID,
                        text: "Нет у тебя ГОВНОТЕНГЕ!!!",
                        parseMode: ParseMode.Html,
                        cancellationToken: _cancellationToken);

                    new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Save);
                }
                else
                {
                    Bank.PayFromProduct(hero.GetMoney(Shop.PurchasedItem.Coast));

                    if (hero.AddItem(Shop.PurchasedItem))
                    {
                        LogsConstructor.WhatInMessage(messageText.Substring(4), "Что вводишь в Hero.AddItem");

                        // Не забудь отнять денег у игрока
                        // Так же добавлять вещь через магазин,
                        // а не прикастовывая новый предмет из воздуха
                        // Так же не забудь записывать деньги в Банк
                        // Который тоже необходимо создать

                        await _botClient.SendTextMessageAsync(
                            chatId: ChatID,
                            text: "С обновкой!!!",
                            parseMode: ParseMode.Html,
                            cancellationToken: _cancellationToken);

                        new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Save);
                    }
                    else
                    {
                        await _botClient.SendTextMessageAsync(
                            chatId: ChatID,
                            text: "Твой рюкзак полон!!!",
                            parseMode: ParseMode.Html,
                            cancellationToken: _cancellationToken);

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

                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Save);
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
                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Save);

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
                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Save);

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
                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Save);

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
                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Save);

            }

            if (messageText.ToLower().Contains(botName + " " + "привет") || messageText.ToLower().Contains(botName + " " + "здравствуй"))
            {
                answer = Hi.Hello + firstName + " " + lastName;
                await _botClient.SendTextMessageAsync(
                    chatId: chatID,
                    text: answer,
                    parseMode: ParseMode.Html,
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: _cancellationToken);
                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Save);
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
                        await _botClient.SendTextMessageAsync(
                            chatId: chatID,
                            text: answer,
                            parseMode: ParseMode.Html,
                            cancellationToken: _cancellationToken);

                        new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Save);
                    }
                    else
                    {
                        answer = arena.AttackNotification;
                        await _botClient.SendTextMessageAsync(
                            chatId: chatID,
                            text: answer,
                            parseMode: ParseMode.Html,
                            cancellationToken: _cancellationToken);

                        new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Save);
                    }
                }
                else
                {
                    answer = $"А кого ты пытаешься бить...? @{userName}";
                    await _botClient.SendTextMessageAsync(
                        chatId: chatID,
                        text: answer,
                        parseMode: ParseMode.Html,
                        cancellationToken: _cancellationToken);

                    new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Save);
                }
            }

            if (messageText.ToLower().Contains("/heroinfo"))
            {
                Room = new ToiletRoom(userName, firstName, lastName);
                answer = Room.HeroInfo();

                await _botClient.SendTextMessageAsync(
                    chatId: chatID,
                    text: answer,
                    parseMode: ParseMode.Html,
                    cancellationToken: _cancellationToken);

                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Save);
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

                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Save);
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

                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Save);
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

                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Save);
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

                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Nope);
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

                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Nope);
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

                await _botClient.SendTextMessageAsync(
                    chatId: chatID,
                    text: answer,
                    parseMode: ParseMode.Html,
                    cancellationToken: _cancellationToken);

                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Nope);
            }

            if (messageText.ToLower().Contains("/use"))
            {
                string useItemId = string.Empty;

                if (messageText.Contains("@ToilettenArbitratorBot"))
                {
                    useItemId = messageText.Replace("@ToilettenArbitratorBot", "");
                    useItemId = useItemId.Replace("/use", "");
                }
                else
                {
                    useItemId = messageText.Replace("/use", "");
                }

                hero = new Hero(heroes.Find(person => person.Name.Contains(userName)));

                answer = string.Empty;
                answer = $"Ты применил{Environment.NewLine}{new Item(useItemId).Name}{Environment.NewLine}" +
                    $"{new Item(useItemId).Description}{Environment.NewLine}";

                if (hero.UsePotion(useItemId))
                {

                    answer += $"Ты стал чище на: {new HealingPotion(useItemId).EffectValue}";

                    await _botClient.SendTextMessageAsync(
                            chatId: chatID,
                            text: answer,
                            parseMode: ParseMode.Html,
                            cancellationToken: _cancellationToken);

                    new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Nope);

                }
                else
                {
                    answer = $"У тебя нет такого предмета";

                    await _botClient.SendTextMessageAsync(
                            chatId: chatID,
                            text: answer,
                            parseMode: ParseMode.Html,
                            cancellationToken: _cancellationToken);

                    new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Nope);

                }
            }

            if (messageText.ToLower().Contains("/equip"))
            {
                string equipItemId = string.Empty;

                if (messageText.Contains("@ToilettenArbitratorBot"))
                {
                    equipItemId = messageText.Replace("@ToilettenArbitratorBot", "");
                    equipItemId = equipItemId.Replace("/equip", "");
                }
                else
                {
                    equipItemId = messageText.Replace("/equip", "");
                }

                hero = new Hero(heroes.Find(person => person.Name.Contains(userName.ToLower())));

                answer = string.Empty;
                answer = $"Ты применил{Environment.NewLine}{new Item(equipItemId).Name}{Environment.NewLine}" +
                    $"{new Item(equipItemId).Description}{Environment.NewLine}";

                hero.EquipItem(equipItemId);

                answer += $"Ты стал чище на: {new HealingPotion(equipItemId).EffectValue}";

                await _botClient.SendTextMessageAsync(
                        chatId: chatID,
                        text: answer,
                        parseMode: ParseMode.Html,
                        cancellationToken: _cancellationToken);

                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Nope);
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

                await _botClient.SendTextMessageAsync(
                        chatId: chatID,
                        text: answer,
                        parseMode: ParseMode.Html,
                        cancellationToken: _cancellationToken);

                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Nope);
            }

            if (messageText.ToLower().Contains("/lookaround"))
            {
                hero = new Hero(heroes.Find(person => person.Name.Contains(userName.ToLower())));
                
                answer = string.Empty;

                zoo.HeroLocated(hero.PositionX, hero.PositionY);

                answer += $"Ты находишься в {zoo.LocationMark} области{Environment.NewLine}Вокруг тебя:{Environment.NewLine}{Environment.NewLine}";
                answer += zoo.MobsAround;

                await _botClient.SendTextMessageAsync(
                        chatId: chatID,
                        text: answer,
                        parseMode: ParseMode.Html,
                        cancellationToken: _cancellationToken);

                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Nope);
            }

            if (messageText.ToLower().Contains("/atk"))
            {
                _mobName = string.Empty;
                hero = new Hero(heroes.Find(person => person.Name.Contains(userName.ToLower())));
                LootBox loot;
                answer = string.Empty;
                string battleResult;

                if (messageText.Contains("@ToilettenArbitratorBot"))
                {
                    _mobName = messageText.Replace("@ToilettenArbitratorBot", "");
                    _mobName = _mobName.Replace("/atk", "");
                }
                else
                {
                    _mobName = messageText.Replace("/atk", "");
                }

                if (zoo.MobFight(hero, _mobName, out loot, out battleResult))
                {
                    answer += $"&#127881 <b>! П О Б Е Д А !</b> &#127881{Environment.NewLine}{Environment.NewLine}";
                    answer += battleResult + $"был забит оружием \"{hero.Weapon.Name}\"{Environment.NewLine}";
                    answer += $"{Environment.NewLine}&#128230 <b>НАГРАДА ИЗ КОРОБКИ</b>{Environment.NewLine}" +
                        $"{loot.Expirience} <i>ОПЫТА</i>{Environment.NewLine}" +
                        $"{loot.Cash} &#128169{Environment.NewLine}" +
                        $"{loot.RankPoints} <i>РАНГОВОГО ОПЫТА</i>";
                    hero.TakeLootBox(loot);
                }
                else
                {
                    answer += battleResult;
                }

                await _botClient.SendTextMessageAsync(
                        chatId: chatID,
                        text: answer,
                        parseMode: ParseMode.Html,
                        cancellationToken: _cancellationToken);

                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Nope);
            }

            if (messageText.ToLower().Contains("/mobinfo"))
            {
                answer = string.Empty;
                _mobName = string.Empty;
                if (messageText.Contains("@ToilettenArbitratorBot"))
                {
                    _mobName = messageText.Replace("@ToilettenArbitratorBot", "");
                    _mobName = _mobName.Replace("/mobinfo", "");
                }
                else
                {
                    _mobName = messageText.Replace("/mobinfo", "");
                }

                answer += zoo.MobInfo(_mobName);

                await _botClient.SendTextMessageAsync(
                        chatId: chatID,
                        text: answer,
                        parseMode: ParseMode.Html,
                        cancellationToken: _cancellationToken);

                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Nope);

            }

            if (messageText.ToLower().Contains("/abouttoilet"))
            {
                answer = string.Empty;
                answer += $"<i><b>Я оставлю это здесь, что-бы ты не забыл</b></i>{Environment.NewLine}" +
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
                    $"/upstat - Раскачать характеристики{Environment.NewLine}" +
                    $"/shitshop - Говномагаз{Environment.NewLine}" +
                    $"/createhero - Создать героя{Environment.NewLine}" +
                    $"/abouttoilet - Про туалет{Environment.NewLine}" +
                    $"{Environment.NewLine}" +
                    $"<b>ХОРОШЕЙ ИГРЫ</b> v.0.1a";

                await _botClient.SendPhotoAsync(
                    chatId: chatID,
                    photo: "https://disk.yandex.ru/i/yeCLlgN7UD5RAw",
                    caption: answer,
                    parseMode: ParseMode.Html,
                    cancellationToken: _cancellationToken
                    );

                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Nope);

            }
            //if (messageText.ToLower().Contains("/cleanroom"))
            //{
            //    Bank.;
            //}
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