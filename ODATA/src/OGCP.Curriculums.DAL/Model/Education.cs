using System;
using System.Collections.Generic;

namespace OGCP.Curriculums.DAL.Model;

public partial class Education
{
    public int Id { get; set; }

    public string Institution { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Discriminator { get; set; } = null!;

    public string? Degree { get; set; }

    public string? ProjectTitle { get; set; }

    public string? Supervisor { get; set; }

    public string? Summary { get; set; }

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}
