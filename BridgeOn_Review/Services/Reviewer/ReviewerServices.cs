using BridgeOn_Review.DTOs.ReviewDTO;
using BridgeOn_Review.Model;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BridgeOn_Review.Services.Reviewer
{
    public class ReviewerServices : IReviewerServices
    {
        private string _connectionSetting;
        private readonly IConfiguration _configuration1;

        public ReviewerServices(IConfiguration configuration, IConfiguration configuration1)
        {
            _connectionSetting = configuration.GetConnectionString("DefaultConnection");
            _configuration1 = configuration1;
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


        private ReviewerModel allData(SqlDataReader reader)
        {
            return new ReviewerModel
            {
                Id = (int)reader["ID"],
                Name = reader["NAME"].ToString(),
                ContactNo = reader["CONTACT"].ToString(),
                Email = reader["EMAIL"].ToString(),
                DomainId = (int)reader["DOMAIN_ID"],
                AdvisorId = (int)reader["PROJECT_ID"]                
            };
        }
        
        public async Task<List<ReviewerModel>> GetAllReviewer()
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "C";
                SqlCommand cmd = new SqlCommand("SP_REVIEWER_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                var response = new List<ReviewerModel>();
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

        public async Task<ReviewerModel> GetReviewerById(int reviewerId)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "D";
                SqlCommand cmd = new SqlCommand("SP_REVIEWER_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", reviewerId);
                ReviewerModel response = null;
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

        public async Task AddReviewer(ReviewerModel reviewerModel)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";
                SqlCommand cmd = new SqlCommand("SP_REVIEWER_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@NAME", reviewerModel.Name);
                cmd.Parameters.AddWithValue("@CONTACT", reviewerModel.ContactNo);
                cmd.Parameters.AddWithValue("@EMAIL", reviewerModel.Email);
                cmd.Parameters.AddWithValue("@DOMAIN_ID", reviewerModel.DomainId);
                cmd.Parameters.AddWithValue("@PROJECT_ID", reviewerModel.AdvisorId);                                            
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task<ReviewerModel> UpdateReviewerById(int reviewerId, ReviewerModel reviewerModel)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "B";
                SqlCommand cmd = new SqlCommand("SP_REVIEWER_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", reviewerId);
                cmd.Parameters.AddWithValue("@NAME", reviewerModel.Name);
                cmd.Parameters.AddWithValue("@CONTACT", reviewerModel.ContactNo);
                cmd.Parameters.AddWithValue("@EMAIL", reviewerModel.Email);
                cmd.Parameters.AddWithValue("@DOMAIN_ID", reviewerModel.DomainId);
                cmd.Parameters.AddWithValue("@PROJECT_ID", reviewerModel.AdvisorId);                
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return reviewerModel;
            }
        }

        public async Task DeleteReviewer(int reviewerId)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "";
                SqlCommand cmd = new SqlCommand("SP_REVIEWER_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Id", reviewerId);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task<(List<Users>, string Message)> LoginReviewer(Users users)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";
                string type1 = "REVIEWER";
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

        public async Task<ReviewModelReviewerDTO> UpdateReviewById(int reviewId, ReviewModelReviewerDTO reviewModelReviewerDTO, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "B";
                string type1 = "REVIEWER";
                SqlCommand cmd = new SqlCommand("SP_REVIEW_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Type1", type1);
                cmd.Parameters.AddWithValue("@EMAIL_ID", TokenResult);
                cmd.Parameters.AddWithValue("@ID", reviewId);                                                
                cmd.Parameters.AddWithValue("@TASK_MARKS", reviewModelReviewerDTO.TaskMarks);

                string status;
                if (Math.Round(reviewModelReviewerDTO.TaskMarks.Value) > 6)
                    status = "PASSED";
                else
                    status = "WEEK BACK";

                cmd.Parameters.AddWithValue("@STATUS", status);
                cmd.Parameters.AddWithValue("@STATUS_DESCRIPTION", reviewModelReviewerDTO.StatusDescription);
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return reviewModelReviewerDTO;
            }
        }




    }
}
