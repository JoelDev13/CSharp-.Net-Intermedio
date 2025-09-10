using Microsoft.AspNetCore.Mvc;
using PlatformSchool.Application.Contracts;
using PlatformSchool.Domain.Base;
using PlatformSchool.Domain.Models.Student;

namespace PlatformSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentApiController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentApiController> _logger;

        public StudentApiController(IStudentService studentService, ILogger<StudentApiController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet("ObtenerEstudiantes")]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                _logger.LogInformation("Solicitando todos los estudiantes");
                var result = await _studentService.GetAllStudentsAsync();

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Error al obtener estudiantes: {Error}", result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Se obtuvieron {Count} estudiantes exitosamente", result.Data?.Count ?? 0);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener estudiantes");
                return StatusCode(500, OperationResult<List<StudentGetModel>>.Failure("Error interno del servidor"));
            }
        }

        [HttpGet("ObtenerEstudiantePorId/{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("ID de estudiante invalido: {Id}", id);
                    return BadRequest(OperationResult<StudentGetModel>.Failure("El ID del estudiante debe ser mayor que cero"));
                }

                _logger.LogInformation("Solicitando estudiante con ID: {Id}", id);
                var result = await _studentService.GetStudentByIdAsync(id);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Error al obtener estudiante con ID {Id}: {Error}", id, result.Message);
                    return NotFound(result);
                }

                _logger.LogInformation("Estudiante obtenido exitosamente: {FirstName} {LastName}", result.Data?.FirstName, result.Data?.LastName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener estudiante con ID: {Id}", id);
                return StatusCode(500, OperationResult<StudentGetModel>.Failure("Error interno del servidor"));
            }
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> CreateStudent([FromBody] StudentCreateModel createModel)
        {
            try
            {
                if (createModel == null)
                {
                    _logger.LogWarning("Intento de crear estudiante con modelo nulo");
                    return BadRequest(OperationResult<StudentUpdateModel>.Failure("El modelo de estudiante no puede ser nulo"));
                }

                _logger.LogInformation("Creando nuevo estudiante: {FirstName} {LastName}", createModel.FirstName, createModel.LastName);
                var result = await _studentService.CreateStudentAsync(createModel);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Error al crear estudiante: {Error}", result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Estudiante creado exitosamente con ID: {StudentId}", result.Data?.StudentId);
                return CreatedAtAction(nameof(GetStudentById), new { id = result.Data?.StudentId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear estudiante");
                return StatusCode(500, OperationResult<StudentUpdateModel>.Failure("Error interno del servidor"));
            }
        }

        [HttpPut("Actualizar")]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentUpdateModel updateModel)
        {
            try
            {
                if (updateModel == null)
                {
                    _logger.LogWarning("Intento de actualizar estudiante con modelo nulo");
                    return BadRequest(OperationResult<StudentUpdateModel>.Failure("El modelo de estudiante no puede ser nulo"));
                }

                _logger.LogInformation("Actualizando estudiante con ID: {StudentId}", updateModel.StudentId);
                var result = await _studentService.UpdateStudentAsync(updateModel);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Error al actualizar estudiante con ID {StudentId}: {Error}", updateModel.StudentId, result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Estudiante actualizado exitosamente con ID: {StudentId}", updateModel.StudentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar estudiante con ID: {StudentId}", updateModel?.StudentId);
                return StatusCode(500, OperationResult<StudentUpdateModel>.Failure("Error interno del servidor"));
            }
        }

        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("ID de estudiante invalido para eliminar: {Id}", id);
                    return BadRequest(OperationResult<bool>.Failure("El ID del estudiante debe ser mayor que cero"));
                }

                _logger.LogInformation("Eliminando estudiante con ID: {Id}", id);
                var result = await _studentService.DeleteStudentAsync(id);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Error al eliminar estudiante con ID {Id}: {Error}", id, result.Message);
                    return BadRequest(result);
                }

                _logger.LogInformation("Estudiante eliminado exitosamente con ID: {Id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar estudiante con ID: {Id}", id);
                return StatusCode(500, OperationResult<bool>.Failure("Error interno del servidor"));
            }
        }
    }
}
