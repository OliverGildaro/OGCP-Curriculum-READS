using System;
using System.Collections.Generic;

namespace OGCP.Curriculums.DAL.Model;

public partial class Language
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Level { get; set; } = null!;

    public byte[]? Checksum { get; set; }

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}
