using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class AuditTrail
{
    public int Id { get; set; }

    public string Operator { get; set; } = null!;

    public string Action { get; set; } = null!;

    public string ServiceName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime TimeStamp { get; set; }

    public string? TableAffected { get; set; }

    public string? RecordTranId { get; set; }
}
