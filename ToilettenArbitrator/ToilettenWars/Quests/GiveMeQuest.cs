using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToilettenArbitrator.ToilettenWars.Quests
{
    public class GiveMeQuest
    {
        private const string NAME = "ГивМиЭКвест";
        private MembersDataContext MDC = new MembersDataContext();
        private QuestBox _questBox;
        private List<QuestBox> _questBoxes = new List<QuestBox>();

        private string _questInfo;
        private string _hello = $"{NAME} приветствует тебя!{Environment.NewLine}";

        public string Hello => _hello;

        public QuestBox QuestBox => _questBox;
        public List<QuestBox> Quests => _questBoxes;

        public string AllQuestsInfo => AllQuests();

        public GiveMeQuest() 
        {
            CreatedQuestBoxes();
        }

        private void CreatedQuestBoxes()
        {
            for (int i = 0; i < MDC.QuestCards.ToList().Count; i++)
            {
                _questBoxes.Add(new QuestBox(MDC.QuestCards.ToList()[i].QuestId));
            }
        }

        private string AllQuests()
        {
            _questInfo = string.Empty;
            _questInfo += "Вот что у меня есть для тебя!" + Environment.NewLine;

            for (int i = 0; i < _questBoxes.Count; i++)
            {
                _questInfo += Environment.NewLine + $"<b><u>{_questBoxes[i].Title}</u></b>" + Environment.NewLine;
                _questInfo += $"<b>[ GIVE /give{_questBoxes[i].QuestID} ]</b>" + Environment.NewLine;
                _questInfo += $"<b>[ QUEST /quest{_questBoxes[i].QuestID} ]</b>" + Environment.NewLine;
            }

            return _questInfo;
        }

        public string WhatQuest(string questID)
        {
            _questInfo = string.Empty;

            _questBox = new QuestBox(questID);

            _questInfo += $"КВЕСТ:" +
                $"{Environment.NewLine}" +
                $"<b><u>\"{_questBox.Title}\"</u></b>" +
                $"{Environment.NewLine}" +
                $"{Environment.NewLine}";

            _questInfo += $"Нужно:" +
                $"{Environment.NewLine}" +
                $"<i>{_questBox.Description}</i>" +
                $"{Environment.NewLine}{Environment.NewLine}";

            return _questInfo;
        }
    }
}
