using System;
using System.Collections.Generic;

namespace ToilettenArbitrator
{
    public partial class MobCard
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Rank { get; set; } = null!;
        public double HitPoints { get; set; }
        public double Attack { get; set; }
        public double Defence { get; set; }
        public string Data { get; set; } = null!;
    }
}
