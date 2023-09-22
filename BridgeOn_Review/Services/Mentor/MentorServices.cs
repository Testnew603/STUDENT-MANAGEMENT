using BridgeOn_Review.DTOs.LeaveDTO;
using BridgeOn_Review.DTOs.ProjectDTO;
using BridgeOn_Review.DTOs.ReviewDTO;
using BridgeOn_Review.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace BridgeOn_Review.Services.Mentor
{
    public class MentorServices : IMentorServices
    {

        private string _connectionSetting;
        
        private readonly IConfiguration _configuration1;

        public MentorServices(IConfiguration configuration, IConfiguration configuration1)
        {
            _connectionSetting = configuration.GetConnectionString("DefaultConnection");            
            _configuration1 = configuration1;
        }      

        ////----------------------- START LOGIN SECTION -----------------------////
        ///
        public async Task<(List<Users>, string Message)> LoginMentor(Users users)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";
                string type1 = "MENTOR";
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

        public async Task<List<ReviewModel>> GetReviewAll(string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "C";
                string type1 = "MENTOR";
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
                string type1 = "MENTOR";
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
                return response.ToList();
            }
        }

        public async Task<string> AddReview(ReviewModelMentorDTO reviewModelMentorDTO, string TokenResult)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionSetting))
                {
                    string type = "A";
                    SqlCommand cmd = new SqlCommand("SP_REVIEW_ALL", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                    cmd.Parameters.AddWithValue("@WEEK_NO", reviewModelMentorDTO.WeekNo);
                    cmd.Parameters.AddWithValue("@SHEDULE_DATE", reviewModelMentorDTO.ScheduledDate);                    
                    cmd.Parameters.AddWithValue("@REVIEW_MODE", reviewModelMentorDTO.ReviewMode);
                    cmd.Parameters.AddWithValue("@REVIEW_TYPE", reviewModelMentorDTO.ReviewType);
                    cmd.Parameters.AddWithValue("@STUDENT_ID", reviewModelMentorDTO.StudentId);
                    cmd.Parameters.AddWithValue("@REVIEWER_ID ", reviewModelMentorDTO.ReviewerId);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return "Review added successfully.";
                }
            }
            catch (Exception ex) 
            {
                return ex.Message;
            }
                return "";
            }
        

        public async Task UpdateReviewById(int reviewId, JsonPatchDocument<ReviewModelMentorDTO>
                                                    JSONreviewModelMentorDTO, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "B";
                string type1 = "MENTOR";
                SqlCommand cmd = new SqlCommand("SP_REVIEW_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Type1", type1);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                cmd.Parameters.AddWithValue("@ID", reviewId);
                foreach(var column in JSONreviewModelMentorDTO.Operations)
                {
                    cmd.Parameters.AddWithValue(column.path, column.value);
                }
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task<ReviewModelDTO> PostponedReviewDateById(int reviewId, ReviewModelDTO reviewModelDTO, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
            await con.OpenAsync();
            using (SqlCommand cmd = new SqlCommand("Select SCHEDULED_DATE, POSTPONED_DATE, DECISIONS from REVIEW where ID = '" + reviewId + "'", con))
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                    if (await reader.ReadAsync())
                    {
                        DateTime scheduled_date = reader.GetDateTime(0);
                        DateTime? postponed_date = !reader.IsDBNull(1) ? reader.GetDateTime(1) : (DateTime?)scheduled_date.AddDays(1);
                        string? decisions = !reader.IsDBNull(2) ? reader.GetString(2) : (string?)null;

                        if (postponed_date > scheduled_date && decisions == null || decisions == "POSTPONED" || decisions == "REQUESTED")
                        {
                            string decisionUpdate;
                            if (reviewModelDTO.PostponedDate == null)
                            {
                                decisionUpdate = "REJECTED";
                            }
                            else
                            {
                                decisionUpdate = "POSTPONED";
                            }
                            string type = "B";
                            string type1 = "MENTOR_POSTPONED_REVIEW_DATE";
                            
                            reader.Close();

                            using (SqlCommand cmd1 = new SqlCommand("SP_REVIEW_ALL", con))
                            {
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@Type", type);
                                cmd1.Parameters.AddWithValue("@Type1", type1);
                                cmd1.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                                cmd1.Parameters.AddWithValue("@ID", reviewId);
                                cmd1.Parameters.AddWithValue("@DECISIONS", decisionUpdate);
                                cmd1.Parameters.AddWithValue("@POSTPONED_DATE", reviewModelDTO.PostponedDate);

                                await cmd1.ExecuteNonQueryAsync();
                            }
                        }                      
                    }
            }
            }
            return reviewModelDTO;
        }



        public async Task<List<ProjectModel>> GetProjectAll(string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "C";
                string type1 = "MENTOR";
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
                            Remarks = reader["REMARKS"].ToString(),
                            Review_Attended = (int)reader["REVIEW_ATTENDED"],
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
                string type1 = "MENTOR";
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

        public async Task<ProjectModelMentorDTO> ProjectStatusUpdateById(int projectId, ProjectModelMentorDTO projectModelMentorDTO, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "B";
                string type1 = "MENTOR";
                SqlCommand cmd = new SqlCommand("SP_PROJECT_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Type1", type1);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                cmd.Parameters.AddWithValue("@ID", projectId);
                cmd.Parameters.AddWithValue("@STATUS", projectModelMentorDTO.Status);
                cmd.Parameters.AddWithValue("@REMARKS", projectModelMentorDTO.Remarks);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return projectModelMentorDTO;
            }
        }       

    }
}
