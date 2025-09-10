using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlatformSchool.Domain.Base;
using PlatformSchool.Domain.Models;
using PlatformSchool.Domain.Models.Course;
using PlatformSchool.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformSchool.Persistence.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CourseRepository> _logger;
        private readonly string _connectionString;

        public CourseRepository(IConfiguration configuration, ILogger<CourseRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<OperationResult<CourseUpdateModel>> CreateCourseAsync(CourseCreateModel model)
        {
            OperationResult<CourseUpdateModel> result = new OperationResult<CourseUpdateModel>();

            try
            {
                if (model is null)
                {
                    result.IsSuccess = false;
                    result.Message = "El modelo de curso es nulo";
                    return result;
                }

                if (string.IsNullOrEmpty(model.Title) || string.IsNullOrWhiteSpace(model.Title))
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

                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("[dbo].[AgregarCurso]", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_Title", model.Title);
                        command.Parameters.AddWithValue("@p_Credits", model.Credits);
                        command.Parameters.AddWithValue("@p_DepartmentId", model.DepartmentId);
                        command.Parameters.AddWithValue("@p_CreateUser", model.CreationUser);

                        SqlParameter p_result = new SqlParameter("@p_result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 1000,
                            Direction = System.Data.ParameterDirection.Output
                        };

                        SqlParameter p_courseId = new SqlParameter("@p_CourseId", System.Data.SqlDbType.Int)
                        {
                            Direction = System.Data.ParameterDirection.Output
                        };

                        command.Parameters.Add(p_result);
                        command.Parameters.Add(p_courseId);

                        await connection.OpenAsync();

                        await command.ExecuteNonQueryAsync();
                        var resultMessage = p_result.Value?.ToString() ?? "";

                        if (resultMessage.Contains("exitosamente"))
                        {
                            var courseId = Convert.ToInt32(p_courseId.Value ?? 0);
                            _logger.LogInformation($"Curso creado exitosamente: {model.Title} con ID: {courseId}. Resultado: {resultMessage}");
                            var createdCourse = new CourseUpdateModel
                            {
                                CourseId = courseId,
                                Title = model.Title,
                                Credits = model.Credits,
                                DepartmentId = model.DepartmentId,
                                UserMod = model.CreationUser,
                                ModifyDate = DateTime.Now
                            };
                            result = OperationResult<CourseUpdateModel>.Success("Curso creado exitosamente", createdCourse);
                        }
                        else
                        {
                            _logger.LogWarning($"Error al crear el curso: {model.Title}. Resultado: {resultMessage}");
                            result = OperationResult<CourseUpdateModel>.Failure(resultMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error al crear el curso");
                result = OperationResult<CourseUpdateModel>.Failure($"Ocurrio un error al crear el curso: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<List<CourseGetModel>>> GetAllCoursesAsync()
        {
            OperationResult<List<CourseGetModel>> result = new OperationResult<List<CourseGetModel>>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("[dbo].[ObtenerCursos]", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            var courses = new List<CourseGetModel>();

                            while (await reader.ReadAsync())
                            {
                                var course = new CourseGetModel
                                {
                                    CourseId = reader.GetInt32(reader.GetOrdinal("CourseID")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Credits = reader.GetInt32(reader.GetOrdinal("Credits")),
                                    DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                    DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName")),
                                    CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                                    CreationDateDisplay = reader.GetString(reader.GetOrdinal("CreationDateDisplay"))
                                };
                                courses.Add(course);
                            }

                            if (courses.Any())
                            {
                                result = OperationResult<List<CourseGetModel>>.Success("Cursos obtenidos exitosamente", courses);
                            }
                            else
                            {
                                result = OperationResult<List<CourseGetModel>>.Failure("No se encontraron cursos");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error al obtener los cursos");
                result = OperationResult<List<CourseGetModel>>.Failure($"Ocurrio un error al obtener los cursos: {ex.Message}");
            }
            return result;
        }

        public async Task<OperationResult<CourseGetModel>> GetCourseByIdAsync(int id)
        {
            OperationResult<CourseGetModel> result = new OperationResult<CourseGetModel>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("[dbo].[ObtenerCursoPorId]", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_CourseId", id);
                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                CourseGetModel courseFound = new CourseGetModel();

                                while (await reader.ReadAsync())
                                {
                                    courseFound.CourseId = reader.GetInt32(reader.GetOrdinal("CourseId"));
                                    courseFound.Title = reader.GetString(reader.GetOrdinal("Title"));
                                    courseFound.Credits = reader.GetInt32(reader.GetOrdinal("Credits"));
                                    courseFound.DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId"));
                                    courseFound.DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName"));
                                    courseFound.CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate"));
                                    courseFound.CreationDateDisplay = reader.GetString(reader.GetOrdinal("CreationDateDisplay"));
                                }

                                result = OperationResult<CourseGetModel>.Success("Curso obtenido exitosamente", courseFound);
                            }
                            else
                            {
                                result = OperationResult<CourseGetModel>.Failure("No se encontro el curso");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error al obtener el curso");
                result = OperationResult<CourseGetModel>.Failure($"Ocurrio un error al obtener el curso: {ex.Message}");
            }
            return result;
        }

        public async Task<OperationResult<CourseUpdateModel>> UpdateCourseAsync(CourseUpdateModel model)
        {
            OperationResult<CourseUpdateModel> result = new OperationResult<CourseUpdateModel>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("[dbo].[ActualizarCurso]", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CourseId", model.CourseId);
                        command.Parameters.AddWithValue("@Title", model.Title);
                        command.Parameters.AddWithValue("@Credits", model.Credits);
                        command.Parameters.AddWithValue("@DepartmentId", model.DepartmentId);
                        command.Parameters.AddWithValue("@UserMod", model.UserMod);

                        SqlParameter p_result = new SqlParameter("@p_result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 1000,
                            Direction = System.Data.ParameterDirection.Output
                        };

                        command.Parameters.Add(p_result);

                        await connection.OpenAsync();

                        await command.ExecuteNonQueryAsync();
                        var resultMessage = p_result.Value?.ToString() ?? "";

                        if (resultMessage.Contains("exitosamente"))
                        {
                            _logger.LogInformation($"Curso actualizado exitosamente: {model.Title}. Resultado: {resultMessage}");
                            result = OperationResult<CourseUpdateModel>.Success("Curso actualizado exitosamente", model);
                        }
                        else
                        {
                            _logger.LogWarning($"Error al actualizar el curso: {model.Title}. Resultado: {resultMessage}");
                            result = OperationResult<CourseUpdateModel>.Failure(resultMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error al actualizar el curso");
                result = OperationResult<CourseUpdateModel>.Failure($"Ocurrio un error al actualizar el curso: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<bool>> DeleteCourseAsync(int id)
        {
            OperationResult<bool> result = new OperationResult<bool>();

            try
            {
                if (id <= 0)
                {
                    return OperationResult<bool>.Failure("El ID del curso debe ser mayor que cero");
                }

                _logger.LogInformation($"Eliminando curso con ID: {id}");

                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("[dbo].[EliminarCurso]", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_CourseId", id);

                        SqlParameter p_result = new SqlParameter("@p_result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 1000,
                            Direction = System.Data.ParameterDirection.Output
                        };

                        command.Parameters.Add(p_result);

                        await connection.OpenAsync();

                        await command.ExecuteNonQueryAsync();
                        var resultMessage = p_result.Value?.ToString() ?? "";

                        if (resultMessage.Contains("exitosamente"))
                        {
                            _logger.LogInformation($"Curso eliminado exitosamente con ID: {id}. Resultado: {resultMessage}");
                            result = OperationResult<bool>.Success("Curso eliminado exitosamente", true);
                        }
                        else
                        {
                            _logger.LogWarning($"Error al eliminar el curso con ID: {id}. Resultado: {resultMessage}");
                            result = OperationResult<bool>.Failure(resultMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error al eliminar el curso");
                result = OperationResult<bool>.Failure($"Ocurrio un error al eliminar el curso: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<List<CourseGetModel>>> GetCoursesByDepartmentAsync(int departmentId)
        {
            OperationResult<List<CourseGetModel>> result = new OperationResult<List<CourseGetModel>>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("[dbo].[ObtenerCursosPorDepartamento]", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_DepartmentId", departmentId);
                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            var courses = new List<CourseGetModel>();

                            while (await reader.ReadAsync())
                            {
                                var course = new CourseGetModel
                                {
                                    CourseId = reader.GetInt32(reader.GetOrdinal("CourseID")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Credits = reader.GetInt32(reader.GetOrdinal("Credits")),
                                    DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                    DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName")),
                                    CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                                    CreationDateDisplay = reader.GetString(reader.GetOrdinal("CreationDateDisplay"))
                                };
                                courses.Add(course);
                            }

                            if (courses.Any())
                            {
                                result = OperationResult<List<CourseGetModel>>.Success("Cursos del departamento obtenidos exitosamente", courses);
                            }
                            else
                            {
                                result = OperationResult<List<CourseGetModel>>.Failure("No se encontraron cursos para este departamento");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error al obtener los cursos del departamento");
                result = OperationResult<List<CourseGetModel>>.Failure($"Ocurrio un error al obtener los cursos del departamento: {ex.Message}");
            }
            return result;
        }
    }
}
