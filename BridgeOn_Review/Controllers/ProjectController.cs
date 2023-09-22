using BridgeOn_Review.Model;
using BridgeOn_Review.Services.Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BridgeOn_Review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectServices _projectServices;

        public ProjectController(IProjectServices projectServices)
        {
            _projectServices = projectServices;
        }

        ////GET api/getAllProject
        //[HttpGet]
        //public async Task<List<ProjectModel>> GetAllProject()
        //{
        //    return await _projectServices.GetProjectAll();
        //}

        ////GET api/getProject/{Id}
        //[HttpGet("Id")]
        //public async Task<ActionResult<ProjectModel>> GetProjectById(int projectId)
        //{
        //    var result = await _projectServices.GetProjectById(projectId);
        //    if (result == null) { return NotFound(); }
        //    return Ok(result);
        //}

        ////POST api/addProject
        //[HttpPost]
        //public async Task<ProjectModel> AddProject(ProjectModel projectModel)
        //{
        //    await _projectServices.AddProject(projectModel);
        //    return projectModel;
        //}

        ////UPDATE api/updateProduct/{Id}
        //[HttpPut]
        //public async Task<ProjectModel> UpdateProjectById(int projectId, ProjectModel projectModel)
        //{
        //    await _projectServices.UpdateProjectById(projectId, projectModel);  
        //    return projectModel;
        //}

        ////DELETE api/deleteProduct/{Id}
        //[HttpDelete]
        //public async Task<int> DeleteProjectById(int projectId)
        //{
        //    await _projectServices.DeleteProject(projectId);
        //    return projectId;
        //}
    }
}
