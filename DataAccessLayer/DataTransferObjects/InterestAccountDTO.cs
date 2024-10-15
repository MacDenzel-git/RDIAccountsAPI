using System;
using System.Collections.Generic;

namespace DataAccessLayer.DataTransferObjects;

public partial class InterestAccountDTO
{
    public long InterestAccountId { get; set; }

    public string InterestAccountName { get; set; } = null!;

    public int GroupId { get; set; } 

    public double InterestAmountActualCollected { get; set; }

    public double InterestAmountExpected { get; set; }

    public string InterestAccountNumber { get; set; } = null!;
}
