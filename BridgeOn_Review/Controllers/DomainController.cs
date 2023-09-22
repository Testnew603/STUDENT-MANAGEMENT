using BridgeOn_Review.Model;
using BridgeOn_Review.Services.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BridgeOn_Review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainController : ControllerBase
    {
        private readonly IDomainServices _domainServices;

        public DomainController(IDomainServices domainServices)
        {
            _domainServices = domainServices;
        }

        ////GET api/domainAll
        //[HttpGet]
        //public async Task<List<DomainModel>> GetAllDomain()
        //{
        //    return await _domainServices.GetDomainAll();
        //}

        ////GET api/domainAll/{id}
        //[HttpGet("Id")]
        //public async Task<ActionResult<DomainModel>> GetAllDomain(int domainId)
        //{
        //    var result = await _domainServices.GetDomainById(domainId);
        //    if(result == null) { return NotFound(); }
        //    return Ok(result);
        //}

        ////POST api/addDomain
        //[HttpPost]
        //public async Task<DomainModel> AddDomain(DomainModel domainModel)
        //{
        //    await _domainServices.AddDomain(domainModel);
        //    return domainModel;
        //}

        ////UPDATE api/updateDomain/{id}
        //[HttpPut]
        //public async Task<DomainModel> UpdateDomainById(int domainId, DomainModel domainModel)
        //{
        //    await _domainServices.UpdateDomainById(domainId, domainModel);  
        //    return domainModel;
        //}

        ////DELETE api/deleteDomain/{id}
        //[HttpDelete]
        //public async Task<int> DeleteDomainById(int domainId)
        //{
        //    await _domainServices.DeleteDomain(domainId);
        //    return domainId;
        //}


    }
}
