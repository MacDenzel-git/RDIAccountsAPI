using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class MailingList
{
    public int Id { get; set; }

    public int GroupId { get; set; }

    public string Email { get; set; } = null!;
}
