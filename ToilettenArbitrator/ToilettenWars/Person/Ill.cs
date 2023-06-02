using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToilettenArbitrator.ToilettenWars.Items;
using ToilettenArbitrator.ToilettenWars.Person.IllTypes;

namespace ToilettenArbitrator.ToilettenWars.Person
{
    public class Ill
    {
        //private int[] _parasiteGradeLevels = new int[3] { 
        //    
        //};
        private MembersDataContext MDC = new MembersDataContext();
        private IllCard _card = new IllCard();

        private string _illId;
        private string _title;
        private string _description;
        private string _medicine;
        private string _sufferingParameter;
        private string[] _factorsData;
        private string[] _data;
        private string[] _icoMass = { "&#129439", "", "" };

        private float _mainFactor;
        private float _minimumFactor;
        private float _maximumFactor;

        private int _stepTotal;
        private int _step;
        

        private StringBuilder _info = new StringBuilder();
        private IllType _type;
        private IllGrade _grade;

        public string ID => _illId;
        public string Title => _title;
        public string Description => _description;
        public string Info => _info.ToString();
        public string SufferingParameter => _sufferingParameter;
        public int Step => _step;
        //public int 

        public float MainFactor => _mainFactor;


        public IllType Type => _type;
        public IllGrade Grade => _grade;

        public Ill()
        {
            BaseSettings();
        }
        public Ill(string id)
        {
            IllHeroProgress(id);
            if (_illId == "E" || _illId == null) { BaseSettings(); return; }
            else
            {
                _card = MDC.IllCards.ToList().Find(iData => iData.IllId.Contains(_illId));
                MainSettings();
            }
        }

        private void MainSettings()
        {
            if (_card == null) return;
            _grade = IllGrade.Incubation;

            _title = _card.Title;
            _description = _card.Description;
            _medicine = _card.Medicine;
            _data = _card.Data.Split('|');

            _sufferingParameter = _data[1];

            TypeSettings();
            GetInfo();
        }

        private void IllHeroProgress(string id)
        {
            if (id.Split('.')[0] == null || id.Split('.')[0] == string.Empty) return;

            if (id.Contains('.'))
            {
                _illId = id.Split('.')[0];
                _step = int.Parse(id.Split('.')[1]);
            }
            else
            {
                _step = 0;
                _illId = id;
            }
        }

        private void TypeSettings()
        {
            switch (_data[2])
            {
                case "parasite":
                    _type = IllType.Parasite;
                    _factorsData = new string[4];
                    _factorsData = _card.FactorsData.Split('|');
                    _minimumFactor = float.Parse(_factorsData[0]);
                    _maximumFactor = float.Parse(_factorsData[0]);
                    _mainFactor = float.Parse(_factorsData[0]);
                    _stepTotal = int.Parse(_factorsData[1]);
                    break;
                case "phisical":
                    _type = IllType.Phisical;
                    _factorsData = new string[4];
                    _factorsData = _card.FactorsData.Split('|');
                    _minimumFactor = float.Parse(_factorsData[0]);
                    _maximumFactor = float.Parse(_factorsData[0]);
                    _mainFactor = float.Parse(_factorsData[0]);
                    break;
                case "mental":
                    _type = IllType.Mental;
                    _factorsData = new string[4];
                    _factorsData = _card.FactorsData.Split('|');
                    _minimumFactor = float.Parse(_factorsData[0]);
                    _maximumFactor = float.Parse(_factorsData[0]);
                    _mainFactor = float.Parse(_factorsData[0]);
                    break;

                default:
                    break;
            }
            GradeSettings();
        }

        private void GradeSettings()
        {
            if (_type == null || _type == default) return;

            if (_type == IllType.Parasite)
            {
                switch (_grade)
                {
                    case IllGrade.Incubation:
                        _stepTotal = int.Parse(_factorsData[1]);
                        break;
                    case IllGrade.Main:
                        _stepTotal = int.Parse(_factorsData[2]);
                        break;
                    case IllGrade.Terminal:
                        _stepTotal = int.Parse(_factorsData[3]);
                        break;
                    default:
                        break;
                }
            }

            if (_type == IllType.Phisical || _type == IllType.Mental)
            {
                switch (_grade)
                {
                    case IllGrade.Incubation:
                        _stepTotal = int.Parse(_factorsData[3]);
                        break;
                    case IllGrade.Main:
                        _stepTotal = int.Parse(_factorsData[4]);
                        break;
                    case IllGrade.Terminal:
                        _stepTotal = int.Parse(_factorsData[5]);
                        break;
                    default:
                        break;
                }
            }
        }

        private void GetInfo()
        {
            _info.Clear();

            string gradeMark = string.Empty;
            string stepWord = string.Empty;
            
            switch (_grade)
            {
                case IllGrade.Incubation:
                    gradeMark = "Инкубационный";
                    break;

                case IllGrade.Main:
                    gradeMark = "Основной";
                    break;

                case IllGrade.Terminal:
                    gradeMark = "Терминальный";
                    break;

                default:
                    gradeMark = "Отсутствует";
                    break;
            }

            if (_mainFactor >= 5) stepWord = "действий";
            if (_mainFactor < 5 && _mainFactor > 1) stepWord = "действия";
            if (_mainFactor == 1) stepWord = "действие";

            switch (_type)
            {
                case IllType.Parasite:
                    _info.AppendLine($"Название: {_title}");
                    _info.AppendLine("Тип: Паразит");
                    _info.AppendLine($"Этап: {gradeMark}{Environment.NewLine}");
                    _info.AppendLine($"Информация:");
                    _info.AppendLine($"{_description}{Environment.NewLine}" +
                        $"{_data[0]} {_mainFactor} / {_stepTotal} {stepWord}");
                    _info.AppendLine($"Лекарство: {new Item(_medicine).Name}");
                    break;

                case IllType.Phisical:
                    _info.AppendLine($"Название: {_title}");
                    _info.AppendLine("Тип: Физическая");
                    _info.AppendLine($"Этап: {gradeMark}{Environment.NewLine}");
                    _info.AppendLine($"Информация:");
                    _info.AppendLine($"{_description}{Environment.NewLine}" +
                        $"{_data[0]} {_mainFactor} / {_stepTotal} {stepWord}");
                    _info.AppendLine($"Лекарство: {new Item(_medicine).Name}");
                    break;

                case IllType.Mental:
                    _info.AppendLine($"Название: {_title}");
                    _info.AppendLine("Тип: Ментальная");
                    _info.AppendLine($"Этап: {gradeMark}{Environment.NewLine}");
                    _info.AppendLine($"Информация:");
                    _info.AppendLine($"{_description}{Environment.NewLine}" +
                        $"{_data[0]} {_mainFactor} / {_stepTotal} {stepWord}");
                    _info.AppendLine($"Лекарство: {new Item(_medicine).Name}");
                    break;

                default:
                    _info.AppendLine($"Название: {_title}");
                    _info.AppendLine("Тип: Отсутствует");
                    _info.AppendLine($"Этап: {gradeMark}{Environment.NewLine}");
                    break;
            }
        }

        private void BaseSettings()
        {
            _illId = "E";
            _title = "Ты здоров!";
            _description = string.Empty;
            _mainFactor = 0;
            _minimumFactor = 0;
            _maximumFactor = 0;
            _step = 0;
            _type = default;
            _grade = default;
        }

        public bool Cure(string itemId)
        {
            if (itemId == _medicine)
            {
                BaseSettings();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IllProcess()
        {
            if(_step % _stepTotal == 0)
            {
                _step++;
                return true;
            }
            else
            {
                _step++;
                return false;
            }
        }
    }
}
