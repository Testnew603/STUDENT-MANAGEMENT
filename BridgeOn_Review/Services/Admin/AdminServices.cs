using BridgeOn_Review.DTOs.LeaveDTO;
using BridgeOn_Review.DTOs.StudentDTO;
using BridgeOn_Review.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Text;

namespace BridgeOn_Review.Services.Admin
{
    public class AdminServices : IAdminServices
    {
        private readonly IConfiguration _configuration;
        private string _connectionSetting;

        public AdminServices(IConfiguration configuration)
        {
            _connectionSetting = configuration.GetConnectionString("DefaultConnection");
            _configuration = configuration;
        }

        //----------------------------------------START LOGIN SECTION---------------------------------------------//
        public Users AuthenticateUser(Users users)
        {
            Users _user = null;
            if (users.Username == "admin" && users.Password == "12345")
            {
                _user = new Users
                {
                    Username = "adminController"
                };
            }
            return _user;
        }
        //------------------------------------------END LOGIN SECTION---------------------------------------------//

        //------------------------------------------START TOKEN GENERATION---------------------------------------//
        public string GenerateToken(Users users)
        {
            var claims = new[]
            {
                new Claim (JwtRegisteredClaimNames.Iss, _configuration["Jwt:Key"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserName", users.Username.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //------------------------------------------END TOKEN GENERATION-----------------------------------------//

        //------------------------------------------START BATCH SECTION-----------------------------------------//
        public async Task<List<BatchModel>> GetAllBacth(string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                if(TokenResult != "adminController")
                {
                    return new List<BatchModel>();
                }
                string type = "C";
                SqlCommand cmd = new SqlCommand("SP_BATCH_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                
                var response = new List<BatchModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = new BatchModel()
                        {
                            Id = (int)reader["ID"],
                            Name = reader["NAME"].ToString(),
                            Batch_Start_Date = (DateTime)reader["START_DATE"]
                        };
                        response.Add(result);
                    }
                    return response;
                }
            }
        }

        public async Task<List<BatchModel>> GetBacthById(int batchId, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                if (TokenResult != "adminController")
                {
                    return new List<BatchModel>();
                }
                string type = "D";
                SqlCommand cmd = new SqlCommand("SP_BATCH_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", batchId);

                var response = new List<BatchModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = new BatchModel()
                        {
                            Id = (int)reader["ID"],
                            Name = reader["NAME"].ToString(),
                            Batch_Start_Date = (DateTime)reader["START_DATE"]
                        };
                        response.Add(result);
                    }
                    return response;
                }
            }
        }

        public async Task AddBatch(BatchModel batchModel, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                if (TokenResult != "adminController")
                {
                    return;
                }
                string type = "A";
                SqlCommand cmd = new SqlCommand("SP_BATCH_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@NAME", batchModel.Name);
                cmd.Parameters.AddWithValue("@START_DATE", batchModel.Batch_Start_Date);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task UpdateBatchById(int batchId, JsonPatchDocument<BatchModel> JSONbatchModel, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {

                string type = "B";
                SqlCommand cmd = new SqlCommand("SP_BATCH_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", batchId);
                foreach (var column in JSONbatchModel.Operations)
                {
                    cmd.Parameters.AddWithValue(column.path, column.value);
                }

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

            public async Task<string> DeleteBatch(int batchId, string TokenResult)
            {
                using (SqlConnection con = new SqlConnection(_connectionSetting))
                {
                    if (TokenResult != "adminController")
                    {
                        return "Error Found...!";
                    }
                    string type = "";
                    SqlCommand cmd = new SqlCommand("SP_BATCH_ALL", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@ID", batchId);                    

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return "Batch Id Not Matching...!";
                }
            }
        //------------------------------------------END BATCH SECTION--------------------------------------------//

        //------------------------------------------START DOMAIN SECTION----------------------------------------//
        public async Task<List<DomainModel>> GetAllDomain(string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                if (TokenResult != "adminController")
                {
                    return new List<DomainModel>();
                }
                string type = "C";
                SqlCommand cmd = new SqlCommand("SP_DOMAIN_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);

                var response = new List<DomainModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = new DomainModel()
                        {
                            Id = (int)reader["ID"],
                            Name = reader["NAME"].ToString()                            
                        };
                        response.Add(result);
                    }
                    return response;
                }
            }
        }

        public async Task<List<DomainModel>> GetDomainById(int domainId, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                if (TokenResult != "adminController")
                {
                    return new List<DomainModel>();
                }
                string type = "D";
                SqlCommand cmd = new SqlCommand("SP_DOMAIN_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", domainId);

                var response = new List<DomainModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = new DomainModel()
                        {
                            Id = (int)reader["ID"],
                            Name = reader["NAME"].ToString(),                            
                        };
                        response.Add(result);
                    }
                    return response;
                }
            }
        }

        public async Task AddDomain(DomainModel domainModel, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                if (TokenResult != "adminController")
                {
                    return;
                }
                string type = "A";
                SqlCommand cmd = new SqlCommand("SP_DOMAIN_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@NAME", domainModel.Name);                

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task UpdateDomainById(int domainId, JsonPatchDocument<DomainModel> JSONdomainModel, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {

                string type = "B";
                SqlCommand cmd = new SqlCommand("SP_DOMAIN_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", domainId);
                foreach (var column in JSONdomainModel.Operations)
                {
                    cmd.Parameters.AddWithValue(column.path, column.value);
                }

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task<string> DeleteDomain(int domainId, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                if (TokenResult != "adminController")
                {
                    return "Error Found...!";
                }
                string type = "";
                SqlCommand cmd = new SqlCommand("SP_DOMAIN_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", domainId);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return "Batch Id Not Matching...!";
            }
        }

        //------------------------------------------END DOMAIN SECTION-------------------------------------------//

        //------------------------------------------START ADVISOR SECTION---------------------------------------//

        public async Task<List<AdvisorModel>> GetAdvisorAll(string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                if (TokenResult != "adminController")
                {
                    return new List<AdvisorModel>();
                }
                string type = "C";
                SqlCommand cmd = new SqlCommand("SP_ADVISOR_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                
                var response = new List<AdvisorModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = new AdvisorModel()
                        {
                            Id = (int)reader["ID"],
                            Name = reader["Name"].ToString(),
                            DOB = (DateTime)reader["DOB"],
                            ContactNo = reader["CONTACT_NO"].ToString(),
                            EmailAddress = reader["EMAIL"].ToString(),
                            JoinDate = (DateTime)reader["JOIN_DATE"],
                            Status = reader["STATUS"].ToString()
                        };
                        response.Add(result);
                    }
                    return response;
                }
            }
        }

        public async Task<List<AdvisorModel>> GetAdvisorById(int advisorId, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                if (TokenResult != "adminController")
                {
                    return new List<AdvisorModel>();
                }
                string type = "D";
                SqlCommand cmd = new SqlCommand("SP_ADVISOR_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", advisorId);
                
                var response = new List<AdvisorModel>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = new AdvisorModel()
                        {
                            Id = (int)reader["ID"],
                            Name = reader["Name"].ToString(),
                            DOB = (DateTime)reader["DOB"],
                            ContactNo = reader["CONTACT_NO"].ToString(),
                            EmailAddress = reader["EMAIL"].ToString(),
                            JoinDate = (DateTime)reader["JOIN_DATE"],
                            Status = reader["STATUS"].ToString()
                        };
                        response.Add(result);
                    }
                    return response;
                }
            }
        }


        public async Task AddAdvisor(AdvisorModel advisorModel, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                if (TokenResult != "adminController")
                {
                    return;
                }
                string type = "A";
                SqlCommand cmd = new SqlCommand("SP_STUDENT_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@EMAIL_SENDER_ID", TokenResult);
                cmd.Parameters.AddWithValue("@NAME", advisorModel.Name);
                cmd.Parameters.AddWithValue("@DOB", advisorModel.DOB);                               
                cmd.Parameters.AddWithValue("@CONTACT", advisorModel.ContactNo);
                cmd.Parameters.AddWithValue("@EMAIL_ID", advisorModel.EmailAddress);                                
                cmd.Parameters.AddWithValue("@JOIN_DATE", advisorModel.JoinDate);
                cmd.Parameters.AddWithValue("@STATUS", advisorModel.Status);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task UpdateAdvisorById(int advisorId, JsonPatchDocument<AdvisorModel>
                                                   JSONadvisorModel, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                if (TokenResult != "adminController")
                {
                    return;
                }
                string type = "B";
                SqlCommand cmd = new SqlCommand("SP_ADVISOR_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);                
                cmd.Parameters.AddWithValue("@ID", advisorId);
                foreach (var column in JSONadvisorModel.Operations)
                {
                    cmd.Parameters.AddWithValue(column.path, column.value);
                }
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task DeleteAdvisorById(int advisorId, string TokenResult)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                if (TokenResult != "adminController")
                {
                    return;
                }
                string type = "";
                SqlCommand cmd = new SqlCommand("SP_ADVISOR_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);                
                cmd.Parameters.AddWithValue("@Id", advisorId);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        //------------------------------------------END ADVISOR SECTION------------------------------------------//
    }
}

