using BridgeOn_Review.DataBase;
using BridgeOn_Review.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Data;
using System.Data.SqlClient;

namespace BridgeOn_Review.Services.Batch
{
    public class BatchServices : IBatchServices
    {
        private string _connectionSetting;

        public BatchServices(IConfiguration configuration) 
        {
            _connectionSetting = configuration.GetConnectionString("DefaultConnection");
        }

        private BatchModel allData(SqlDataReader reader)
        {
            return new BatchModel()
            {
                Id = (int)reader["ID"],
                Name = reader["Name"].ToString(),
                Batch_Start_Date = (DateTime)reader["Start_Date"]
            };
        }

        public async Task<List<BatchModel>> GetBatchAll()
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
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
                        response.Add(allData(reader));
                    }
                    return response;
                }
            }
        }

        public async Task<BatchModel> GetBatchById(int batchId)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "D";
                SqlCommand cmd = new SqlCommand("SP_BATCH_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", batchId);
                BatchModel response = null;
                await con.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        response = allData(reader);
                    }
                }
                return response;
            }
        }
        public async Task AddBatch(BatchModel batch)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";
                SqlCommand cmd = new SqlCommand("SP_BATCH_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@NAME", batch.Name);
                cmd.Parameters.AddWithValue("@START_DATE", batch.Batch_Start_Date);                
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task<BatchModel> UpdateBatchById(int batchId, BatchModel batch)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "B";
                SqlCommand cmd = new SqlCommand("SP_BATCH_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", batchId);
                cmd.Parameters.AddWithValue("@NAME", batch.Name);
                cmd.Parameters.AddWithValue("@START_DATE", batch.Batch_Start_Date);
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return batch;
            }
        }

        public async Task DeleteBatch(int batchId)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "";
                SqlCommand cmd = new SqlCommand("SP_BATCH_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Id", batchId);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }       
    }
}
