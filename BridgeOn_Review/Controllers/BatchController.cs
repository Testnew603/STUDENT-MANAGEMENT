using BridgeOn_Review.DataBase;
using BridgeOn_Review.Model;
using BridgeOn_Review.Services.Batch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Numerics;

namespace BridgeOn_Review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        private readonly IBatchServices _batchServices;
        public BatchController(IBatchServices batchServices)
        {
            _batchServices = batchServices;
        }

        ////GET api/allBatch
        //[HttpGet]
        //public async Task<List<BatchModel>> GetAllBatch()
        //{
        //    return await _batchServices.GetBatchAll();
        //}

        ////GET api/batch/{id}
        //[HttpGet("Id")]
        //public async Task<ActionResult<BatchModel>> GetBatchById(int batchId)
        //{
        //    var result = await _batchServices.GetBatchById(batchId);
        //    if (result == null) { return NotFound(); }
        //    return result;
        //}

        ////POST api/addBatch
        //[HttpPost]
        //public async Task<BatchModel> AddBatch(BatchModel batch)
        //{
        //    await _batchServices.AddBatch(batch);
        //    return batch;
        //}

        ////UPDATE api/updateBatch
        //[HttpPut]
        //public async Task<BatchModel> UpdateBatchById(int batchId, BatchModel batch)
        //{
        //    await _batchServices.UpdateBatchById(batchId, batch);
        //    return batch;
        //}

        ////DELETE batchById
        //[HttpDelete]
        //public async Task<int> DeleteBatchById(int batchId)
        //{
        //    await _batchServices.DeleteBatch(batchId);
        //    return batchId;
        //}
        
    }
       
}
