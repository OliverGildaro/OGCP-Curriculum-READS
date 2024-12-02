using System;
using System.Collections.Generic;

namespace OGCP.Curriculums.DAL.Model;

public partial class DetailInfo
{
    public int Id { get; set; }

    public string Emails { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int ProfileId { get; set; }

    public virtual Profile Profile { get; set; } = null!;
}
