using System;
using System.Collections.Generic;

namespace OGCP.Curriculums.DAL.Model;

public partial class Profile
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Summary { get; set; }

    public bool IsPublic { get; set; }

    public string? Visibility { get; set; }

    public string DetailLevel { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string Discriminator { get; set; } = null!;

    public string? PersonalGoals { get; set; }

    public string? DesiredJobRole { get; set; }

    public string? Major { get; set; }

    public string? CareerGoals { get; set; }

    public virtual DetailInfo? DetailInfo { get; set; }

    public virtual ICollection<JobExperience> JobExperiences { get; set; } = new List<JobExperience>();

    public virtual ICollection<Education> Educations { get; set; } = new List<Education>();

    public virtual ICollection<Language> Languages { get; set; } = new List<Language>();
}
