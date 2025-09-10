using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlatformSchool.Domain.Base;
using PlatformSchool.Domain.Models.Department;
using PlatformSchool.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PlatformSchool.Persistence.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DepartmentRepository> _logger;
        private readonly string _connectionString;

        public DepartmentRepository(IConfiguration configuration,
                                    ILogger<DepartmentRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("connSchooldb");
        }
        public async Task<OperationResult<DepartmentUpdateModel>> CreateDepartmentAsync(DepartmentCreateModel model)
        {
            OperationResult<DepartmentUpdateModel> result = new OperationResult<DepartmentUpdateModel>();

            try
            {

                if (model is null)
                {
                    result.IsSuccess = false;
                    result.Message = "El modelo de departamento es nulo";
                    return result;
                }

                if (string.IsNullOrEmpty(model.Name) || string.IsNullOrWhiteSpace(model.Name))
                {
                    return OperationResult<DepartmentUpdateModel>.Failure("El nombre del departamento es requerido");
                }

                if (model.Name.Length > 50)
                {
                    return OperationResult<DepartmentUpdateModel>.Failure("El nombre del departamento no puede exceder 50 caracteres");
                }

                if (model.Budget <= 0)
                {
                    return OperationResult<DepartmentUpdateModel>.Failure("El presupuesto del departamento debe ser mayor que cero");
                }

                _logger.LogInformation($"Creating a new department: {model.Name}");

                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("[dbo].[AgregarDepartamento]", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_Name", model.Name);
                        command.Parameters.AddWithValue("@p_Budget", model.Budget);
                        command.Parameters.AddWithValue("@p_StartDate", model.StartDate);
                        command.Parameters.AddWithValue("@p_Administrator", model.Administrator);
                        command.Parameters.AddWithValue("@p_CreateUser", model.CreationUser);

                        SqlParameter p_result = new SqlParameter("@p_result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 1000,
                            Direction = System.Data.ParameterDirection.Output
                        };

                        command.Parameters.Add(p_result);

                        await connection.OpenAsync();

                        var rowsAffected = await command.ExecuteNonQueryAsync();
                        var resultMessage = p_result.Value.ToString();

                        if (rowsAffected > 0)
                        {
                            _logger.LogInformation($"Departamento creado exitosamente: {model.Name}. Resultado: {resultMessage}");
                            var createdDepartment = new DepartmentUpdateModel
                            {
                                Name = model.Name,
                                Budget = model.Budget,
                                StartDate = model.StartDate,
                                Administrator = model.Administrator
                            };
                            result = OperationResult<DepartmentUpdateModel>.Success("Departamento creado exitosamente", createdDepartment);
                        }
                        else
                        {
                            _logger.LogWarning($"No se afectaron filas al crear el departamento: {model.Name}. Resultado: {resultMessage}");
                            result = OperationResult<DepartmentUpdateModel>.Failure("No se afectaron filas al crear el departamento");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error al crear el departamento");
                result = OperationResult<DepartmentUpdateModel>.Failure($"Ocurrio un error al crear el departamento: {ex.Message}");
            }

            return result;

        }

        public async Task<OperationResult<List<DepartmentGetModel>>> GetAllDepartmentsAsync()
        {
            OperationResult<List<DepartmentGetModel>> result = new OperationResult<List<DepartmentGetModel>>();

            try
            {
                using (var connetction = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("[dbo].[ObtenerDepartamentos]", connetction))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        await connetction.OpenAsync();


                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            var departments = new List<DepartmentGetModel>();

                            while (await reader.ReadAsync())
                            {

                                var department = new DepartmentGetModel
                                {

                                    DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Budget = reader.GetDecimal(reader.GetOrdinal("Budget")),
                                    StartDate = reader.GetString("StartDate"),
                                    Administrator = reader.IsDBNull(reader.GetOrdinal("Administrator")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Administrator")),
                                    CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                                    CreationDateDisplay = reader.GetString(reader.GetOrdinal("CreationDateDisplay")),
                                };
                                departments.Add(department);
                            }


                            if (departments.Any())
                            {
                                result = OperationResult<List<DepartmentGetModel>>.Success("Departamentos obtenidos exitosamente", departments);
                            }
                            else
                            {
                                result = OperationResult<List<DepartmentGetModel>>.Failure("No se encontraron departamentos");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error al obtener los departamentos");
                result = OperationResult<List<DepartmentGetModel>>.Failure($"Ocurrio un error al obtener los departamentos: {ex.Message}");
            }
            return result;

        }

        public async Task<OperationResult<DepartmentGetModel>> GetDepartmentByIdAsync(int id)
        {
            OperationResult<DepartmentGetModel> result = new OperationResult<DepartmentGetModel>();

            try
            {
                using (var connetction = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("[dbo].[ObtenerDepartamentoPorId]", connetction))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_DepartmentId", id);
                        await connetction.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                DepartmentGetModel departmentFound = new DepartmentGetModel();

                                while (await reader.ReadAsync())
                                {

                                    departmentFound.DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId"));
                                    departmentFound.Name = reader.GetString(reader.GetOrdinal("Name"));
                                    departmentFound.Budget = reader.GetDecimal(reader.GetOrdinal("Budget"));
                                    departmentFound.StartDate = reader.GetString(reader.GetOrdinal("StartDate"));
                                    departmentFound.Administrator = reader.IsDBNull(reader.GetOrdinal("Administrator")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Administrator"));
                                    departmentFound.CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate"));
                                    departmentFound.CreationDateDisplay = reader.GetString(reader.GetOrdinal("CreationDateDisplay"));

                                }

                                result = OperationResult<DepartmentGetModel>.Success("Departamento obtenido exitosamente", departmentFound);
                            }
                            else
                            {
                                result = OperationResult<DepartmentGetModel>.Failure("No se encontro el departamento");
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error al obtener el departamento");
                result = OperationResult<DepartmentGetModel>.Failure($"Ocurrio un error al obtener el departamento: {ex.Message}");
            }
            return result;
        }

        public async Task<OperationResult<DepartmentUpdateModel>> UpdateDepartmentAsync(DepartmentUpdateModel model)
        {
            OperationResult<DepartmentUpdateModel> result = new OperationResult<DepartmentUpdateModel>();

            try
            {

                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("[dbo].[ActualizarDepartamento]", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DepartmentId", model.DepartmentId);
                        command.Parameters.AddWithValue("@Name", model.Name);
                        command.Parameters.AddWithValue("@Budget", model.Budget);
                        command.Parameters.AddWithValue("@StartDate", model.StartDate);
                        command.Parameters.AddWithValue("@Administrator", model.Administrator);
                        command.Parameters.AddWithValue("@UserMod", model.UserMod);

                        SqlParameter p_result = new SqlParameter("@p_result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 1000,
                            Direction = System.Data.ParameterDirection.Output
                        };

                        command.Parameters.Add(p_result);

                        await connection.OpenAsync();

                        var rowsAffected = await command.ExecuteNonQueryAsync();
                        var resultMessage = p_result.Value.ToString();

                        if (rowsAffected > 0)
                        {
                            _logger.LogInformation($"Departamento actualizado exitosamente: {model.Name}. Resultado: {resultMessage}");
                            var createdDepartment = new DepartmentUpdateModel
                            {
                                Name = model.Name,
                                Budget = model.Budget,
                                StartDate = model.StartDate,
                                Administrator = model.Administrator,
                                DepartmentId = model.DepartmentId,
                            };
                            result = OperationResult<DepartmentUpdateModel>.Success("Departamento actualizado exitosamente", model);
                        }
                        else
                        {
                            _logger.LogWarning($"No se afectaron filas al actualizar el departamento: {model.Name}. Resultado: {resultMessage}");
                            result = OperationResult<DepartmentUpdateModel>.Failure("No se afectaron filas al actualizar el departamento");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error al actualizar el departamento");
                result = OperationResult<DepartmentUpdateModel>.Failure($"Ocurrio un error al actualizar el departamento: {ex.Message}");
            }
            return result;
        }

        public async Task<OperationResult<bool>> DeleteDepartmentAsync(int id)
        {
            OperationResult<bool> result = new OperationResult<bool>();

            try
            {
                if (id <= 0)
                {
                    return OperationResult<bool>.Failure("El ID del departamento debe ser mayor que cero");
                }

                _logger.LogInformation($"Eliminando departamento con ID: {id}");

                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("[dbo].[EliminarDepartamento]", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_DepartmentId", id);

                        SqlParameter p_result = new SqlParameter("@p_result", System.Data.SqlDbType.VarChar)
                        {
                            Size = 1000,
                            Direction = System.Data.ParameterDirection.Output
                        };

                        command.Parameters.Add(p_result);

                        await connection.OpenAsync();

                        var rowsAffected = await command.ExecuteNonQueryAsync();
                        var resultMessage = p_result.Value.ToString();

                        if (rowsAffected > 0)
                        {
                            _logger.LogInformation($"Departamento eliminado exitosamente con ID: {id}. Resultado: {resultMessage}");
                            result = OperationResult<bool>.Success("Departamento eliminado exitosamente", true);
                        }
                        else
                        {
                            _logger.LogWarning($"No se afectaron filas al eliminar el departamento con ID: {id}. Resultado: {resultMessage}");
                            result = OperationResult<bool>.Failure("No se encontro el departamento para eliminar");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error al eliminar el departamento");
                result = OperationResult<bool>.Failure($"Ocurrio un error al eliminar el departamento: {ex.Message}");
            }

            return result;
        }
    }
}
