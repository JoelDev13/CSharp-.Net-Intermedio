namespace PlatformSchool.Domain.Models.Course
{
    public abstract record CourseModel
    {
        public string Title { get; set; }
        public int Credits { get; set; }
        public int DepartmentId { get; set; }
    }
}
