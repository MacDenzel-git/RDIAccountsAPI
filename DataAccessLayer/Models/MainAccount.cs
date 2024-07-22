using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class MainAccount
{
    public int AccountId { get; set; }

    public string AccountName { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string ModifiedBy { get; set; } = null!;

    public DateTime ModifiedDate { get; set; }

    public double Balance { get; set; }
}
