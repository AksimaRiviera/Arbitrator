using System.Text;
using Telegram.Bot.Types;

namespace ToilettenArbitrator.Brain
{
    internal class LogsConstructor
    {
        private static FileStream logStream;
        private static StreamWriter logWriter;
        private static StreamReader logReader;
        private static int membersValue;

        private static string logLine, logPath = $"{Environment.CurrentDirectory}" +
            $"\\Brain" +
            $"\\LogsData" +
            string.Format("{0:d}", DateTime.Now) + "_IML.txt";


        static LogsConstructor()
        {
            using (MembersDataContext MemberArchive = new MembersDataContext())
            {
                membersValue = MemberArchive.HeroCards.Count();
            }

            logStream = new FileStream(logPath, FileMode.Append);
            logWriter = new StreamWriter(logStream, encoding: Encoding.UTF8, 777000);

            logWriter.WriteLine($"[ - - - ЛОГ НАЧАТ - - - ]{Environment.NewLine}" +
                string.Format("[ {0} ] /", DateTime.Now) +
                $"{Environment.NewLine}[ - - Количество игроков - - ]" +
                $"{Environment.NewLine}[ >>> {membersValue} <<< ]" +
                $"{Environment.NewLine}[ - - - - - - - - ]");

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
            logLine = $"{Environment.NewLine}[ - - - НОВОЕ СООБЩЕНИЕ - - - ]{Environment.NewLine}" +
                $"[ - - - - - - - - - - - - - - - - - - - - - - - - - ]{Environment.NewLine}" +
                $"[ * user name > {update.Message.From.Username} * " +
                $" * first name > {update.Message.From.FirstName} * ]{Environment.NewLine}" +
                $"[ - - - - - - - - - - - - - - - - - - - - - - - - - ]{Environment.NewLine}" +
                $"[ ** text > {update.Message.Text} ** ]{Environment.NewLine}" +
                $"[ - - - - - - - - - - - - - - - - - - - - - - - - - ]{Environment.NewLine}" +
                $"[ * user id > {update.Message.From.Id} *  * chat id > {update.Message.Chat.Id} * ]{Environment.NewLine}" +
                $"[ - - - - - - - - - - - - - - - - - - - - - - - - - ]{Environment.NewLine}" +
                $"[ < Дата _ Время > " +
                string.Format(" < {0:d} _ {0:t} > ]", DateTime.Now) + Environment.NewLine +
                $"[ - - - - - - - - - - - - - - - - - - - - - - - - - ]{Environment.NewLine}";

            Console.WriteLine($"{logLine}{Environment.NewLine} [ - - - ОЖИДАНИЕ - - - ] ");
            Console.WriteLine();

            if (saveLogs == SaveLogs.Save)
            {
                logStream = new FileStream(logPath, FileMode.Append);
                logWriter = new StreamWriter(logStream, encoding: Encoding.UTF8, 777000);

                logWriter.WriteLine(logLine);

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