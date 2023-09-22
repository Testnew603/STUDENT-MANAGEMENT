using BridgeOn_Review.Model;

namespace BridgeOn_Review.Services.Project
{
    public interface IProjectServices
    {
        Task<List<ProjectModel>> GetProjectAll();

        Task<ProjectModel> GetProjectById(int projectId);

        Task AddProject(ProjectModel projectModel);

        Task<ProjectModel> UpdateProjectById(int projectId, ProjectModel projectModel);

        Task DeleteProject(int projectId);  
    }
}