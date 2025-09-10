using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlatformSchool.Domain.Base;
using PlatformSchool.Domain.Models.Student;
using PlatformSchool.Domain.Repositories;
using System.Data;

namespace PlatformSchool.Persistence.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<StudentRepository> _logger;
        private readonly string _connectionString;

        public StudentRepository(IConfiguration configuration, ILogger<StudentRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<OperationResult<StudentUpdateModel>> CreateStudentAsync(StudentCreateModel model)
        {
            try
            {
                _logger.LogInformation("Creando estudiante: {FirstName} {LastName}", model.FirstName, model.LastName);

                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("[dbo].[AgregarEstudiante]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                AddStudentParameters(command, model);
                AddOutputParameters(command);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                var resultMessage = command.Parameters["@p_result"].Value?.ToString() ?? "";
                var studentId = Convert.ToInt32(command.Parameters["@p_StudentId"].Value ?? 0);

                if (resultMessage.Contains("exitosamente"))
                {
                    var createdStudent = CreateStudentUpdateModel(studentId, model);
                    return OperationResult<StudentUpdateModel>.Success("Estudiante creado exitosamente", createdStudent);
                }

                return OperationResult<StudentUpdateModel>.Failure(resultMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear estudiante");
                return OperationResult<StudentUpdateModel>.Failure($"Error al crear estudiante: {ex.Message}");
            }
        }

        private void AddStudentParameters(SqlCommand command, StudentCreateModel model)
        {
            command.Parameters.AddWithValue("@p_FirstName", model.FirstName);
            command.Parameters.AddWithValue("@p_LastName", model.LastName);
            command.Parameters.AddWithValue("@p_Email", model.Email);
            command.Parameters.AddWithValue("@p_PhoneNumber", model.PhoneNumber ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@p_EnrollmentDate", model.EnrollmentDate);
            command.Parameters.AddWithValue("@p_CreateUser", 1);
        }

        private void AddOutputParameters(SqlCommand command)
        {
            command.Parameters.Add(new SqlParameter("@p_result", SqlDbType.VarChar) { Size = 1000, Direction = ParameterDirection.Output });
            command.Parameters.Add(new SqlParameter("@p_StudentId", SqlDbType.Int) { Direction = ParameterDirection.Output });
        }

        private StudentUpdateModel CreateStudentUpdateModel(int studentId, StudentCreateModel model)
        {
            return new StudentUpdateModel
            {
                StudentId = studentId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                EnrollmentDate = model.EnrollmentDate,
                UserMod = 1,
                ModifyDate = DateTime.Now
            };
        }

        public async Task<OperationResult<List<StudentGetModel>>> GetAllStudentsAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("[dbo].[ObtenerEstudiantes]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();

                var students = await MapStudentsFromReader(reader);

                return students.Any()
                    ? OperationResult<List<StudentGetModel>>.Success("Estudiantes obtenidos exitosamente", students)
                    : OperationResult<List<StudentGetModel>>.Failure("No se encontraron estudiantes");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estudiantes");
                return OperationResult<List<StudentGetModel>>.Failure($"Error al obtener estudiantes: {ex.Message}");
            }
        }

        private async Task<List<StudentGetModel>> MapStudentsFromReader(SqlDataReader reader)
        {
            var students = new List<StudentGetModel>();

            while (await reader.ReadAsync())
            {
                students.Add(new StudentGetModel
                {
                    StudentId = reader.GetInt32("StudentId"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    Email = reader.GetString("Email"),
                    PhoneNumber = reader.IsDBNull("PhoneNumber") ? null : reader.GetString("PhoneNumber"),
                    EnrollmentDate = reader.GetDateTime("EnrollmentDate"),
                    CreationDate = reader.GetDateTime("CreationDate"),
                    EnrollmentDateDisplay = reader.GetString("EnrollmentDateDisplay"),
                    CreationDateDisplay = reader.GetString("CreationDateDisplay")
                });
            }

            return students;
        }

        public async Task<OperationResult<StudentGetModel>> GetStudentByIdAsync(int id)
        {
            OperationResult<StudentGetModel> result = new OperationResult<StudentGetModel>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("[dbo].[ObtenerEstudiantePorId]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_StudentId", id);
                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                StudentGetModel studentFound = new StudentGetModel();

                                while (await reader.ReadAsync())
                                {
                                    studentFound.StudentId = reader.GetInt32(reader.GetOrdinal("StudentId"));
                                    studentFound.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                                    studentFound.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                                    studentFound.Email = reader.GetString(reader.GetOrdinal("Email"));
                                    studentFound.PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber"));
                                    studentFound.EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("EnrollmentDate"));
                                    studentFound.CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate"));
                                    studentFound.EnrollmentDateDisplay = reader.GetString(reader.GetOrdinal("EnrollmentDateDisplay"));
                                    studentFound.CreationDateDisplay = reader.GetString(reader.GetOrdinal("CreationDateDisplay"));
                                }

                                result = OperationResult<StudentGetModel>.Success("Estudiante obtenido exitosamente", studentFound);
                            }
                            else
                            {
                                result = OperationResult<StudentGetModel>.Failure("No se encontro el estudiante");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error al obtener el estudiante");
                result = OperationResult<StudentGetModel>.Failure($"Ocurrio un error al obtener el estudiante: {ex.Message}");
            }
            return result;
        }

        public async Task<OperationResult<StudentUpdateModel>> UpdateStudentAsync(StudentUpdateModel model)
        {
            OperationResult<StudentUpdateModel> result = new OperationResult<StudentUpdateModel>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("[dbo].[ActualizarEstudiante]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StudentId", model.StudentId);
                        command.Parameters.AddWithValue("@FirstName", model.FirstName);
                        command.Parameters.AddWithValue("@LastName", model.LastName);
                        command.Parameters.AddWithValue("@Email", model.Email);
                        command.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@EnrollmentDate", model.EnrollmentDate);
                        command.Parameters.AddWithValue("@UserMod", model.UserMod);

                        SqlParameter p_result = new SqlParameter("@p_result", SqlDbType.VarChar)
                        {
                            Size = 1000,
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(p_result);

                        await connection.OpenAsync();

                        await command.ExecuteNonQueryAsync();
                        var resultMessage = p_result.Value?.ToString() ?? "";

                        if (resultMessage.Contains("exitosamente"))
                        {
                            _logger.LogInformation($"Estudiante actualizado exitosamente: {model.FirstName} {model.LastName}. Resultado: {resultMessage}");
                            result = OperationResult<StudentUpdateModel>.Success("Estudiante actualizado exitosamente", model);
                        }
                        else
                        {
                            _logger.LogWarning($"Error al actualizar el estudiante: {model.FirstName} {model.LastName}. Resultado: {resultMessage}");
                            result = OperationResult<StudentUpdateModel>.Failure(resultMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error al actualizar el estudiante");
                result = OperationResult<StudentUpdateModel>.Failure($"Ocurrio un error al actualizar el estudiante: {ex.Message}");
            }

            return result;
        }

        public async Task<OperationResult<bool>> DeleteStudentAsync(int id)
        {
            OperationResult<bool> result = new OperationResult<bool>();

            try
            {
                if (id <= 0)
                {
                    return OperationResult<bool>.Failure("El ID del estudiante debe ser mayor que cero");
                }

                _logger.LogInformation($"Eliminando estudiante con ID: {id}");

                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("[dbo].[EliminarEstudiante]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_StudentId", id);

                        SqlParameter p_result = new SqlParameter("@p_result", SqlDbType.VarChar)
                        {
                            Size = 1000,
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(p_result);

                        await connection.OpenAsync();

                        await command.ExecuteNonQueryAsync();
                        var resultMessage = p_result.Value?.ToString() ?? "";

                        if (resultMessage.Contains("exitosamente"))
                        {
                            _logger.LogInformation($"Estudiante eliminado exitosamente con ID: {id}. Resultado: {resultMessage}");
                            result = OperationResult<bool>.Success("Estudiante eliminado exitosamente", true);
                        }
                        else
                        {
                            _logger.LogWarning($"Error al eliminar el estudiante con ID: {id}. Resultado: {resultMessage}");
                            result = OperationResult<bool>.Failure(resultMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error al eliminar el estudiante");
                result = OperationResult<bool>.Failure($"Ocurrio un error al eliminar el estudiante: {ex.Message}");
            }

            return result;
        }
    }
}
