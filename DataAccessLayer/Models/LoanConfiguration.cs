using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class LoanConfiguration
{
    public int LoanConfigurationId { get; set; }

    public string LoanDescription { get; set; } = null!;

    public int InterestRate { get; set; }

    public string Createdby { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int RepaymentPeriod { get; set; }

    public int DefaultInterest { get; set; }

    public int InterestAccumulativeDays { get; set; }

    public bool IsComputedDefaultInterest { get; set; }

    public int? GroupId { get; set; }
}
