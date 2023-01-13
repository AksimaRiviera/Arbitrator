using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ToilettenArbitrator.Brain
{
    public class AnswerSynapse
    {
        ITelegramBotClient _botClient;
        Update _update;
        CancellationToken _cancellationToken;
        private const long MAIN_CHAT_ID = -1001719641552;

        public AnswerSynapse(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

        }

        public async void Answer(string message)
        {
            await _botClient.SendTextMessageAsync(
                chatId: MAIN_CHAT_ID,
                text: message,
                parseMode: ParseMode.Html,
                cancellationToken: _cancellationToken);

            new LogsConstructor().ConsoleEcho(_update, LogsConstructor.SaveLogs.Save);
        }
    }
}
