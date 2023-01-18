using ToilettenArbitrator.ToilettenWars.Items;
using ToilettenArbitrator.ToilettenWars.Quests;

namespace ToilettenArbitrator.ToilettenWars
{
    public class QuestBox
    { 
        private MembersDataContext MDC = new MembersDataContext();

        private string _questID;
        private string _questTitle;
        private string _questDescription;
        private string _whatDo;

        // questData
        // [0] - Что делать (string)
        // [1] - Количество (int)
        // questPrize
        // [0] - экспа (float)
        // [1] - бабло (long)
        // [2] - ранговая экспа (float)
        private string[] _questData;
        private string[] _questPrize = new string[4];

        private int _progressCount;
        private int _totalCount;
        private int _maximumCount;
        private int _minimumCount;

        private long _cash;

        private float _exp;
        private float _rankExp;
        
        private QuestCard _card = new QuestCard();
        private QuestTypes _type;

        public string QuestID => _questID;
        public string Title => _questTitle;
        public string Description => _questDescription;
        public int StartCount => 0;
        public int Progress => _progressCount;
        public string FirstSuspectID => _questData[2];
        public int Total => _totalCount;

        public LootBox Prize => GetLoot();

        public QuestBox()
        {
            GetEmptyQuest();

        }

        public QuestBox(string questID)
        {
            if (questID == null) return;

            if (questID.Contains('.'))
            {
                string[] idData = questID.Split('.');
                _questID = idData[0];
                _progressCount = int.Parse(idData[1]);
            }
            else
            {
                _progressCount = 0;
                _questID = questID;
            }

            _card = MDC.QuestCards.ToList().Find(quest => quest.QuestId.Contains(_questID));

            if (_card == null) return;

            _questTitle = _card.Title;
            _questDescription = _card.Description;
            _type = (QuestTypes)_card.QuestType;

            _questData = _card.QuestData.Split('|');

            _questPrize = new string[4];
            _questPrize = _card.QuestPrize.Split('|');

            _whatDo = _questData[0];
            _totalCount = int.Parse(_questData[1]);

            _exp = (float.Parse(_questPrize[0]) * _totalCount);
            _cash = (long)(float.Parse(_questPrize[1]) * _totalCount);
            _rankExp = (float.Parse(_questPrize[2]) * _totalCount);

            _questDescription += Environment.NewLine + Environment.NewLine;
            _questDescription += $"{_questData[0]}{_totalCount}";
            _questDescription += Environment.NewLine;
            _questDescription += $"( &#128176 {_cash} ) " +
                $"( &#128167 {string.Format("{0:f2}", _exp)} ) " +
                $"( &#9884 {string.Format("{0:f2}", _rankExp)} )";
        }

        public bool Managed()
        {
            _progressCount += 1;
            if (_totalCount == _progressCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private LootBox GetLoot()
        {
            return new LootBox(_questPrize, _totalCount);
        }

        private void GetEmptyQuest()
        {
            _questID = "E";
            _questTitle = "Нет квеста?!";
            _questDescription = "Простое решение! Возьми Квест!";
            
            _exp = 0.0f;
            _cash = 0;
            _rankExp = 0.0f;
            return;
        }

    }
}