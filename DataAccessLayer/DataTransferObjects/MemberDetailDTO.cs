﻿using System;
using System.Collections.Generic;

namespace DataAccessLayer.DataTransferObjects;

public partial class MemberDetailDTO
{
    public int MemberId { get; set; }
    public int GroupId { get; set; }

    public string? MemberName { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public double Occupation { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }
    public string Email { get; set; }
}
