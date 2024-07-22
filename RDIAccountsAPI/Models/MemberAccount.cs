using System;
using System.Collections.Generic;

namespace RDIAccountsAPI.Models;

public partial class MemberAccount
{
    public int AccountId { get; set; }

    public string MemberAccountNumber { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public double Balance { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }
}
