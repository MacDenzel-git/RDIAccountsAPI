using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class LoanAccountDTO
{
    public int Id { get; set; } 

    public string LoanAccountNumber { get; set; } 

    public DateTime LoanDate { get; set; }

    public double PrincipalAmount { get; set; }

    public DateTime ExpectedRepaymentDate { get; set; }

    public DateTime ActualPaymentDate { get; set; }

    public int NumberOfDefaultDays { get; set; }

    public int LoanConfigurationId { get; set; }

    public string MemberAccountNumber { get; set; } = null!;

    public double? ExpectedTotalRepaymentAmount { get; set; }

    public double? InterestPaidAmount { get; set; }

    public double? PrincipalRepaidAmount { get; set; }

    public double? TotalRepaymentAmount { get; set; }

    public double? Balance { get; set; }

    public bool? IsApproved { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }
}
