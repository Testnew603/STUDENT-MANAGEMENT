using Azure;
using Azure.Messaging;
using BridgeOn_Review.DTOs.AttendanceDTO;
using BridgeOn_Review.DTOs.LeaveDTO;
using BridgeOn_Review.DTOs.ProjectDTO;
using BridgeOn_Review.DTOs.ReviewDTO;
using BridgeOn_Review.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace BridgeOn_Review.Services.Student
{
    public class StudentServices : IStudentServices
    {
        private string _connectionSetting;
        private readonly IConfiguration _configuration1;

        public StudentServices(IConfiguration configuration, IConfiguration configuration1)
        {
            _connectionSetting = configuration.GetConnectionString("DefaultConnection");
            _configuration1 = configuration1;
        }       

        ////----------------------- START LOGIN SECTION -----------------------////
        ///
        public async Task<(List<Users>, string Message)> LoginStudent(Users users)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";
                string type1 = "STUDENT";
                string message = "";
                SqlCommand cmd = new SqlCommand("SP_UPDATE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Type1", type1);
                cmd.Parameters.AddWithValue("@USERNAME", users.Username);
                cmd.Parameters.AddWithValue("@PASSWORD", users.Password);
                var response = new List<Users>();
                await con.OpenAsync();               
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Users users1 = new Users
                    {
                        Username = reader.GetString(0),
                        Password = reader.GetString(1)
                    };
                    response.Add(users1);
                }
                bool validLogin = false;
                foreach (var data in response)
                {
                    if (data.Username == users.Username && data.Password == users.Password)
                    {
                        validLogin = true;
                        break;
                    }
                }              
                if (validLogin)
                {
                    message = "Login Success...!";
                }
                else
                {
                    message = "Invalid Credentials ...!";
                }
                return (null, message);
            }
        }

        ////----------------------- END LOGIN SECTION -----------------------////

        ////----------------------- START TOKEN GENERATE -----------------------////
        public string GetToken(Users usersData)
        {
            var claims = new[]
            {
                new Claim (JwtRegisteredClaimNames.Iss, _configuration1["Jwt:Key"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserName", usersData.Username.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration1["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration1["Jwt:Issuer"],
                _configuration1["Jwt:Audience"], claims,
               expires: DateTime.Now.AddHours(1),
               signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        ////----------------------- END TOKEN GENERATE -----------------------////

        ////----------------------- START LEAVE -----------------------////
        public async Task<List<LeaveModel>> GetLeaveAll(LeaveModel leaveModel)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "C";
                SqlCommand cmd = new SqlCommand("SP_LEAVE_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@EMAIL_ID", leaveModel.TokenResult);
                var response = new List<LeaveModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = new LeaveModel()
                        {
                            Id = (int)reader["ID"],
                            Date = (DateTime)reader["DATE"],
                            DaysOfLeave = (int)reader["DAYS_OF_LEAVE"],
                            Reason = reader["REASON"].ToString(),
                            Status = reader["STATUS"].ToString(),
                            ApprovedBy = reader["APPROVED_BY"].ToString(),
                            StudentId = (int)reader["STUDENT_ID"],
                            MentorId = (int)reader["MENTOR_ID"],
                            BatchId = (int)reader["BATCH_ID"]
                        };
                        response.Add(result);
                    }
                    return response;
                }
            }
        }

        public async Task AddLeave(LeaveModelDTO leaveModelDTO)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";
                SqlCommand cmd = new SqlCommand("SP_LEAVE_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);               
                cmd.Parameters.AddWithValue("@DATE", leaveModelDTO.Date);
                cmd.Parameters.AddWithValue("@DAYS_OF_LEAVE", leaveModelDTO.DaysOfLeave);
                cmd.Parameters.AddWithValue("@REASON", leaveModelDTO.Reason);
                cmd.Parameters.AddWithValue("@EMAIL_ID", leaveModelDTO.TokenResult); 
                
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task UpdateLeave(int leaveId, JsonPatchDocument<LeaveModelDTO> JSONleaveModelDTO, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                
                string type = "B";
                SqlCommand cmd = new SqlCommand("SP_LEAVE_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", leaveId);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                foreach (var column in JSONleaveModelDTO.Operations)
                {
                    cmd.Parameters.AddWithValue(column.path, column.value);
                }

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }


        ////----------------------- END LEAVE -----------------------////

        ////----------------------- START ATTENDANCE -----------------------////

        public async Task<List<AttendanceModel>> GetAllAttendance(string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {                
                string type = "C";
                SqlCommand cmd = new SqlCommand("SP_ATTENDANCE_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                var response = new List<AttendanceModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = new AttendanceModel()
                        {
                            Id = (int)reader["ID"],
                            StudentName = reader["STUD_NAME"].ToString(),
                            EntryTime = (DateTime)reader["ENTRY_TIME"],
                            LateReason = reader["LATE_REASON"].ToString(),
                            ExitTime = reader["EXIT_TIME"] != DBNull.Value ? (DateTime)reader["EXIT_TIME"] : null,                       
                            LeavingReason = reader["LEAVING_REASON"].ToString(),
                            Status = reader["STATUS"].ToString(),
                            StudentId = (int)reader["STUDENT_ID"],
                            MentorId = (int)reader["MENTOR_ID"]
                        };
                        response.Add(result);
                    }
                    return response;
                }
            }
        }

        public async Task AddAttendance(AttendanceModelDTO attendanceModelDTO, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";                
                SqlCommand cmd = new SqlCommand("SP_ATTENDANCE_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);                
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);

                var officeStartTime = new TimeSpan(9, 30, 0); // 9:30 AM
                var currentTime = DateTime.Now.TimeOfDay;
                var entryTime = attendanceModelDTO.EntryTime.Value.TimeOfDay;
                if(entryTime < currentTime)
                {
                    entryTime = currentTime;
                }
                if(entryTime <= officeStartTime)
                {                    
                    cmd.Parameters.AddWithValue("@ENTRY_TIME", attendanceModelDTO.EntryTime);                    
                    cmd.Parameters.AddWithValue("@LATE_REASON", attendanceModelDTO.LateReason);
                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;                    
                }
                else
                {
                    Console.WriteLine("Please provide a late reason:");
                    var lateReason = attendanceModelDTO.LateReason = Console.ReadLine();
                   
                    cmd.Parameters.AddWithValue("@ENTRY_TIME", attendanceModelDTO.EntryTime);                    
                    cmd.Parameters.AddWithValue("@LATE_REASON", lateReason);
                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }                
            }
        }

        public async Task AddAttendanceExitTime(int attendanceId, AttendanceExitModelDTO attendanceExitModelDTO, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
               await con.OpenAsync();
                using (SqlCommand cmd1 = new SqlCommand("Select entry_time from ATTENDANCE where STATUS = 'PENDING' and ID = '"+attendanceId+"'",con))
                using (SqlDataReader reader = await cmd1.ExecuteReaderAsync())
                {
                    if(await reader.ReadAsync())
                    {
                        var entryTime = reader.GetDateTime(0).TimeOfDay;
                        entryTime = entryTime.Add(TimeSpan.FromHours(3));
                        var officeEndTime = new TimeSpan(17, 30, 0);
                        var currentTime = DateTime.Now.TimeOfDay;
                        var exitTime = attendanceExitModelDTO.ExitTime.Value.TimeOfDay;
                        if (exitTime >= currentTime)
                        {
                            exitTime = currentTime;
                        }

                        if (exitTime >= entryTime && exitTime >= officeEndTime)
                        {
                            string type = "B";
                            string type1 = "EXIT_TIME";
                            reader.Close();
                            SqlCommand cmd = new SqlCommand("SP_ATTENDANCE_ALL", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Type", type);
                            cmd.Parameters.AddWithValue("@Type1", type1);
                            cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                            cmd.Parameters.AddWithValue("@ID", attendanceId);
                            cmd.Parameters.AddWithValue("@EXIT_TIME", exitTime);
                            cmd.Parameters.AddWithValue("@LATE_REASON", attendanceExitModelDTO.LeavingReason);
                      
                            await cmd.ExecuteNonQueryAsync();
                        }
                        else
                        {
                            
                            Console.WriteLine("Please provide a leaving reason:");
                            var leavingReason = attendanceExitModelDTO.LeavingReason = Console.ReadLine();

                            string type = "B";
                            string type1 = "EXIT_TIME";
                            reader.Close();
                            SqlCommand cmd = new SqlCommand("SP_ATTENDANCE_ALL", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Type", type);
                            cmd.Parameters.AddWithValue("@Type1", type1);
                            cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                            cmd.Parameters.AddWithValue("@ID", attendanceId);
                            cmd.Parameters.AddWithValue("@EXIT_TIME", exitTime);
                            cmd.Parameters.AddWithValue("@LEAVING_REASON", leavingReason);
                            
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }                
                return;
            }
        }

        public async Task UpdateAttendanceById(int attendanceId, JsonPatchDocument<AttendanceModelDTO>
                                               JSONattendanceModelDTO, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "B";
                string type1 = "ATTENDANCE_UPDATE";
                SqlCommand cmd = new SqlCommand("SP_ATTENDANCE_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Type", type1);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                cmd.Parameters.AddWithValue("@ID", attendanceId);             
                foreach(var column in  JSONattendanceModelDTO.Operations) 
                {
                    cmd.Parameters.AddWithValue(column.path, column.value);
                }                           
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        ////----------------------- END ATTENDANCE -----------------------////

        public async Task<List<ReviewModel>> GetReviewAll(string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "C";
                string type1 = "STUDENT";
                SqlCommand cmd = new SqlCommand("SP_REVIEW_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Type1", type1);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                var response = new List<ReviewModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = new ReviewModel()
                        {
                            Id = (int)reader["ID"],
                            WeekNo = (int)reader["WEEK_NO"],
                            ScheduledDate = (DateTime)reader["SCHEDULED_DATE"],
                            PostponedDate = reader["POSTPONED_DATE"] != DBNull.Value ? (DateTime)reader["POSTPONED_DATE"] : null,
                            Decisions = reader["DECISIONS"].ToString(),
                            TaskMarks = reader["TASK_MARKS"] != DBNull.Value ? (double)reader["TASK_MARKS"] : 0,
                            ReviewType = (int)reader["REVIEW_TYPE"],
                            ReviewMode = reader["REVIEW_MODE"].ToString(),
                            Status = reader["STATUS"].ToString(),
                            StatusDescription = reader["STATUS_DESCRIPTION"].ToString(),
                            StudentId = (int)reader["STUDENT_ID"],
                            MentorId = (int)reader["MENTOR_ID"],
                            ReviewerId = (int)reader["REVIEWER_ID"],
                            DomainId = (int)reader["DOMAIN_ID"]
                        };
                        response.Add(result);
                    }
                    return response;
                }
            }
        }

        public async Task<List<ReviewModel>> GetReviewById(int reviewId, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "D";
                string type1 = "STUDENT";
                SqlCommand cmd = new SqlCommand("SP_REVIEW_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Type1", type1);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                cmd.Parameters.AddWithValue("@ID", reviewId);
                var response = new List<ReviewModel>();
                await con.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = new ReviewModel()
                        {
                            Id = (int)reader["ID"],
                            WeekNo = (int)reader["WEEK_NO"],
                            ScheduledDate = (DateTime)reader["SCHEDULED_DATE"],
                            PostponedDate = reader["POSTPONED_DATE"] != DBNull.Value ? (DateTime)reader["POSTPONED_DATE"] : null,
                            Decisions = reader["DECISIONS"].ToString(),
                            TaskMarks = reader["TASK_MARKS"] != DBNull.Value ? (double)reader["TASK_MARKS"] : 0,
                            ReviewType = (int)reader["REVIEW_TYPE"],
                            ReviewMode = reader["REVIEW_MODE"].ToString(),
                            Status = reader["STATUS"].ToString(),
                            StatusDescription = reader["STATUS_DESCRIPTION"].ToString(),
                            StudentId = (int)reader["STUDENT_ID"],
                            MentorId = (int)reader["MENTOR_ID"],
                            ReviewerId = (int)reader["REVIEWER_ID"],
                            DomainId = (int)reader["DOMAIN_ID"]
                        };
                        response.Add(result);
                    }
                }
                return response;
            }
        }


        public async  Task SendReviewPostponeDate(int reviewId, JsonPatchDocument<ReviewModelDTO>
                                               JSONreviewModelDTO, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "B";
                string type1 = "STUDENT";
                SqlCommand cmd = new SqlCommand("SP_REVIEW_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Type1", type1);                
                cmd.Parameters.AddWithValue("@ID", reviewId);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                foreach (var column in JSONreviewModelDTO.Operations)
                {
                    cmd.Parameters.AddWithValue(column.path, column.value);
                }                                                                                                            
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }


        //------------------------ START PROJECT ------------------------//

        public async Task<List<ProjectModel>> GetAllProject(string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "C";
                string type1 = "STUDENT";
                SqlCommand cmd = new SqlCommand("SP_PROJECT_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Type1", type1);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                var response = new List<ProjectModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {                    
                    while (await reader.ReadAsync())
                    {                        
                        var result = new ProjectModel()
                        {
                            Id = (int)reader["ID"],
                            Title = reader["TITLE"].ToString(),
                            Modules = reader["MODULES"].ToString(),
                            ShortDescription = reader["SHORT_DESCRIPTION"].ToString(),
                            ProposedDate = (DateTime)reader["PROPOSED_DATE"],
                            Status = reader["STATUS"].ToString(),
                            Remarks = reader["REMARKS"] != DBNull.Value ? reader["REMARKS"].ToString() : "pending",                            
                            Review_Attended = reader["REVIEW_ATTENDED"] != DBNull.Value ? (int)reader["REVIEW_ATTENDED"] : 0,
                            Domain_Id = (int)reader["DOMAIN_ID"],
                            Student_Id = (int)reader["STUDENT_ID"],
                            Mentor_Id = (int)reader["MENTOR_ID"]
                        };
                                                
                        response.Add(result);
                    }
                    return response;
                }
            }
        }

        public async Task<List<ProjectModel>> GetProjectById(int projectId, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "C";
                string type1 = "STUDENT";
                SqlCommand cmd = new SqlCommand("SP_PROJECT_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Type1", type1);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                cmd.Parameters.AddWithValue("@ID", projectId);
                var response = new List<ProjectModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = new ProjectModel()
                        {
                            Id = (int)reader["ID"],
                            Title = reader["TITLE"].ToString(),
                            Modules = reader["MODULES"].ToString(),
                            ShortDescription = reader["SHORT_DESCRIPTION"].ToString(),
                            ProposedDate = (DateTime)reader["PROPOSED_DATE"],
                            Status = reader["STATUS"].ToString(),
                            Remarks = reader["REMARKS"] != DBNull.Value ? reader["REMARKS"].ToString() : "pending",
                            Review_Attended = reader["REVIEW_ATTENDED"] != DBNull.Value ? (int)reader["REVIEW_ATTENDED"] : 0,
                            Domain_Id = (int)reader["DOMAIN_ID"],
                            Student_Id = (int)reader["STUDENT_ID"],
                            Mentor_Id = (int)reader["MENTOR_ID"]
                        };
                        response.Add(result);
                    }
                    return response;
                }
            }
        }

        public async Task AddProject(ProjectModelDTO projectModelDTO, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";                
                SqlCommand cmd = new SqlCommand("SP_PROJECT_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);                
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                cmd.Parameters.AddWithValue("@TITLE", projectModelDTO.Title);
                cmd.Parameters.AddWithValue("@MODULES", projectModelDTO.Modules);
                cmd.Parameters.AddWithValue("@SHORT_DESC", projectModelDTO.ShortDescription);
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task UpdateProjectById(int projectId, JsonPatchDocument<ProjectModelDTO>
                                               JSONprojectModelDTO, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "B";
                string type1 = "STUDENT";
                SqlCommand cmd = new SqlCommand("SP_PROJECT_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Type1", type1);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                cmd.Parameters.AddWithValue("@ID", projectId);
                foreach (var column in JSONprojectModelDTO.Operations)
                {
                    cmd.Parameters.AddWithValue(column.path, column.value);
                }
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }      

        //------------------------ END PROJECT ------------------------//

    }
}
