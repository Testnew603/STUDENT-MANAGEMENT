using BridgeOn_Review.Model;

namespace BridgeOn_Review.Services.Batch
{
    public interface IBatchServices
    {
        Task<List<BatchModel>> GetBatchAll();

        Task<BatchModel> GetBatchById(int batchId);

        Task AddBatch(BatchModel batch);

        Task<BatchModel> UpdateBatchById(int batchId, BatchModel batch);

        Task DeleteBatch(int batchId);
    }
}