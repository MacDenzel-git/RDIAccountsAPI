using System;
using System.Collections.Generic;

namespace DataAccessLayer.DataTransferObjects;

public partial class LoanAccountDTO
{
    public int Id { get; set; }

    public string LoanAccountNumber { get; set; } = null!;

    public string MemberAccountNumber { get; set; } = null!;

    public double? RequestedLoanAmount { get; set; }

    public double? LoanInterestAmount { get; set; }

    public double? ExpectedTotalRepaymentAmount { get; set; }

    public DateTime LoanRequestedDate { get; set; }

    public DateTime ExpectedFinalRepaymentDate { get; set; }

    public DateTime ActualPaymentDate { get; set; }

    public int NumberOfDefaultDays { get; set; }

    public double MonthlyInstallmentsByPeriod { get; set; }

    public int LoanConfigurationId { get; set; }

    public double? AmountPaid { get; set; }

    public double? LoanBalance { get; set; }

    public bool? IsApproved { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }
    public int  LoanPeriod { get; set; }
    public string? LoanStatus { get; set; }
    public int  GroupId { get; set; }
    public string? LoanAcccountStatus { get; set; }
    public bool  WaiveMultipleAccountsRestrictions { get; set; }
    public int GroupAccountId { get; set; }
    public int MemberId { get; set; }
    public int GroupDetailId { get; set; }
}
