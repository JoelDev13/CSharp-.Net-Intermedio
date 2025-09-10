using Microsoft.Extensions.Logging;
using PlatformSchool.Application.Contracts;
using PlatformSchool.Domain.Base;
using PlatformSchool.Domain.Models;
using PlatformSchool.Domain.Models.Course;
using PlatformSchool.Domain.Repositories;

namespace PlatformSchool.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<CourseService> _logger;

        public CourseService(ICourseRepository courseRepository, ILogger<CourseService> logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }

        public async Task<OperationResult<CourseUpdateModel>> CreateCourseAsync(CourseCreateModel model)
        {
            try
            {
                if (model == null)
                {
                    return OperationResult<CourseUpdateModel>.Failure("El modelo de curso no puede ser nulo");
                }

                if (string.IsNullOrWhiteSpace(model.Title))
                {
                    return OperationResult<CourseUpdateModel>.Failure("El titulo del curso es requerido");
                }

                if (model.Title.Length > 100)
                {
                    return OperationResult<CourseUpdateModel>.Failure("El titulo del curso no puede exceder 100 caracteres");
                }

                if (model.Credits <= 0 || model.Credits > 10)
                {
                    return OperationResult<CourseUpdateModel>.Failure("Los creditos del curso deben estar entre 1 y 10");
                }

                if (model.DepartmentId <= 0)
                {
                    return OperationResult<CourseUpdateModel>.Failure("El ID del departamento debe ser valido");
                }

                _logger.LogInformation($"Creando nuevo curso: {model.Title}");
                return await _courseRepository.CreateCourseAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el curso");
                return OperationResult<CourseUpdateModel>.Failure($"Error al crear el curso: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<CourseGetModel>>> GetAllCoursesAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los cursos");
                return await _courseRepository.GetAllCoursesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los cursos");
                return OperationResult<List<CourseGetModel>>.Failure($"Error al obtener los cursos: {ex.Message}");
            }
        }

        public async Task<OperationResult<CourseGetModel>> GetCourseByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return OperationResult<CourseGetModel>.Failure("El ID del curso debe ser mayor que cero");
                }

                _logger.LogInformation($"Obteniendo curso con ID: {id}");
                return await _courseRepository.GetCourseByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el curso con ID: {id}");
                return OperationResult<CourseGetModel>.Failure($"Error al obtener el curso: {ex.Message}");
            }
        }

        public async Task<OperationResult<CourseUpdateModel>> UpdateCourseAsync(CourseUpdateModel model)
        {
            try
            {
                if (model == null)
                {
                    return OperationResult<CourseUpdateModel>.Failure("El modelo de curso no puede ser nulo");
                }

                if (model.CourseId <= 0)
                {
                    return OperationResult<CourseUpdateModel>.Failure("El ID del curso debe ser valido");
                }

                if (string.IsNullOrWhiteSpace(model.Title))
                {
                    return OperationResult<CourseUpdateModel>.Failure("El titulo del curso es requerido");
                }

                if (model.Title.Length > 100)
                {
                    return OperationResult<CourseUpdateModel>.Failure("El titulo del curso no puede exceder 100 caracteres");
                }

                if (model.Credits <= 0 || model.Credits > 10)
                {
                    return OperationResult<CourseUpdateModel>.Failure("Los creditos del curso deben estar entre 1 y 10");
                }

                if (model.DepartmentId <= 0)
                {
                    return OperationResult<CourseUpdateModel>.Failure("El ID del departamento debe ser valido");
                }

                _logger.LogInformation($"Actualizando curso con ID: {model.CourseId}");
                return await _courseRepository.UpdateCourseAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el curso con ID: {model.CourseId}");
                return OperationResult<CourseUpdateModel>.Failure($"Error al actualizar el curso: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> DeleteCourseAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return OperationResult<bool>.Failure("El ID del curso debe ser mayor que cero");
                }

                _logger.LogInformation($"Eliminando curso con ID: {id}");
                return await _courseRepository.DeleteCourseAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar el curso con ID: {id}");
                return OperationResult<bool>.Failure($"Error al eliminar el curso: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<CourseGetModel>>> GetCoursesByDepartmentAsync(int departmentId)
        {
            try
            {
                if (departmentId <= 0)
                {
                    return OperationResult<List<CourseGetModel>>.Failure("El ID del departamento debe ser mayor que cero");
                }

                _logger.LogInformation($"Obteniendo cursos del departamento con ID: {departmentId}");
                return await _courseRepository.GetCoursesByDepartmentAsync(departmentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener los cursos del departamento con ID: {departmentId}");
                return OperationResult<List<CourseGetModel>>.Failure($"Error al obtener los cursos del departamento: {ex.Message}");
            }
        }
    }
}
