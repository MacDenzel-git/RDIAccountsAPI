using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class MemberAccount
{
    public int MemberAccountId { get; set; }

    public int MemberId { get; set; }

    public int GroupId { get; set; }

    public string MemberAccountName { get; set; } = null!;

    public string MemberAccountNumber { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public double? CurrentBalance { get; set; }

    public double? TotalLoans { get; set; }

    public double? TotalInterestAccumulated { get; set; }

    public double? CurrentLoanBalance { get; set; }
}
