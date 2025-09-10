

using PlatformSchool.Domain.Base;
using PlatformSchool.Domain.Models.Department;
namespace PlatformSchool.Application.Contracts
{
    public interface IDepartmentService
    {
        Task<OperationResult<List<DepartmentGetModel>>> GetAllDepartmentsAsync();
        Task<OperationResult<DepartmentGetModel>> GetDepartmentByIdAsync(int id);
        Task<OperationResult<DepartmentUpdateModel>> CreateDepartmentAsync(DepartmentCreateModel model);
        Task<OperationResult<DepartmentUpdateModel>> UpdateDepartmentAsync(DepartmentUpdateModel model);
        Task<OperationResult<bool>> DeleteDepartmentAsync(int id);
    }
}
