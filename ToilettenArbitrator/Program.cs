using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ToilettenArbitrator.Brain;
using ToilettenArbitrator.ToilettenWars.Person;

var Arbitrator = new TelegramBotClient("5146908025:AAE5lkcA_2UqMi2Rl4eiN3f8Z6MgAwpABk8");

int year, month, day, hour, minute, second, millisecond;

long chatID = 0;
long userID;
string messageText, firstName, lastName, userName, enemyName, botName = "ToilettenArbitratorBot";

List<string> UserHeroData = new List<string>();

int messageID;
Message sentMessage;

year = int.Parse(DateTime.Now.Year.ToString());
month = int.Parse(DateTime.Now.Month.ToString());
day = int.Parse(DateTime.Now.Day.ToString());
hour = int.Parse(DateTime.Now.Hour.ToString());
minute = int.Parse(DateTime.Now.Minute.ToString());
second = int.Parse(DateTime.Now.Second.ToString());
millisecond = int.Parse(DateTime.Now.Millisecond.ToString());

GetData();

using var cts = new CancellationTokenSource();

var reciverOptions = new ReceiverOptions
{
    AllowedUpdates = { },
};

Arbitrator.StartReceiving(
    HandleUpdateAsync,
    HandleErrorAsync,
    reciverOptions,
    cancellationToken: cts.Token);

var me = await Arbitrator.GetMeAsync();
Console.WriteLine(new HelloSynapse().Hello + $"я {me.Username}. Запущен! Ожидаю!{Environment.NewLine}");

string command = Console.ReadLine();
Console.WriteLine();

if (command.ToLower().Contains("res"))
{
    cts.TryReset();
}
else
{
    cts.Cancel();
}

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Type != UpdateType.Message)
        return;
    if (update.Message!.Type != MessageType.Text)
        return;

    chatID = update.Message.Chat.Id;
    messageID = update.Message.MessageId;

    userName = update.Message.From.Username;
    firstName = update.Message.From.FirstName;
    lastName = update.Message.From.LastName;

    if (update.Message.From.Username == null) userName = firstName;
    if (update.Message.From.FirstName == null) firstName = userName;
    if (update.Message.From.LastName == null) lastName = userName;

    messageText = update.Message.Text;

    userID = update.Message.From.Id;

    messageText = update.Message.Text;
    chatID = update.Message.Chat.Id;

    year = update.Message.Date.Year;
    month = update.Message.Date.Month;
    day = update.Message.Date.Day;
    hour = update.Message.Date.Hour;
    minute = update.Message.Date.Minute;
    second = update.Message.Date.Second;
    millisecond = update.Message.Date.Millisecond;

    //new LogsConstructor().ConsoleEcho(update, LogsConstructor.SaveLogs.Save);

    if (messageText != null
                && int.Parse(day.ToString()) >= day
                && int.Parse(hour.ToString()) >= hour
                && int.Parse(minute.ToString()) >= minute
                && int.Parse(second.ToString()) >= second - 10)
    {
        MainSynapse mainSynapse = new MainSynapse(botClient, update, cancellationToken);
        mainSynapse.SynapseAnswer();
    }
}

Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram Api Error: \n[{apiRequestException.ErrorCode}]\n[{apiRequestException.Message}]",
        _ => exception.ToString()
    };
    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

void GetData()
{

    Console.WriteLine($"-------------------------------------------------------");
    Console.WriteLine($" >>> >>> Дата [ День . Месяц . Год ] {day} . {month} . {year} ");
    Console.WriteLine($" >>> >>> Время [ Час : Минута : Секунда ] {hour} : {minute} : {second}");
    Console.WriteLine("-------------------------------------------------------{0}", Environment.NewLine);
}