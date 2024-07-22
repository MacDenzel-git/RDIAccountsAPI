using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class TransactionType
{
    public int TransactionTypeId { get; set; }

    public string TransactionTypeDescription { get; set; } = null!;

    public virtual ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
}
