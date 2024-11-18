using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Bcpg;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace CS_Console.UserRepo
{
    public class UserOperation : IUser
    {
        private string _connectionString;
        public UserOperation(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<bool> Authenticate(string userID, string password)
        {
            bool isAuthenticated = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Authentication", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.Parameters.AddWithValue("@Password", password);

                        SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(resultParam);

                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        isAuthenticated = Convert.ToBoolean(resultParam.Value);
                    }
                }
                return isAuthenticated;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<string> SignIn(string userID, string password)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_signin", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.Parameters.AddWithValue("@Password", password);

                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        return "Successfully Registered User";
                    }
                }
            }
            catch(DbException ex)
            {
                return "Error Registering the user: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Error Registering the user: " + ex.Message;
            }
        }
    }
}
