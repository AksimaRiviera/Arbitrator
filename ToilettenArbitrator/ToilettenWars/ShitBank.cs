using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToilettenArbitrator.ToilettenWars
{
    internal class ShitBank
    {
        private long _MainCell;
        private long _FirstCell;
        private long _SecondCell;
        private long _ThirdCell;
        private long _FourthCell;

        private List<ProfileCard> _ProfileCards = new List<ProfileCard>();
        private BankCard _BankData = new BankCard();

        public ShitBank()
        {
            using (MembersDataContext MDC = new MembersDataContext())
            {
                _BankData = MDC.BankCards.ToList()[0];
                _ProfileCards = MDC.ProfileCards.ToList();
            }
            _MainCell = _BankData.MainCell;
            _FirstCell = _BankData.FirstCell;
            _SecondCell = _BankData.SecondCell;
            _ThirdCell = _BankData.ThirdCell;
            _FourthCell = _BankData.FourthCell;
        }

        public long Credit(long HowMuchCash, string UserName)
        {
            ProfileCard profileCard;
            _ThirdCell -= HowMuchCash;

            if (_ProfileCards.Find(profile => profile.Name.Contains(UserName)) != null)
            {
                profileCard = _ProfileCards.Find(profile => profile.Name.Contains(UserName));
                
                using (MembersDataContext MDC = new MembersDataContext())
                {

                }
            }

            profileCard = new ProfileCard()
            {
                Name = UserName,
                Status = 
            
            };

            using (MembersDataContext MDC = new MembersDataContext())
            {

            }

            return HowMuchCash;
        }

        public void Deposite(long HowMuchCash)
        {

        }

        private class ProfileInfo
        {
            private ProfileCard _Profile;
            protected bool FirstCellEmpty => EmptyCells()[0];
            protected bool SecondCellEmpty => EmptyCells()[1];
            protected bool ThirdCellEmpty => EmptyCells()[2];
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
        }
    }
}
