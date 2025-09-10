using Microsoft.Extensions.Logging;
using PlatformSchool.Application.Contracts;
using PlatformSchool.Domain.Base;
using PlatformSchool.Domain.Models.Student;
using PlatformSchool.Domain.Repositories;

namespace PlatformSchool.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly LoggingService _loggingService;

        public StudentService(IStudentRepository studentRepository, ILogger<StudentService> logger)
        {
            _studentRepository = studentRepository;
            _loggingService = new LoggingService(logger);
        }

        public async Task<OperationResult<StudentUpdateModel>> CreateStudentAsync(StudentCreateModel model)
        {
            try
            {
                var validationErrors = ValidationService.ValidateStudent(model);
                if (validationErrors.Any())
                {
                    return OperationResult<StudentUpdateModel>.Failure(string.Join(", ", validationErrors));
                }

                _loggingService.LogInformation("Creando estudiante: {FirstName} {LastName}", model.FirstName, model.LastName);
                return await _studentRepository.CreateStudentAsync(model);
            }
            catch (Exception ex)
            {
                _loggingService.LogError(ex, "Error al crear estudiante");
                return OperationResult<StudentUpdateModel>.Failure("Error interno al crear estudiante");
            }
        }

        public async Task<OperationResult<List<StudentGetModel>>> GetAllStudentsAsync()
        {
            try
            {
                _loggingService.LogInformation("Obteniendo todos los estudiantes");
                return await _studentRepository.GetAllStudentsAsync();
            }
            catch (Exception ex)
            {
                _loggingService.LogError(ex, "Error al obtener estudiantes");
                return OperationResult<List<StudentGetModel>>.Failure("Error interno al obtener estudiantes");
            }
        }

        public async Task<OperationResult<StudentGetModel>> GetStudentByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return OperationResult<StudentGetModel>.Failure("ID de estudiante invalido");
                }

                _loggingService.LogInformation("Obteniendo estudiante con ID: {Id}", id);
                return await _studentRepository.GetStudentByIdAsync(id);
            }
            catch (Exception ex)
            {
                _loggingService.LogError(ex, "Error al obtener estudiante con ID: {Id}", id);
                return OperationResult<StudentGetModel>.Failure("Error interno al obtener estudiante");
            }
        }

        public async Task<OperationResult<StudentUpdateModel>> UpdateStudentAsync(StudentUpdateModel model)
        {
            try
            {
                if (model == null)
                {
                    return OperationResult<StudentUpdateModel>.Failure("Modelo de estudiante es nulo");
                }

                _loggingService.LogInformation("Actualizando estudiante con ID: {Id}", model.StudentId);
                return await _studentRepository.UpdateStudentAsync(model);
            }
            catch (Exception ex)
            {
                _loggingService.LogError(ex, "Error al actualizar estudiante con ID: {Id}", model?.StudentId);
                return OperationResult<StudentUpdateModel>.Failure("Error interno al actualizar estudiante");
            }
        }

        public async Task<OperationResult<bool>> DeleteStudentAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return OperationResult<bool>.Failure("ID de estudiante invalido");
                }

                _loggingService.LogInformation("Eliminando estudiante con ID: {Id}", id);
                return await _studentRepository.DeleteStudentAsync(id);
            }
            catch (Exception ex)
            {
                _loggingService.LogError(ex, "Error al eliminar estudiante con ID: {Id}", id);
                return OperationResult<bool>.Failure("Error interno al eliminar estudiante");
            }
        }
    }
}
