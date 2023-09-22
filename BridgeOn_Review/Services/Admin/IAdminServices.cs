using BridgeOn_Review.Model;
using Microsoft.AspNetCore.JsonPatch;

namespace BridgeOn_Review.Services.Admin
{
    public interface IAdminServices
    {
        Users AuthenticateUser(Users users);
        string GenerateToken(Users users);
        Task<List<BatchModel>> GetAllBacth(string TokenResult);
        Task<List<BatchModel>> GetBacthById(int batchId, string TokenResult);
        Task AddBatch(BatchModel batchModel, string TokenResult);
        Task UpdateBatchById(int batchId, JsonPatchDocument<BatchModel> JSONbatchModel, string TokenResult);
        Task<string> DeleteBatch(int batchId, string TokenResult);

        Task<List<DomainModel>> GetAllDomain(string TokenResult);
        Task<List<DomainModel>> GetDomainById(int domainId, string TokenResult);
        Task AddDomain(DomainModel domainModel, string TokenResult);
        Task UpdateDomainById(int domainId, JsonPatchDocument<DomainModel> JSONdomainModel, string TokenResult);
        Task<string> DeleteDomain(int domainId, string TokenResult);


        Task<List<AdvisorModel>> GetAdvisorAll(string TokenResult);
        Task<List<AdvisorModel>> GetAdvisorById(int advisorId, string TokenResult);
        Task AddAdvisor(AdvisorModel advisorModel, string TokenResult);
        Task UpdateAdvisorById(int advisorId, JsonPatchDocument<AdvisorModel>
                                                   JSONadvisorModel, string TokenResult);
        Task DeleteAdvisorById(int advisorId, string TokenResult);
    }
}