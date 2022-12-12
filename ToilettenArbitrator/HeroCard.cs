using System;
using System.Collections.Generic;

namespace ToilettenArbitrator
{
    public partial class HeroCard
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string LevelRank { get; set; } = null!;
        public string Atributes { get; set; } = null!;
        public string Position { get; set; } = null!;
        public string Expirience { get; set; } = null!;
        public string Dirty { get; set; } = null!;
        public string Inventory { get; set; } = null!;
        public string EntryDate { get; set; } = null!;
        public string TimersOne { get; set; } = null!;
        public string TimersTwo { get; set; } = null!;
        public string Skills { get; set; } = null!;
        public string Talents { get; set; } = null!;
        public long Money { get; set; }
    }
}
