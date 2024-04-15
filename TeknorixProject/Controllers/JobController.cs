using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TeknorixProject.Models.Request;
using TeknorixProject.Models.Response;

namespace TeknorixProject.Controllers
{
    public class JobController : ApiController
    {
        private readonly string _connectionString;
        private readonly LogSetting _logSetting;

        public JobController()
        {
            _logSetting = new LogSetting();
            _connectionString = _logSetting.GetConnectionString("ConString");
        }

        [HttpPost]
        [Route("createJob")]
        public JobApplicationResponse CreateJob([FromBody] ApplyjobRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.title) || string.IsNullOrEmpty(request.description) || request.departmentId == 0 || request.locationId == 0 || request.closingDate == null)
                {
                    throw new ArgumentException("Invalid input parameters");
                }
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand("ManageJobDetails", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Flag", "I");
                        command.Parameters.AddWithValue("@id", DBNull.Value);
                        command.Parameters.AddWithValue("@Title", request.title);
                        command.Parameters.AddWithValue("@Description", request.description);
                        command.Parameters.AddWithValue("@LocationId", request.locationId);
                        command.Parameters.AddWithValue("@DepartmentId", request.departmentId);
                        command.Parameters.AddWithValue("@ClosingDate", request.closingDate);

                        command.ExecuteNonQuery();
                    }
                }
                return new JobApplicationResponse { Success = true, Message = "Job created successfully" };

            }
            catch (Exception ex)
            {

                return new JobApplicationResponse { Success = false, Message = "An error occurred while updating job" };
            }
        }


        [HttpPut]
        [Route("JobUpdate")]
        public JobApplicationResponse UpdateJob([FromBody] UpdateJob request)
        {
            try
            {

                if (string.IsNullOrEmpty(request.title) || string.IsNullOrEmpty(request.description) || request.departmentId == 0 || request.locationId == 0 || request.closingDate == null)
                {

                    throw new ArgumentException("Invalid input parameters");
                }
                else
                {

                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {

                        connection.Open();
                        using (SqlCommand command = new SqlCommand("ManageJobDetails", connection))
                        {

                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Flag", "U");
                            command.Parameters.AddWithValue("@id", request.id);
                            command.Parameters.AddWithValue("@Title", request.title);
                            command.Parameters.AddWithValue("@Description", request.description);
                            command.Parameters.AddWithValue("@LocationId", request.locationId);
                            command.Parameters.AddWithValue("@DepartmentId", request.departmentId);
                            command.Parameters.AddWithValue("@ClosingDate", request.closingDate);

                            command.ExecuteNonQuery();
                        }
                    }

                    return new JobApplicationResponse { Success = true, Message = "Job updated successfully" };
                }
            }
            catch (Exception ex)
            {

                return new JobApplicationResponse { Success = false, Message = "An error occurred while updating job" };
            }
        }
        [HttpGet]
        [Route("jobs/{id}")]
        public async Task<JobApplicationResponse> GetJobAsync(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("ManageJobDetails", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Flag", "S");
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var job = MapJobFromReader(reader);
                                return new JobApplicationResponse { Success = true, Job = job };
                            }
                            else
                            {
                                return new JobApplicationResponse { Success = false, Message = "Job not found" };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new JobApplicationResponse { Success = false, Message = "An error occurred while fetching job details" };
            }
        }

        private JobResponseModel MapJobFromReader(SqlDataReader reader)
        {
            return new JobResponseModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Code = reader.GetString(reader.GetOrdinal("Code")),
                Title = reader.GetString(reader.GetOrdinal("title")),
                Description = reader.GetString(reader.GetOrdinal("description")),
                Location = new LocationModel
                {
                    Id = reader.GetInt32(reader.GetOrdinal("LocationId")),
                    Title = reader.GetString(reader.GetOrdinal("title")),
                    City = reader.GetString(reader.GetOrdinal("city")),
                    State = reader.GetString(reader.GetOrdinal("state")),
                    Country = reader.GetString(reader.GetOrdinal("country")),
                    Zip = reader.GetString(reader.GetOrdinal("zip"))
                },
                Department = new DepartmentModel
                {
                    Id = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                    Title = reader.GetString(reader.GetOrdinal("department_title"))
                },
                PostedDate = reader.GetDateTime(reader.GetOrdinal("postedDate")),
                ClosingDate = reader.GetDateTime(reader.GetOrdinal("ClosingDate"))
            };
        }

    }
}