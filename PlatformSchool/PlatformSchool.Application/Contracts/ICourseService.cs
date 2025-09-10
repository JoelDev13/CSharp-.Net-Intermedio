using PlatformSchool.Domain.Base;
using PlatformSchool.Domain.Models;
using PlatformSchool.Domain.Models.Course;

namespace PlatformSchool.Application.Contracts
{
    public interface ICourseService
    {
        Task<OperationResult<List<CourseGetModel>>> GetAllCoursesAsync();
        Task<OperationResult<CourseGetModel>> GetCourseByIdAsync(int id);
        Task<OperationResult<CourseUpdateModel>> CreateCourseAsync(CourseCreateModel model);
        Task<OperationResult<CourseUpdateModel>> UpdateCourseAsync(CourseUpdateModel model);
        Task<OperationResult<bool>> DeleteCourseAsync(int id);
        Task<OperationResult<List<CourseGetModel>>> GetCoursesByDepartmentAsync(int departmentId);
    }
}
