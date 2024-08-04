using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class GroupDetailDTO
{
    public int GroupId { get; set; }
    public int GGBId { get; set; }

    public string GroupName { get; set; } = null!;

    public string GroupInitials { get; set; } = null!;

 }
