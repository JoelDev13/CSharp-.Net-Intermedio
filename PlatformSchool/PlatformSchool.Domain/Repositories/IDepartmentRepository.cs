
using PlatformSchool.Domain.Base;
using PlatformSchool.Domain.Models;
using PlatformSchool.Domain.Models.Department;

namespace PlatformSchool.Domain.Repositories
{
    public interface IDepartmentRepository
    {
        Task<OperationResult<List<DepartmentGetModel>>> GetAllDepartmentsAsync();
        Task<OperationResult<DepartmentGetModel>> GetDepartmentByIdAsync(int id);
        Task<OperationResult<DepartmentUpdateModel>> CreateDepartmentAsync(DepartmentCreateModel model);  
        Task<OperationResult<DepartmentUpdateModel>> UpdateDepartmentAsync(DepartmentUpdateModel model);
        Task<OperationResult<bool>> DeleteDepartmentAsync(int id);
    }
}
