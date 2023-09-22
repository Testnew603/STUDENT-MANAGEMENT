using BridgeOn_Review.DataBase;
using BridgeOn_Review.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace BridgeOn_Review.Services.Attendance
{
    public class AttendanceServices : IAttendanceService
    {
        private readonly string _connectionSetting;

        public AttendanceServices(IConfiguration configuration)
        {
            _connectionSetting = configuration.GetConnectionString("DefaultConnection");
        }

        private AttendanceModel allData(SqlDataReader reader)
        {
            return new AttendanceModel()
            {
                Id = (int)reader["ID"],
                StudentName = reader["STUD_NAME"].ToString(),
                EntryTime = (DateTime)reader["ENTRY_TIME"],
                ExitTime = (DateTime)reader["EXIT_TIME"],
                Status = reader["STATUS"].ToString(),
                LateReason = reader["LATE_REASON"].ToString(),
                StudentId = (int)reader["STUDENT_ID"],
                MentorId = (int)reader["MENTOR_ID"]
            };
        }
        public async Task<List<AttendanceModel>> GetAllAttendance()
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "C";             
                SqlCommand cmd = new SqlCommand("SP_ATTENDANCE_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                var response = new List<AttendanceModel>();
                await con.OpenAsync();
                using (var  reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response.Add(allData(reader));
                    }
                        return response;
                }               
            }
        }
        public async Task<AttendanceModel> GetAttendanceByID(int attendanceId)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "D";
                SqlCommand cmd = new SqlCommand("SP_ATTENDANCE_ALL", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@ID", attendanceId);          
                    AttendanceModel response = null;
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
        public async Task AddAttendance(AttendanceModel attendance)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";
                SqlCommand cmd = new SqlCommand("SP_ATTENDANCE_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@STUD_NAME", attendance.StudentName);
                cmd.Parameters.AddWithValue("@ENTRY_TIME", attendance.EntryTime);
                cmd.Parameters.AddWithValue("@EXIT_TIME", attendance.ExitTime);
                cmd.Parameters.AddWithValue("@STATUS", attendance.Status);
                cmd.Parameters.AddWithValue("@LATE_REASON", attendance.LateReason);
                cmd.Parameters.AddWithValue("@STUDENT_ID", attendance.StudentId);
                cmd.Parameters.AddWithValue("@MENTOR_ID", attendance.MentorId);
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }            
        public async Task DeleteAttendance(int attendanceId)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "";
                SqlCommand cmd = new SqlCommand("SP_ATTENDANCE_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Id", attendanceId);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task<AttendanceModel> UpdateAttendanceById(int attendanceId, AttendanceModel attendance)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "B";
                SqlCommand cmd = new SqlCommand("SP_ATTENDANCE_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", attendanceId);
                cmd.Parameters.AddWithValue("@STUD_NAME", attendance.StudentName);
                cmd.Parameters.AddWithValue("@ENTRY_TIME", attendance.EntryTime);
                cmd.Parameters.AddWithValue("@EXIT_TIME", attendance.ExitTime);
                cmd.Parameters.AddWithValue("@STATUS", attendance.Status);
                cmd.Parameters.AddWithValue("@LATE_REASON", attendance.LateReason);
                cmd.Parameters.AddWithValue("@STUDENT_ID", attendance.StudentId);
                cmd.Parameters.AddWithValue("@MENTOR_ID", attendance.MentorId);
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return attendance;
            }
        }
    }
}
