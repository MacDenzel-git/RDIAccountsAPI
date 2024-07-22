using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class TransactionTypeDTO
{
    public int TransactionTypeId { get; set; }

    public string TransactionTypeDescription { get; set; } = null!;
}
