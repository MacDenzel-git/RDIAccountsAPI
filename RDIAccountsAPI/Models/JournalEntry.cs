using System;
using System.Collections.Generic;

namespace RDIAccountsAPI.Models;

public partial class JournalEntry
{
    public long JournalEntryTransId { get; set; }

    public int MemberId { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string MemberName { get; set; } = null!;

    public string? ChqNo { get; set; }

    public double AmountPaid { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public string PayMode { get; set; } = null!;

    public string Rev { get; set; } = null!;

    public string DrCr { get; set; } = null!;

    public string ProcessedStatus { get; set; } = null!;

    public DateTime ProcessDateTime { get; set; }

    public string Processedby { get; set; } = null!;

    public int TranscationTypeId { get; set; }

    public string LoanBalance { get; set; } = null!;

    public string? TranscationDetails { get; set; }

    public string Revreq { get; set; } = null!;

    public bool? IsApproved { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string RequestMethod { get; set; } = null!;

    public DateTime RequestedDate { get; set; }

    public double InterestCalculated { get; set; }

    public string TransactionTypeDescription { get; set; } = null!;

    public string? TransactionStatus { get; set; }

    public string? FromAccountNumber { get; set; }

    public string? ToAccountNumber { get; set; }

    public string? TransactionIdentifier { get; set; }

    public virtual TransactionType TranscationType { get; set; } = null!;
}
