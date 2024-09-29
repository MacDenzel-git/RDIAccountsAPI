using System;
using System.Collections.Generic;

namespace RDIAccountsAPI.Models;

public partial class TransIdCounter
{
    public int GroupId { get; set; }

    public long ShareTran { get; set; }

    public long LoanTran { get; set; }
}
