namespace PlatformSchool.Domain.Models.Student
{
    public class StudentGetModel
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string EnrollmentDateDisplay { get; set; } = string.Empty;
        public string CreationDateDisplay { get; set; } = string.Empty;
    }
}

