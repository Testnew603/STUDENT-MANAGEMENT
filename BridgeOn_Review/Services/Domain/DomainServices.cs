using BridgeOn_Review.Model;
using System.Data;
using System.Data.SqlClient;

namespace BridgeOn_Review.Services.Domain
{
    public class DomainServices : IDomainServices
    {
        private string _connectionSetting;

        public DomainServices(IConfiguration configuration)
        {
            _connectionSetting = configuration.GetConnectionString("DefaultConnection");
        }

        private DomainModel allData (SqlDataReader reader)
        {
            return new DomainModel
            {
                Id = (int)reader["ID"],
                Name = reader["Name"].ToString()
            };
        }

        public async Task<List<DomainModel>> GetDomainAll()
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "C";
                SqlCommand cmd = new SqlCommand("SP_DOMAIN_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                var response = new List<DomainModel>();
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

        public async Task<DomainModel> GetDomainById(int domainId)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "D";
                SqlCommand cmd = new SqlCommand("SP_DOMAIN_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", domainId);
                DomainModel response = null;
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
        public async Task AddDomain(DomainModel domainModel)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";
                SqlCommand cmd = new SqlCommand("SP_DOMAIN_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@NAME", domainModel.Name);                
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task<DomainModel> UpdateDomainById(int domainId, DomainModel domainModel)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "B";
                SqlCommand cmd = new SqlCommand("SP_DOMAIN_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", domainId);
                cmd.Parameters.AddWithValue("@NAME", domainModel.Name);
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return domainModel;
            }
        }

        public async Task DeleteDomain(int domainId)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "";
                SqlCommand cmd = new SqlCommand("SP_BATCH_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Id", domainId);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }       

       
    }
}
