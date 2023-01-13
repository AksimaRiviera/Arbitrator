using System;
using System.Collections.Generic;

namespace ToilettenArbitrator;

public partial class ItemCard
{
    public long Id { get; set; }

    public string ItemId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public long Cash { get; set; }

    public string Options { get; set; } = null!;
}
