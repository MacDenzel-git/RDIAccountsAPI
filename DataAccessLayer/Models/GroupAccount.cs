using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class GroupAccount
{
    public int GroupAccountId { get; set; }

    public string GroupAccountName { get; set; } = null!;

    public string GroupAccountNumber { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public double? TotalSharesReceived { get; set; }

    public double? TotalLoans { get; set; }

    public double? InterestExpected { get; set; }

    public double? ActualInterestReceived { get; set; }

    public int Ggbid { get; set; }

    public virtual GroupGorveningBody Ggb { get; set; } = null!;
}
