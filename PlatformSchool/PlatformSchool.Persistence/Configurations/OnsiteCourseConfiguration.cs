
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlatformSchool.Domain.Entities;
using System;
using System.Collections.Generic;

namespace PlatformSchool.Persistence.Configurations
{
    public partial class OnsiteCourseConfiguration : IEntityTypeConfiguration<OnsiteCourse>
    {
        public void Configure(EntityTypeBuilder<OnsiteCourse> entity)
        {
            entity.HasKey(e => e.CourseId);

            entity.ToTable("OnsiteCourse");

            entity.Property(e => e.CourseId)
                .ValueGeneratedNever()
                .HasColumnName("CourseID");
            entity.Property(e => e.Days)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Location)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Time).HasColumnType("smalldatetime");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<OnsiteCourse> entity);
    }
}
