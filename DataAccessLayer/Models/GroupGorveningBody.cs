using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class GroupGorveningBody
{
    public int Ggbid { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<GroupAccount> GroupAccounts { get; set; } = new List<GroupAccount>();
}
