using Azure;
using BridgeOn_Review.Model;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BridgeOn_Review.Services.Login
{
    public class LoginServices : ILoginServices
    {
        private string _connectionSetting;
        private readonly IConfiguration _configuration1;

        public LoginServices(IConfiguration configuration, IConfiguration configuration1)
        {
            _connectionSetting = configuration.GetConnectionString("DefaultConnection");
            _configuration1 = configuration1;
        }

        private Users allData(SqlDataReader reader)
        {
            return new Users
            {
                Username = reader["EMAIL_ID"].ToString(),
                Password = reader["PASSWORD"].ToString()
            };
        }

        public async Task<(List<Users>, string)> LoginStudent(Users users)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";
                string message = "";
                SqlCommand cmd = new SqlCommand("SP_UPDATE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@USERNAME", users.Username);
                cmd.Parameters.AddWithValue("@PASSWORD", users.Password);
                var response = new List<Users>();
                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response.Add(allData(reader));
                    }
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

        public string GetToken(Users usersData)
        {
            var claims = new[]
            {
                new Claim (JwtRegisteredClaimNames.Iss, _configuration1["Jwt:Key"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),            
                new Claim("Username",usersData.Username.ToString())
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

    public Task LoginMentor(MentorModel mentorModel)
        {
            throw new NotImplementedException();
        }

        public Task LoginAdvisor(AdvisorModel advisorModel)
        {
            throw new NotImplementedException();
        }
    }
}
