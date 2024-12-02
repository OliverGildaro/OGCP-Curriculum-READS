using System;
using System.Collections.Generic;

namespace OGCP.Curriculums.DAL.Model;

public partial class Certification
{
    public int Id { get; set; }

    public string? CertificationName { get; set; }

    public DateTime DateIssued { get; set; }

    public string? Description { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public string? IssuingOrganization { get; set; }

    public int ProfileId { get; set; }
}
