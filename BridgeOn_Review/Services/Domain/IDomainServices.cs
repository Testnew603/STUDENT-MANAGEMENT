using BridgeOn_Review.Model;

namespace BridgeOn_Review.Services.Domain
{
    public interface IDomainServices
    {
        Task<List<DomainModel>> GetDomainAll();

        Task<DomainModel> GetDomainById(int domainId);

        Task AddDomain(DomainModel domainModel);

        Task<DomainModel> UpdateDomainById(int domainId, DomainModel domainModel);

        Task DeleteDomain(int domainId);
    }
}