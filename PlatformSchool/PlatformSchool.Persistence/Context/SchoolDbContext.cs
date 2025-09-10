using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PlatformSchool.Domain.Entities;
using PlatformSchool.Persistence.Configurations;



namespace PlatformSchool.Persistence.Context

{
    public partial class SchoolDbContext : DbContext
    {
        public SchoolDbContext()
        {
        }

        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<OnlineCourse> OnlineCourses { get; set; }

        public DbSet<OnsiteCourse> OnsiteCourses { get; set; }
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new OnlineCourseConfiguration());
            modelBuilder.ApplyConfiguration(new OnsiteCourseConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
