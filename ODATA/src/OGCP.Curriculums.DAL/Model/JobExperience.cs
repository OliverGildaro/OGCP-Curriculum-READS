using System;
using System.Collections.Generic;

namespace OGCP.Curriculums.DAL.Model;

public partial class JobExperience
{
    public int Id { get; set; }

    public string Company { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Description { get; set; }

    public string Discriminator { get; set; } = null!;

    public int? ProfileId { get; set; }

    public string? Role { get; set; }

    public string? Position { get; set; }

    public virtual Profile? Profile { get; set; }
}
