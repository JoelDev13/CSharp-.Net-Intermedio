using System.ComponentModel.DataAnnotations;

namespace PlatformSchool.Domain.Models.Student
{
    public class StudentUpdateModel
    {
        [Required(ErrorMessage = "El ID del estudiante es requerido")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(50, ErrorMessage = "El apellido no puede exceder 50 caracteres")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        [EmailAddress(ErrorMessage = "El formato del email no es valido")]
        public string Email { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "El numero de telefono no puede exceder 20 caracteres")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "La fecha de inscripcion es requerida")]
        public DateTime EnrollmentDate { get; set; }

        [Required(ErrorMessage = "El usuario de modificacion es requerido")]
        public int UserMod { get; set; }

        public DateTime? ModifyDate { get; set; }
    }
}
