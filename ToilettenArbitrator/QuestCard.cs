using System;
using System.Collections.Generic;

namespace ToilettenArbitrator;

public partial class QuestCard
{
    public long Id { get; set; }

    public string QuestId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string QuestData { get; set; } = null!;

    public string QuestPrize { get; set; } = null!;

    public long QuestType { get; set; }
}
