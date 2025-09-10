namespace PlatformSchool.Domain.Models.Department
{
    public record DepartmentCreateModel : DepartmentModel
    {
        public int CreationUser { get; set; }
    }
}
