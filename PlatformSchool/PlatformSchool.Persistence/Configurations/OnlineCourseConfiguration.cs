using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlatformSchool.Domain.Entities;
using System;
using System.Collections.Generic;

namespace PlatformSchool.Persistence.Configurations
{
    public partial class OnlineCourseConfiguration : IEntityTypeConfiguration<OnlineCourse>
    {
        public void Configure(EntityTypeBuilder<OnlineCourse> entity)
        {
            entity.HasKey(e => e.CourseId);

            entity.ToTable("OnlineCourse");

            entity.Property(e => e.CourseId)
                .ValueGeneratedNever()
                .HasColumnName("CourseID");
            entity.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("URL");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<OnlineCourse> entity);
    }
}
