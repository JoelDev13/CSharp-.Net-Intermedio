
using Microsoft.Extensions.Logging;
using PlatformSchool.Application.Contracts;
using PlatformSchool.Domain.Base;
using PlatformSchool.Domain.Models.Department;
using PlatformSchool.Domain.Repositories;

namespace PlatformSchool.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository, ILogger<DepartmentService> logger)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<OperationResult<DepartmentUpdateModel>> CreateDepartmentAsync(DepartmentCreateModel model)
        {
            return await _departmentRepository.CreateDepartmentAsync(model);
        }

        public async Task<OperationResult<List<DepartmentGetModel>>> GetAllDepartmentsAsync()
        {
            return await _departmentRepository.GetAllDepartmentsAsync();
        }

        public async Task<OperationResult<DepartmentGetModel>> GetDepartmentByIdAsync(int id)
        {
            return await _departmentRepository.GetDepartmentByIdAsync(id);
        }

        public async Task<OperationResult<DepartmentUpdateModel>> UpdateDepartmentAsync(DepartmentUpdateModel model)
        {
            return await _departmentRepository.UpdateDepartmentAsync(model);
        }

        public async Task<OperationResult<bool>> DeleteDepartmentAsync(int id)
        {
            return await _departmentRepository.DeleteDepartmentAsync(id);
        }
    }
}
