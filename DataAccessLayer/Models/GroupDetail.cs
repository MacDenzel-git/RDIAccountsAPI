using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class GroupDetail
{
    public int GroupId { get; set; }

    public string GroupName { get; set; } = null!;

    public string GroupInitials { get; set; } = null!;

    public double? InterestRate { get; set; }

    public virtual ICollection<MemberDetail> MemberDetails { get; set; } = new List<MemberDetail>();
}
