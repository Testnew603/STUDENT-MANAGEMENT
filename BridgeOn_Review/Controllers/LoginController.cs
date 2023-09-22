using Azure;
using BridgeOn_Review.Model;
using BridgeOn_Review.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BridgeOn_Review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAdminServices _adminServices;

        public LoginController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        //private Users AuthenticateUser(Users users)
        //{
        //    Users _user = null;
        //    if(users.Username == "admin" && users.Password == "12345") 
        //    {
        //        _user = new Users
        //        {
        //            Username = "adminController"
        //        };
        //    }
        //        return _user;
        //} 

        //private string GenerateToken(Users users)
        //{
        //    var claims = new[]
        //    {
        //        new Claim (JwtRegisteredClaimNames.Iss, _configuration["Jwt:Key"]),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        //        new Claim("UserName", users.Username.ToString())
        //    };

        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"],claims,
        //        expires:DateTime.Now.AddMinutes(5),
        //        signingCredentials:credentials);
        //    return new JwtSecurityTokenHandler().WriteToken(token); 
        //}

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> AuthenticateUser(Users users)
        {
            var result = _adminServices.AuthenticateUser(users);
            ActionResult response = Unauthorized();
            if (result != null)
            {
                var token = _adminServices.GenerateToken(result);
                response = Ok(new { token = token });
            }
            return response;
        }

        [HttpGet("GetAllBacth")]
        public async Task<List<BatchModel>> GetAllBacth()
        {
            var idClaim = User.FindFirstValue("Username");
            var result = await _adminServices.GetAllBacth(idClaim);
            return result.ToList();
        }

        [Authorize]
        [HttpGet("GetBacthById")]
        public async Task<List<BatchModel>> GetBacthById(int batchId)
        {
            var idClaim = User.FindFirstValue("Username");
            var result = await _adminServices.GetBacthById(batchId, idClaim);
            return result.ToList();
        }

        [Authorize]
        [HttpPost("AddBatch")]
        public async Task<BatchModel> AddBatch(BatchModel batchModel)
        {
            var idClaim = User.FindFirstValue("Username");
            await _adminServices.AddBatch(batchModel, idClaim);
            return batchModel;
        }

        [Authorize]
        [HttpPatch("UpdateBatchById")]
        public async Task<ActionResult<BatchModel>> UpdateBatchById(int batchId, JsonPatchDocument<BatchModel> JSONbatchModel)
        {
            var idClaim = User.FindFirstValue("Username");
            await _adminServices.UpdateBatchById(batchId, JSONbatchModel, idClaim);
            return Ok(JSONbatchModel);
        }

        [Authorize]
        [HttpDelete("DeleteBatch")]
        public async Task<ActionResult<int>> DeleteBatch(int batchId)
        {
            var idClaim = User.FindFirstValue("Username");
            await _adminServices.DeleteBatch(batchId, idClaim);
            return batchId;
        }

        [Authorize]
        [HttpGet("GetAllDomain")]
        public async Task<List<DomainModel>> GetAllDomain()
        {
            var idClaim = User.FindFirstValue("Username");
            var result = await _adminServices.GetAllDomain(idClaim);
            return result.ToList();
        }

        [Authorize]
        [HttpGet("GetDomainById")]
        public async Task<List<DomainModel>> GetDomainById(int domainId)
        {
            var idClaim = User.FindFirstValue("Username");
            var result = await _adminServices.GetDomainById(domainId, idClaim);
            return result.ToList();
        }

        [Authorize]
        [HttpPost("AddDomain")]
        public async Task<DomainModel> AddDomain(DomainModel domainModel)
        {
            var idClaim = User.FindFirstValue("Username");
            await _adminServices.AddDomain(domainModel, idClaim);
            return domainModel;
        }

        [Authorize]
        [HttpPatch("UpdateDomainById")]
        public async Task<ActionResult<BatchModel>> UpdateDomainById(int batchId, JsonPatchDocument<DomainModel> JSONdomainModel)
        {
            var idClaim = User.FindFirstValue("Username");
            await _adminServices.UpdateDomainById(batchId, JSONdomainModel, idClaim);
            return Ok(JSONdomainModel);
        }

        [Authorize]
        [HttpDelete("DeleteDomain")]
        public async Task<ActionResult<int>> DeleteDomain(int domainId)
        {
            var idClaim = User.FindFirstValue("Username");
            await _adminServices.DeleteDomain(domainId, idClaim);
            return domainId;
        }

        [Authorize]
        [HttpGet("GetAdvisorAll")]
        public async Task<List<AdvisorModel>> GetAdvisorAll()
        {
            var idClaim = User.FindFirstValue("Username");
            var result = await _adminServices.GetAdvisorAll(idClaim);
            return result.ToList();
        }

        [Authorize]
        [HttpGet("GetAdvisorById")]
        public async Task<List<AdvisorModel>> GetAdvisorById(int advisorId)
        {
            var idClaim = User.FindFirstValue("Username");
            var result = await _adminServices.GetAdvisorById(advisorId, idClaim);
            return result.ToList();
        }

        [Authorize]
        [HttpPost("AddAdvisor")]
        public async Task<AdvisorModel> AddAdvisor(AdvisorModel advisorModel)
        {
            var idClaim = User.FindFirstValue("Username");
            await _adminServices.AddAdvisor(advisorModel, idClaim);
            return advisorModel;
        }

        [Authorize]
        [HttpPatch("UpdateAdvisorById")]
        public async Task<ActionResult<AdvisorModel>> UpdateAdvisorById(int advisorId, JsonPatchDocument<AdvisorModel> JSONadvisorModel)
        {
            var idClaim = User.FindFirstValue("Username");
            await _adminServices.UpdateAdvisorById(advisorId, JSONadvisorModel, idClaim);
            return Ok(JSONadvisorModel);
        }

        [Authorize]
        [HttpDelete("DeleteAdvisorById")]
        public async Task<ActionResult<int>> DeleteAdvisorById(int advisorId)
        {
            var idClaim = User.FindFirstValue("Username");
            await _adminServices.DeleteAdvisorById(advisorId, idClaim);
            return advisorId;
        }














        //[AllowAnonymous]
        //[HttpPost]
        //public IActionResult Login(Users users)
        //{
        //    IActionResult response = Unauthorized();
        //    var user_ = AuthenticateUser(users);
        //    if(user_ != null) 
        //    { 
        //        var token = GenerateToken(user_);
        //        response = Ok(new {token = token});
        //    }
        //    return response;
        //}



    }
}
