using Microsoft.AspNetCore.Mvc;
using PlatformSchool.Application.Contracts;
using PlatformSchool.Domain.Models.Department;

namespace PlatformSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentApiController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentApiController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpGet("ObtenerDepartamentos")]
        public async Task<IActionResult> Get()
        {
            var result = await _departmentService.GetAllDepartmentsAsync();

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("ObtenerDeptoPorId")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _departmentService.GetDepartmentByIdAsync(id);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("Guardar")]
        public async Task<IActionResult> Post([FromBody] DepartmentCreateModel createModel)
        {
            var result = await _departmentService.CreateDepartmentAsync(createModel);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("Actualizar")]
        public async Task<IActionResult> Put([FromBody] DepartmentUpdateModel updateModel)
        {
            var result = await _departmentService.UpdateDepartmentAsync(updateModel);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _departmentService.DeleteDepartmentAsync(id);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
