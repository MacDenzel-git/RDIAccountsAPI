using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class TransIdCounter
{
    public int Id { get; set; }

    public int GroupId { get; set; }

    public long ShareTran { get; set; }

    public long LoanTran { get; set; }
}
