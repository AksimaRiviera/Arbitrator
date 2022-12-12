using System;
using System.Collections.Generic;

namespace ToilettenArbitrator
{
    public partial class ProfileCard
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Status { get; set; } = null!;
        public long? FirstCell { get; set; }
        public long? SecondCell { get; set; }
        public long? ThirdCell { get; set; }
    }
}
