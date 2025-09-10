using PlatformSchool.Domain.Base;
using PlatformSchool.Domain.Models.Student;

namespace PlatformSchool.Application.Contracts
{
    public interface IStudentService
    {
        Task<OperationResult<StudentUpdateModel>> CreateStudentAsync(StudentCreateModel model);
        Task<OperationResult<List<StudentGetModel>>> GetAllStudentsAsync();
        Task<OperationResult<StudentGetModel>> GetStudentByIdAsync(int id);
        Task<OperationResult<StudentUpdateModel>> UpdateStudentAsync(StudentUpdateModel model);
        Task<OperationResult<bool>> DeleteStudentAsync(int id);
    }
}

