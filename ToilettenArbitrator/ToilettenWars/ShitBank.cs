using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToilettenArbitrator.ToilettenWars
{
    internal class ShitBank
    {
        private const float CREDIT_PERCENT = 0.06f;
        private const float DEPOSITE_PERCENT = 0.09f;
        private const float DEPOSITE_RETAINED_FUNDS = 0.02f;
        public const string BANK_NAME = "SHIT BANK";

        private long _MainCellId;
        private ulong _MainCell;
        private long _FirstCell;
        private long _SecondCell;
        private long _ThirdCell;
        private long _FourthCell;
        private bool _isCellsFull;

        private List<ProfileCard> _ProfileCards = new List<ProfileCard>();
        private BankCard _BankData = new BankCard();
        private BankCard _RoomData = new BankCard();
        private MembersDataContext MDC = new MembersDataContext();

        public bool CellsFull => _isCellsFull;

        public ShitBank()
        {
            _BankData = MDC.BankCards.ToList()[0];
            _RoomData = MDC.BankCards.ToList()[1];
            _ProfileCards = MDC.ProfileCards.ToList();

            _MainCellId = _BankData.Id;
            _MainCell = _BankData.MainCell;
            _FirstCell = _BankData.FirstCell;
            _SecondCell = _BankData.SecondCell;
            _ThirdCell = _BankData.ThirdCell;
            _FourthCell = _BankData.FourthCell;
        }

        public long Credit(long HowMuchCash, string UserName)
        {
            ProfileCard profileCard;
            ProfileInfo info;

            _ThirdCell -= HowMuchCash;

            if (_ProfileCards.Find(profile => profile.Name.Contains(UserName)) != null)
            {
                int cellNumber;
                profileCard = _ProfileCards.Find(profile => profile.Name.Contains(UserName));
                info = new ProfileInfo(profileCard);

                if (info.IsEmpty[0] && info.IsEmpty[1] && info.IsEmpty[2] != true) { _isCellsFull = true; return 0; }

                for (cellNumber = 0; cellNumber < info.IsEmpty.Length;)
                {
                    if (info.IsEmpty[cellNumber]) continue;
                    cellNumber++;
                }

                switch (cellNumber)
                {
                    case 0:
                        profileCard.Status = $"credit|" +
                        $"{profileCard.Status.Split("|")[1]}|" +
                        $"{profileCard.Status.Split("|")[2]}";
                        profileCard.FirstCell = HowMuchCash;
                        break;

                    case 1:
                        profileCard.Status = $"{profileCard.Status.Split("|")[0]}|" +
                        $"credit|" +
                        $"{profileCard.Status.Split("|")[2]}";
                        profileCard.SecondCell = HowMuchCash;
                        break;

                    case 2:
                        profileCard.Status = $"{profileCard.Status.Split("|")[0]}|" +
                        $"{profileCard.Status.Split("|")[1]}|" +
                        $"credit";
                        profileCard.ThirdCell = HowMuchCash;
                        break;

                    default:
                        break;
                }
                info = new ProfileInfo(profileCard);
                info.SaveProfile();
            }
            else
            {
                profileCard = new ProfileCard()
                {
                    Name = UserName,
                    Status = "credit|none|none",
                    FirstCell = HowMuchCash,
                    SecondCell = 0,
                    ThirdCell = 0
                };
                info = new ProfileInfo(profileCard);
                info.SaveProfile();
            }
            return HowMuchCash;
        }

        public void Deposite(long HowMuchCash, string UserName)
        {
            ProfileCard profileCard;
            ProfileInfo info;

            _SecondCell += (long)(HowMuchCash * DEPOSITE_RETAINED_FUNDS);
            HowMuchCash -= (long)(HowMuchCash * DEPOSITE_RETAINED_FUNDS);

            if (_ProfileCards.Find(profile => profile.Name.Contains(UserName)) != null)
            {
                int cellNumber;
                profileCard = _ProfileCards.Find(profile => profile.Name.Contains(UserName));
                info = new ProfileInfo(profileCard);

                if (info.IsEmpty[0] && info.IsEmpty[1] && info.IsEmpty[2] != true) { _isCellsFull = true; return; }

                for (cellNumber = 0; cellNumber < info.IsEmpty.Length;)
                {
                    if (info.IsEmpty[cellNumber]) continue;
                    cellNumber++;
                }

                switch (cellNumber)
                {
                    case 0:
                        profileCard.Status = $"deposite|" +
                        $"{profileCard.Status.Split("|")[1]}|" +
                        $"{profileCard.Status.Split("|")[2]}";
                        profileCard.FirstCell = HowMuchCash;
                        break;

                    case 1:
                        profileCard.Status = $"{profileCard.Status.Split("|")[0]}|" +
                        $"deposite|" +
                        $"{profileCard.Status.Split("|")[2]}";
                        profileCard.SecondCell = HowMuchCash;
                        break;

                    case 2:
                        profileCard.Status = $"{profileCard.Status.Split("|")[0]}|" +
                        $"{profileCard.Status.Split("|")[1]}|" +
                        $"deposite";
                        profileCard.ThirdCell = HowMuchCash;
                        break;

                    default:
                        break;
                }
                info = new ProfileInfo(profileCard);
                info.SaveProfile();
            }
            else
            {
                profileCard = new ProfileCard()
                {
                    Name = UserName,
                    Status = "deposite|none|none",
                    FirstCell = HowMuchCash,
                    SecondCell = 0,
                    ThirdCell = 0
                };
                info = new ProfileInfo(profileCard);
                info.SaveProfile();
            }
        }

        public void PayFromProduct(long heroCash)
        {
            _BankData.SecondCell += heroCash;
            MDC.Update(_BankData);
            MDC.SaveChanges();
        }

        private class ProfileInfo
        {
            private MembersDataContext MDC = new MembersDataContext();

            private ProfileCard _Profile;
            protected internal bool[] IsEmpty => EmptyCells();
            public ProfileInfo(ProfileCard profile)
            {
                _Profile = profile;
            }

            private bool[] EmptyCells()
            {
                bool[] emptyCells = new bool[3];
                string[] statusMass = new string[3];
                statusMass = _Profile.Status.Split('|');

                for (int i = 0; i < statusMass.Length; i++)
                {
                    if (statusMass[i].Contains("none")) { emptyCells[i] = true; }
                    else { emptyCells[i] = false; }
                }
                return emptyCells;
            }

            protected internal void SaveProfile()
            {
                MDC.Update(_Profile);
                MDC.SaveChanges();
            }
        }
    }
}
