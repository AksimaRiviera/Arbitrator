using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ToilettenArbitrator.ToilettenWars;
using ToilettenArbitrator.ToilettenWars.Person;
using ToilettenArbitrator.ToilettenWars.Items;

namespace ToilettenArbitrator.Brain
{
    public class MainSynapse
    {
        private HelloSynapse Hi = new HelloSynapse();

        private int year, month, day, hour, minute, second, millisecond, messageID;

        private long chatID = 0, userID, mainChatID = -1001719641552;
        private string answer, messageText, firstName, lastName, userName, enemyName, botName = "ToilettenArbitratorBot";

        private Arena arena, botArena;
        private ToiletRoom Room;
        private DeviceShop Shop;
        private ShitBank Bank;

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
                            Inventory = "e|e|e|e|e|e|e|e|e|e|e",
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
                
                Shop = new DeviceShop();
                Shop.WhatItemBought = messageText.Substring(4);

                hero = new Hero(heroes.Find(name => name.Name.Contains(userName.ToLower())));

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
                    chatId: userID,
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
                    chatId: userID,
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
                    chatId: userID,
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
                hero = new Hero(heroes.Find(person => person.Name.Contains(userName)));
                answer = $"Cумка @{hero.Name}" +
                    $"{Environment.NewLine}содержит:{Environment.NewLine}";

                answer += hero.InventoryData();

                await _botClient.SendTextMessageAsync(
                    chatId: chatID,
                    text: answer,
                    parseMode: ParseMode.Html,
                    cancellationToken: _cancellationToken);

                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Nope);
            }

            if (messageText.ToLower().Contains("/use"))
            {
                hero = new Hero(heroes.Find(person => person.Name.Contains(userName)));

                answer = string.Empty;
                answer = $"Ты применил{Environment.NewLine}{new Item(messageText.Substring(4)).Name}{Environment.NewLine}" +
                    $"{new Item(messageText.Substring(4)).Description}{Environment.NewLine}";

                hero.UsePotion(messageText.Substring(4));

                answer += $"Ты стал чище на: {new HealingPotion(messageText.Substring(4)).EffectValue}";

                await _botClient.SendTextMessageAsync(
                        chatId: chatID,
                        text: answer,
                        parseMode: ParseMode.Html,
                        cancellationToken: _cancellationToken);

                new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Nope);
            }
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