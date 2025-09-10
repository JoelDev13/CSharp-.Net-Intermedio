namespace PlatformSchool.Domain.Models.Course
{
    public record CourseUpdateModel : CourseModel
    {
        public int CourseId { get; set; }
        public int UserMod { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
