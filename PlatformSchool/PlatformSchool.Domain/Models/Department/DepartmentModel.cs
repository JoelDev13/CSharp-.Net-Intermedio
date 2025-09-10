

namespace PlatformSchool.Domain.Models.Department
{
    public abstract record DepartmentModel
    {
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        public int Administrator { get; set; }
      
    }
}
