using Microsoft.AspNetCore.Mvc;
using PlatformSchool.Application.Contracts;
using PlatformSchool.Domain.Models;
using PlatformSchool.Domain.Models.Course;
using PlatformSchool.Domain.Base;

namespace PlatformSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseApiController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<CourseApiController> _logger;

        public CourseApiController(ICourseService courseService, ILogger<CourseApiController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        [HttpGet("ObtenerCursos")]
        public async Task<IActionResult> GetAllCourses()
        {
            try
            {
                _logger.LogInformation("Solicitando todos los cursos");
                var result = await _courseService.GetAllCoursesAsync();

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Error al obtener cursos: {Error}", result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Se obtuvieron {Count} cursos exitosamente", result.Data?.Count ?? 0);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener cursos");
                return StatusCode(500, OperationResult<List<CourseGetModel>>.Failure("Error interno del servidor"));
            }
        }

        [HttpGet("ObtenerCursoPorId/{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("ID de curso inválido: {Id}", id);
                    return BadRequest(OperationResult<CourseGetModel>.Failure("El ID del curso debe ser mayor que cero"));
                }

                _logger.LogInformation("Solicitando curso con ID: {Id}", id);
                var result = await _courseService.GetCourseByIdAsync(id);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Error al obtener curso con ID {Id}: {Error}", id, result.Message);
                    return NotFound(result);
                }

                _logger.LogInformation("Curso obtenido exitosamente: {Title}", result.Data?.Title);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener curso con ID: {Id}", id);
                return StatusCode(500, OperationResult<CourseGetModel>.Failure("Error interno del servidor"));
            }
        }

        [HttpGet("ObtenerCursosPorDepartamento/{departmentId}")]
        public async Task<IActionResult> GetCoursesByDepartment(int departmentId)
        {
            try
            {
                if (departmentId <= 0)
                {
                    _logger.LogWarning("ID de departamento inválido: {DepartmentId}", departmentId);
                    return BadRequest(OperationResult<List<CourseGetModel>>.Failure("El ID del departamento debe ser mayor que cero"));
                }

                _logger.LogInformation("Solicitando cursos del departamento con ID: {DepartmentId}", departmentId);
                var result = await _courseService.GetCoursesByDepartmentAsync(departmentId);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Error al obtener cursos del departamento {DepartmentId}: {Error}", departmentId, result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Se obtuvieron {Count} cursos del departamento {DepartmentId}", result.Data?.Count ?? 0, departmentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener cursos del departamento: {DepartmentId}", departmentId);
                return StatusCode(500, OperationResult<List<CourseGetModel>>.Failure("Error interno del servidor"));
            }
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateModel createModel)
        {
            try
            {
                if (createModel == null)
                {
                    _logger.LogWarning("Intento de crear curso con modelo nulo");
                    return BadRequest(OperationResult<CourseUpdateModel>.Failure("El modelo de curso no puede ser nulo"));
                }

                _logger.LogInformation("Creando nuevo curso: {Title}", createModel.Title);
                var result = await _courseService.CreateCourseAsync(createModel);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Error al crear curso: {Error}", result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Curso creado exitosamente con ID: {CourseId}", result.Data?.CourseId);
                return CreatedAtAction(nameof(GetCourseById), new { id = result.Data?.CourseId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear curso");
                return StatusCode(500, OperationResult<CourseUpdateModel>.Failure("Error interno del servidor"));
            }
        }

        [HttpPut("Actualizar")]
        public async Task<IActionResult> UpdateCourse([FromBody] CourseUpdateModel updateModel)
        {
            try
            {
                if (updateModel == null)
                {
                    _logger.LogWarning("Intento de actualizar curso con modelo nulo");
                    return BadRequest(OperationResult<CourseUpdateModel>.Failure("El modelo de curso no puede ser nulo"));
                }

                _logger.LogInformation("Actualizando curso con ID: {CourseId}", updateModel.CourseId);
                var result = await _courseService.UpdateCourseAsync(updateModel);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Error al actualizar curso con ID {CourseId}: {Error}", updateModel.CourseId, result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Curso actualizado exitosamente con ID: {CourseId}", updateModel.CourseId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar curso con ID: {CourseId}", updateModel?.CourseId);
                return StatusCode(500, OperationResult<CourseUpdateModel>.Failure("Error interno del servidor"));
            }
        }

        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("ID de curso inválido para eliminar: {Id}", id);
                    return BadRequest(OperationResult<bool>.Failure("El ID del curso debe ser mayor que cero"));
                }

                _logger.LogInformation("Eliminando curso con ID: {Id}", id);
                var result = await _courseService.DeleteCourseAsync(id);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Error al eliminar curso con ID {Id}: {Error}", id, result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Curso eliminado exitosamente con ID: {Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar curso con ID: {Id}", id);
                return StatusCode(500, OperationResult<bool>.Failure("Error interno del servidor"));
            }
        }
    }
}
