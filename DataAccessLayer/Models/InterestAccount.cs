using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class InterestAccount
{
    public long InterestAccountId { get; set; }

    public string InterestAccountName { get; set; } = null!;

    public string InterestAccountNumber { get; set; } = null!;

    public int GroupId { get; set; }

    public double InterestAmountActualCollected { get; set; }

    public double InterestAmountExpected { get; set; }
}
