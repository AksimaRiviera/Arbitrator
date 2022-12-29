using System;
using System.Collections.Generic;

namespace ToilettenArbitrator
{
    public partial class BankCard
    {
        public long Id { get; set; }
        public ulong MainCell { get; set; }
        public long FirstCell { get; set; }
        public long SecondCell { get; set; }
        public long ThirdCell { get; set; }
        public long FourthCell { get; set; }
    }
}
