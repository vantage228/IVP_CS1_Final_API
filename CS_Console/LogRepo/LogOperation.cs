using CS_Console.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CS_Console.LogRepo
{
    public class LogOperation : ILog
    {
        private string _connectionString;

        public LogOperation(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<SecurityUpdateLog> GetAllSecurityUpdateLogs()
        {
            List<SecurityUpdateLog> logs = new List<SecurityUpdateLog>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("GetAllSecurityLogs", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SecurityUpdateLog log = new SecurityUpdateLog
                                {
                                    LogId = reader["log_id"] != DBNull.Value ? Convert.ToInt32(reader["log_id"]) : 0,
                                    SecurityId = reader["security_id"] != DBNull.Value ? Convert.ToInt32(reader["security_id"]) : 0,
                                    UpdateTime = reader["update_time"] != DBNull.Value ? Convert.ToDateTime(reader["update_time"]) : DateTime.MinValue,
                                    UpdatedBy = reader["updated_by"] != DBNull.Value ? reader["updated_by"].ToString() : string.Empty,
                                    FieldUpdated = reader["field_updated"] != DBNull.Value ? reader["field_updated"].ToString() : string.Empty,
                                    OldValue = reader["old_value"] != DBNull.Value ? reader["old_value"].ToString() : string.Empty,
                                    NewValue = reader["new_value"] != DBNull.Value ? reader["new_value"].ToString() : string.Empty,
                                    UpdateStatus = reader["update_status"] != DBNull.Value ? reader["update_status"].ToString() : string.Empty,
                                    ErrorMessage = reader["error_message"] != DBNull.Value ? reader["error_message"].ToString() : string.Empty,
                                    tableType = reader["table_type"] != DBNull.Value ? reader["table_type"].ToString() : string.Empty
                                };


                                logs.Add(log);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return logs;
        }
    }
}
