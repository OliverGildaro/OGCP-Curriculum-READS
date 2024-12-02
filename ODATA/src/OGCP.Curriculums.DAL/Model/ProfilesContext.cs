using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OGCP.Curriculums.DAL.Model;

public partial class ProfilesContext : DbContext
{
    public ProfilesContext()
    {
    }

    public ProfilesContext(DbContextOptions<ProfilesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Certification> Certifications { get; set; }

    public virtual DbSet<DetailInfo> DetailInfos { get; set; }

    public virtual DbSet<Education> Educations { get; set; }

    public virtual DbSet<JobExperience> JobExperiences { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DetailInfo>(entity =>
        {
            entity.HasIndex(e => e.ProfileId, "IX_DetailInfos_ProfileId").IsUnique();

            entity.Property(e => e.Phone).HasMaxLength(20);

            entity.HasOne(d => d.Profile).WithOne(p => p.DetailInfo).HasForeignKey<DetailInfo>(d => d.ProfileId);
        });

        modelBuilder.Entity<Education>(entity =>
        {
            entity.Property(e => e.Discriminator).HasMaxLength(21);
            entity.Property(e => e.Institution).HasMaxLength(200);

            entity.HasMany(d => d.Profiles).WithMany(p => p.Educations)
                .UsingEntity<Dictionary<string, object>>(
                    "ProfileEducation",
                    r => r.HasOne<Profile>().WithMany().HasForeignKey("ProfileId"),
                    l => l.HasOne<Education>().WithMany().HasForeignKey("EducationId"),
                    j =>
                    {
                        j.HasKey("EducationId", "ProfileId");
                        j.ToTable("ProfileEducations");
                        j.HasIndex(new[] { "ProfileId" }, "IX_ProfileEducations_ProfileId");
                    });
        });

        modelBuilder.Entity<JobExperience>(entity =>
        {
            entity.HasIndex(e => e.ProfileId, "IX_JobExperiences_ProfileId");

            entity.Property(e => e.Discriminator).HasMaxLength(21);

            entity.HasOne(d => d.Profile).WithMany(p => p.JobExperiences)
                .HasForeignKey(d => d.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.Property(e => e.Checksum)
                .HasMaxLength(1024)
                .HasComputedColumnSql("(CONVERT([varbinary](1024),checksum([Name],[Level])))", false);

            entity.HasMany(d => d.Profiles).WithMany(p => p.Languages)
                .UsingEntity<Dictionary<string, object>>(
                    "ProfileLanguage",
                    r => r.HasOne<Profile>().WithMany().HasForeignKey("ProfileId"),
                    l => l.HasOne<Language>().WithMany().HasForeignKey("LanguageId"),
                    j =>
                    {
                        j.HasKey("LanguageId", "ProfileId");
                        j.ToTable("ProfileLanguages");
                        j.HasIndex(new[] { "ProfileId" }, "IX_ProfileLanguages_ProfileId");
                    });
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasIndex(e => e.LastName, "AK_Profiles_LastName").IsUnique();

            entity.Property(e => e.DesiredJobRole).HasMaxLength(200);
            entity.Property(e => e.DetailLevel).HasMaxLength(18);
            entity.Property(e => e.Discriminator).HasMaxLength(21);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsPublic).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
