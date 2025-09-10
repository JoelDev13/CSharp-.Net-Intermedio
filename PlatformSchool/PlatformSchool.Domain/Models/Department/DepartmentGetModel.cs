

namespace PlatformSchool.Domain.Models.Department
{
    public record DepartmentGetModel
    {
        public int DepartmentId { get; set; }

        public string Name { get; set; }

        public decimal Budget { get; set; }
        public string StartDate { get; set; }
        public string CreationDateDisplay { get; set; }
        public DateTime CreationDate { get; set; }

        public int? Administrator { get; set; }

    }
}
