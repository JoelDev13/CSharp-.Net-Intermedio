using PlatformSchool.Domain.Models.Student;

namespace PlatformSchool.Application.Services
{
    public class ValidationService
    {
        public static List<string> ValidateStudent(StudentCreateModel model)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.FirstName))
                errors.Add("El nombre es requerido");
            else if (model.FirstName.Length > 50)
                errors.Add("El nombre no puede exceder 50 caracteres");

            if (string.IsNullOrWhiteSpace(model.LastName))
                errors.Add("El apellido es requerido");
            else if (model.LastName.Length > 50)
                errors.Add("El apellido no puede exceder 50 caracteres");

            if (string.IsNullOrWhiteSpace(model.Email))
                errors.Add("El email es requerido");
            else if (model.Email.Length > 100)
                errors.Add("El email no puede exceder 100 caracteres");
            else if (!IsValidEmail(model.Email))
                errors.Add("El formato del email no es valido");

            if (!string.IsNullOrWhiteSpace(model.PhoneNumber) && model.PhoneNumber.Length > 20)
                errors.Add("El numero de telefono no puede exceder 20 caracteres");

            if (model.EnrollmentDate == default)
                errors.Add("La fecha de inscripcion es requerida");

            return errors;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}

