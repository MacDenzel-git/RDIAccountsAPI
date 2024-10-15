using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class JournalEntry
{
    public long JournalEntryTransId { get; set; }

    public int MemberId { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string MemberName { get; set; } = null!;

    public string? ChequeNumber { get; set; }

    public double AmountPaid { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public string PayMode { get; set; } = null!;

    public string Rev { get; set; } = null!;

    public string DrCr { get; set; } = null!;

    public string ProcessedStatus { get; set; } = null!;

    public DateTime ProcessDateTime { get; set; }

    public string Processedby { get; set; } = null!;

    public int TranscationTypeId { get; set; }

    public double LoanBalance { get; set; }

    public string? TranscationDetails { get; set; }

    public string Revreq { get; set; } = null!;

    public bool? IsApproved { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string RequestMethod { get; set; } = null!;

    public DateTime RequestedDate { get; set; }

    public string TransactionTypeDescription { get; set; } = null!;

    public string? TransactionStatus { get; set; }

    public string? FromAccountNumber { get; set; }

    public string? ToAccountNumber { get; set; }

    public string? TransactionIdentifier { get; set; }

    public string? MemberBankAccount { get; set; }

    public string? MemberBankName { get; set; }

    public DateTime? DateLoanSent { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? DateModified { get; set; }

    public string? BankTransferTransId { get; set; }

    public bool? IsShareTransaction { get; set; }

    public string? AssociatedLoanAccountNumber { get; set; }

    public virtual TransactionType TranscationType { get; set; } = null!;
}
