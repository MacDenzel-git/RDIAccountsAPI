using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class InterestAccount
{
    public long InterestAccountId { get; set; }

    public string InterestTitle { get; set; } = null!;

    public string LoanAccountId { get; set; } = null!;

    public double Amount { get; set; }

    public string MemberAccountNumber { get; set; } = null!;

    public string Rate { get; set; } = null!;

    public double InitialAmount { get; set; }
}
