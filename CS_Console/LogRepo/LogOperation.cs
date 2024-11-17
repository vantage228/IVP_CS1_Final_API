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
                                    LogId = Convert.ToInt32(reader["log_id"]),
                                    SecurityId = Convert.ToInt32(reader["security_id"]),
                                    UpdateTime = Convert.ToDateTime(reader["update_time"]),
                                    UpdatedBy = reader["updated_by"].ToString(),
                                    FieldUpdated = reader["field_updated"].ToString(),
                                    OldValue = reader["old_value"].ToString(),
                                    NewValue = reader["new_value"].ToString(),
                                    UpdateStatus = reader["update_status"].ToString(),
                                    ErrorMessage = reader["error_message"] as string, // Handles null
                                    tableType = reader["table_type"].ToString()
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
