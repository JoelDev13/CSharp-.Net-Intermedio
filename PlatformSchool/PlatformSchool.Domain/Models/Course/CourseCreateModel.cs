namespace PlatformSchool.Domain.Models.Course
{
    public record CourseCreateModel : CourseModel
    {
        public int CreationUser { get; set; }
    }
}
