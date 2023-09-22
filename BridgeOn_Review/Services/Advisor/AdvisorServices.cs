using BridgeOn_Review.DataBase;
using BridgeOn_Review.DTOs.MentorDTO;
using BridgeOn_Review.DTOs.ReviewDTO;
using BridgeOn_Review.DTOs.ReviewerDTO;
using BridgeOn_Review.DTOs.StudentDTO;
using BridgeOn_Review.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace BridgeOn_Review.Services.Advisor
{
    public class AdvisorServices : IAdvisorService
    {
        private string _connectionSetting;
        private readonly IConfiguration _configuration1;

        public AdvisorServices(IConfiguration configuration, IConfiguration configuration1)
        {
            _connectionSetting = configuration.GetConnectionString("DefaultConnection");
            _configuration1 = configuration1;
        }

        public async Task<(List<Users>, string Message)> LoginAdvisor(Users users)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";
                string type1 = "ADVISOR";
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

        ////----------------------- START REVIEWER SECTION -----------------------////
        public async Task<List<ReviewerModel>> GetAllReviewer(string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "C";
                SqlCommand cmd = new SqlCommand("SP_REVIEWER_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                var response = new List<ReviewerModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = new ReviewerModel()
                        {
                            Id = (int)reader["ID"],
                            Name = reader["NAME"].ToString(),
                            ContactNo = reader["CONTACT"].ToString(),
                            Email = reader["EMAIL"].ToString(),                            
                            DomainId = (int)reader["DOMAIN_ID"],
                            AdvisorId = (int)reader["ADVISOR_ID"]
                        };
                        response.Add(result);
                    }
                    return response;
                }
            }
        }

        public async Task<List<ReviewerModel>> GetReviewerById(int reviewerId, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "D";
                SqlCommand cmd = new SqlCommand("SP_REVIEWER_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                cmd.Parameters.AddWithValue("@ID", reviewerId);
                var response = new List<ReviewerModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {                    
                    while (await reader.ReadAsync())
                    {
                        
                        var result = new ReviewerModel()
                        {
                            Id = (int)reader["ID"],
                            Name = reader["NAME"].ToString(),
                            ContactNo = reader["CONTACT"].ToString(),
                            Email = reader["EMAIL"].ToString(),                            
                            DomainId = (int)reader["DOMAIN_ID"],
                            AdvisorId = (int)reader["ADVISOR_ID"]
                        };
                        response.Add(result);
                    }
                    return response;
                }
            }     
        }
        
        public async Task AddReviewer(ReviewerModelDTO reviewerModelDTO, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";
                SqlCommand cmd = new SqlCommand("SP_REVIEWER_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@EMAIL_SENDER_ID", TokenResult);
                cmd.Parameters.AddWithValue("@NAME", reviewerModelDTO.Name);
                cmd.Parameters.AddWithValue("@CONTACT", reviewerModelDTO.ContactNo);
                cmd.Parameters.AddWithValue("@EMAIL_ID", reviewerModelDTO.Email);
                cmd.Parameters.AddWithValue("@DOMAIN_ID", reviewerModelDTO.DomainId);                
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task UpdateReviewerById(int reviewerId, JsonPatchDocument<ReviewerModelDTO>
                                                   JSONreviewerModelDTO, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "B";               
                SqlCommand cmd = new SqlCommand("SP_REVIEWER_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);            
                cmd.Parameters.AddWithValue("@EMAIL_SENDER_ID", TokenResult);
                cmd.Parameters.AddWithValue("@ID", reviewerId);
                foreach (var column in JSONreviewerModelDTO.Operations)
                {
                    cmd.Parameters.AddWithValue(column.path, column.value);
                }
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task DeleteReviewer(int reviewerId, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "";
                SqlCommand cmd = new SqlCommand("SP_REVIEWER_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@EMAIL_SENDER_ID", TokenResult);
                cmd.Parameters.AddWithValue("@Id", reviewerId);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        ////----------------------- END REVIEWER SECTION -----------------------////

        ////----------------------- START MENTOR SECTION -----------------------////

        public async Task<List<MentorModel>> GetMentorAll(string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "C";
                SqlCommand cmd = new SqlCommand("SP_MENTOR_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                var response = new List<MentorModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = new MentorModel()
                        {
                            Id = (int)reader["ID"],
                            Name = reader["NAME"].ToString(),
                            DOB = (DateTime)reader["DOB"],
                            Qualification = reader["QUALIFICATION"].ToString(),
                            ContactNo = reader["CONTACT"].ToString(),
                            EmailAddress = reader["EMAIL_ID"].ToString(),
                            JoinDate = (DateTime)reader["JOIN_DATE"],
                            Status = reader["STATUS"].ToString(),
                            DomainId = (int)reader["DOMAIN_ID"],
                            AdvisorId = (int)reader["ADVISOR_ID"]
                        };
                        response.Add(result);
                    }
                    return response;
                }
            }
        }

        public async Task<List<MentorModel>> GetMentorById(int mentorId, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "D";
                SqlCommand cmd = new SqlCommand("SP_MENTOR_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                cmd.Parameters.AddWithValue("@ID", mentorId);
                var response = new List<MentorModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = new MentorModel()
                        {
                            Id = (int)reader["ID"],
                            Name = reader["NAME"].ToString(),
                            DOB = (DateTime)reader["DOB"],
                            Qualification = reader["QUALIFICATION"].ToString(),
                            ContactNo = reader["CONTACT"].ToString(),
                            EmailAddress = reader["EMAIL_ID"].ToString(),
                            JoinDate = (DateTime)reader["JOIN_DATE"],
                            Status = reader["STATUS"].ToString(),
                            DomainId = (int)reader["DOMAIN_ID"],
                            AdvisorId = (int)reader["ADVISOR_ID"]
                        };
                        response.Add(result);
                    }
                    return response;
                }
            }

        }

        public async Task AddMentor(MentorModelDTO mentorModelDTO, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";
                SqlCommand cmd = new SqlCommand("SP_MENTOR_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@EMAIL_SENDER_ID", TokenResult);
                cmd.Parameters.AddWithValue("@NAME", mentorModelDTO.Name);
                cmd.Parameters.AddWithValue("@DOB", mentorModelDTO.DOB);
                cmd.Parameters.AddWithValue("@QUALIFICATION", mentorModelDTO.Qualification);
                cmd.Parameters.AddWithValue("@CONTACT", mentorModelDTO.ContactNo);
                cmd.Parameters.AddWithValue("@EMAIL_ID", mentorModelDTO.EmailAddress);
                cmd.Parameters.AddWithValue("@JOIN_DATE", mentorModelDTO.JoinDate);
                cmd.Parameters.AddWithValue("@STATUS", mentorModelDTO.Status);
                cmd.Parameters.AddWithValue("@DOMAIN_ID", mentorModelDTO.DomainId);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task UpdateMentorById(int mentorId, JsonPatchDocument<MentorModelDTO>
                                                   JSONmentorModelDTO, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "B";
                SqlCommand cmd = new SqlCommand("SP_MENTOR_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@EMAIL_SENDER_ID", TokenResult);
                cmd.Parameters.AddWithValue("@ID", mentorId);
                foreach (var column in JSONmentorModelDTO.Operations)
                {
                    cmd.Parameters.AddWithValue(column.path, column.value);
                }
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task DeleteMentorById(int mentorId, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "";
                SqlCommand cmd = new SqlCommand("SP_MENTOR_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@EMAIL_SENDER_ID", TokenResult);
                cmd.Parameters.AddWithValue("@Id", mentorId);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        ////----------------------- END MENTOR SECTION -----------------------////


        ////----------------------- START STUDENT SECTION -----------------------////

        public async Task<List<StudentModel>> GetStudentAll(string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "C";
                SqlCommand cmd = new SqlCommand("SP_STUDENT_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                var response = new List<StudentModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = new StudentModel()
                        {
                            Id = (int)reader["ID"],
                            Name = reader["Name"].ToString(),
                            DOB = (DateTime)reader["DOB"],
                            Address = reader["ADDRESS"].ToString(),
                            Qualification = reader["QUALIFICATION"].ToString(),
                            ContactNumber = reader["CONTACT_NUM"].ToString(),
                            EmailId = reader["EMAIL_ID"].ToString(),
                            BatchId = (int)reader["BATCH_ID"],
                            MentorId = (int)reader["MENTOR_ID"],
                            AdvisorId = (int)reader["ADVISOR_ID"],
                            DomainId = (int)reader["DOMAIN_ID"],
                            Status = reader["STATUS"].ToString()                                                       
                        };
                        response.Add(result);
                    }
                    return response;
                }
            }
        }

        public async Task<List<StudentModel>> GetStudentById(int studentId, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "D";
                SqlCommand cmd = new SqlCommand("SP_STUDENT_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", studentId);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                var response = new List<StudentModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = new StudentModel()
                        {
                            Id = (int)reader["ID"],
                            Name = reader["Name"].ToString(),
                            DOB = (DateTime)reader["DOB"],
                            Address = reader["ADDRESS"].ToString(),
                            Qualification = reader["QUALIFICATION"].ToString(),
                            ContactNumber = reader["CONTACT_NUM"].ToString(),
                            EmailId = reader["EMAIL_ID"].ToString(),
                            BatchId = (int)reader["BATCH_ID"],
                            MentorId = (int)reader["MENTOR_ID"],
                            AdvisorId = (int)reader["ADVISOR_ID"],
                            DomainId = (int)reader["DOMAIN_ID"],
                            Status = reader["STATUS"].ToString()
                        };
                        response.Add(result);
                    }
                    return response;
                }
            }
        }

        public async Task AddStudent(StudentModelDTO studentModelDTO, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";
                SqlCommand cmd = new SqlCommand("SP_STUDENT_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@EMAIL_SENDER_ID", TokenResult);
                cmd.Parameters.AddWithValue("@NAME", studentModelDTO.Name);
                cmd.Parameters.AddWithValue("@DOB", studentModelDTO.DOB);
                cmd.Parameters.AddWithValue("@ADDRESS", studentModelDTO.Address);
                cmd.Parameters.AddWithValue("@QUALIFICATION", studentModelDTO.Qualification);
                cmd.Parameters.AddWithValue("@CONTACT", studentModelDTO.ContactNumber);
                cmd.Parameters.AddWithValue("@EMAIL_ID", studentModelDTO.EmailId);
                cmd.Parameters.AddWithValue("@BATCH_ID", studentModelDTO.BatchId);
                cmd.Parameters.AddWithValue("@MENTOR_ID", studentModelDTO.MentorId);
                cmd.Parameters.AddWithValue("@DOMAIN_ID", studentModelDTO.DomainId);                

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task UpdateStudentById(int studentId, JsonPatchDocument<StudentModelDTO>
                                                   JSONstudentModelDTO, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "B";
                SqlCommand cmd = new SqlCommand("SP_STUDENT_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@EMAIL_SENDER_ID", TokenResult);                
                cmd.Parameters.AddWithValue("@ID", studentId);
                foreach (var column in JSONstudentModelDTO.Operations)
                {
                    cmd.Parameters.AddWithValue(column.path, column.value);
                }
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task DeleteStudentById(int studentId, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "";
                SqlCommand cmd = new SqlCommand("SP_MENTOR_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@EMAIL_SENDER_ID", TokenResult);
                cmd.Parameters.AddWithValue("@Id", studentId);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        ////----------------------- START STUDENT SECTION -----------------------////
    }
}
