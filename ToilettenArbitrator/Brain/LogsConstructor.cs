using System.Text;
using Telegram.Bot.Types;

namespace ToilettenArbitrator.Brain
{
    internal class LogsConstructor
    {
        private static FileStream logStream;
        private static StreamWriter logWriter;
        private static int membersValue;

        private static string logLine, logPath = $"F:\\GitProjects" +
            $"\\Arbitrator" +
            $"\\ToilettenArbitrator" +
            $"\\Brain" +
            $"\\LogsData\\" +
            string.Format("{0:d}", DateTime.Now) + "_IML.txt";
        //F:\GitProjects\Arbitrator\ToilettenArbitrator\Brain
        //F:\GitProjects\Arbitrator\ToilettenArbitrator\Brain\LogsData\
        static LogsConstructor()
        {
            using (MembersDataContext MemberArchive = new MembersDataContext())
            {
                membersValue = MemberArchive.HeroCards.Count();
            }

            logStream = new FileStream(logPath, FileMode.Append);
            logWriter = new StreamWriter(logStream, encoding: Encoding.UTF8);

            logWriter.WriteLine($"> > ЛОГ НАЧАТ < <{Environment.NewLine}" +
                string.Format("[ Дата ] [ {0} ]", DateTime.Now) +
                $"{Environment.NewLine}[ Количество игроков ] " +
                $"[ {membersValue} ]{Environment.NewLine}");

            logWriter.Close();
            logStream.Close();
        }
        // IML - Incoming Message Logs

        public enum SaveLogs
        {
            Save,
            Nope
        }

        public async void ConsoleEcho(Update update, SaveLogs saveLogs)
        {
            logLine = $"> > СООБЩЕНИЕ #{update.Message.MessageId} - UID #{update.Message.From.Id}{Environment.NewLine}" +
                $"{string.Format("> > ДАТА [ {0:d} | {0:t} ]", DateTime.Now)}{Environment.NewLine}" +
                $"> > User Data [ {update.Message.From.Username} * {update.Message.From.FirstName} ]{Environment.NewLine}" +
                $"> > Message * * * [ {update.Message.Text} ]{Environment.NewLine}" +
                $"> > КОНЕЦ СООБЩЕНИЯ - CID #{update.Message.Chat.Id}";

            Console.WriteLine($"{logLine}{Environment.NewLine}{Environment.NewLine}> > О Ж И Д А Н И Е < <");
            Console.WriteLine();

            if (saveLogs == SaveLogs.Save)
            {
                logStream = new FileStream(logPath, FileMode.Append);
                logWriter = new StreamWriter(logStream, encoding: Encoding.UTF8, 777000);

                logWriter.WriteLine(logLine + Environment.NewLine);

                logWriter.Close();
                logStream.Close();
            }
        }

        public static void WhatInMessage(string message, string comment)
        {
            Console.WriteLine($"#################### {comment} ####################");
            Console.WriteLine($"#### [ {message} ] ####");
            Console.WriteLine("########## END ##########");
        }

        public static void WhatInMessage(string[] mesArr, string comment)
        {
            Console.WriteLine($"#################### {comment} ####################");
            Console.Write($"#### [ ");
            for (int i = 0; i < mesArr.Length; i++)
            {
                Console.Write($"{mesArr[i]}*");
            }
            Console.WriteLine(" ] ####");
            Console.WriteLine("########## END ##########");
        }

        public static void WhatInMessage(List<string> Data, string comment)
        {
            Console.WriteLine($"#################### {comment} ####################");
            Console.Write($"#### [ ");
            for (int i = 0; i < Data.Count; i++)
            {
                Console.Write($"{Data[i]}*");
            }
            Console.WriteLine(" ] ####");
            Console.WriteLine("########## END ##########");
        }
    }
}