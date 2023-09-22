using BridgeOn_Review.Controllers;
using BridgeOn_Review.Model;
using System.Data;
using System.Data.SqlClient;

namespace BridgeOn_Review.Services.Leave
{
    public class LeaveServices : ILeaveServices
    {
        private string _connectionSetting;

        public LeaveServices(IConfiguration configuration)
        {
            _connectionSetting = configuration.GetConnectionString("DefaultConnection");
        }

        private LeaveModel allData(SqlDataReader reader)
        {
            return new LeaveModel
            {
                Id = (int)reader["ID"],
                Date = (DateTime)reader["DATE"],
                DaysOfLeave = (int)reader["DAYS_OF_LEAVE"],
                Reason = reader["REASON"].ToString(),                                
            };
        }

        public async Task<List<LeaveModel>> GetLeaveAll()
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "C";
                SqlCommand cmd = new SqlCommand("SP_LEAVE_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                var response = new List<LeaveModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response.Add(allData(reader));                        
                    }
                    return response;
                }
            }
        }

        public async Task<LeaveModel> GetLeaveById(int leaveId)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "D";
                SqlCommand cmd = new SqlCommand("SP_LEAVE_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", leaveId);
                LeaveModel response = null;
                await con.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response = allData(reader);
                    }
                }
                return response;
            }
        }
        public async Task AddLeave(LeaveModel leaveModel)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";                
                SqlCommand cmd = new SqlCommand("SP_LEAVE_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@DATE", leaveModel.Date);
                cmd.Parameters.AddWithValue("@DAYS_OF_LEAVE", leaveModel.DaysOfLeave);
                cmd.Parameters.AddWithValue("@REASON", leaveModel.Reason);
                cmd.Parameters.AddWithValue("@EMAIL_ID", leaveModel.TokenResult);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task<LeaveModel> UpdateLeaveById(int leaveId, LeaveModel leaveModel)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "B";
                SqlCommand cmd = new SqlCommand("SP_LEAVE_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", leaveId);
                cmd.Parameters.AddWithValue("@DATE", leaveModel.Date);
                cmd.Parameters.AddWithValue("@DAYS_OF_LEAVE", leaveModel.DaysOfLeave);
                cmd.Parameters.AddWithValue("@REASON", leaveModel.Reason);                           

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return leaveModel;
            }
        }

        public async Task DeleteLeave(int leaveId)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "";
                SqlCommand cmd = new SqlCommand("SP_LEAVE_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Id", leaveId);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        

        
    }
}
