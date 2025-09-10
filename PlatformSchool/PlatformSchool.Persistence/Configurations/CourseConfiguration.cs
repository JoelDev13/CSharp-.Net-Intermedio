


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlatformSchool.Domain.Entities;

namespace PlatformSchool.Persistence.Configurations
{
    public partial class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> entity)
        {
            entity.HasKey(e => e.CourseId).HasName("PK_School.Course");

            entity.HasIndex(e => new { e.DepartmentId, e.CreationDate }, "IDEX_COURSE_CREATION_DATE");

            entity.HasIndex(e => e.DepartmentId, "IDX_course_department");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreationUser).HasDefaultValue(1);
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.ModifyDate).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Course> entity);
    }
}
