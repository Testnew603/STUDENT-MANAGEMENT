using BridgeOn_Review.Model;
using System.Data;
using System.Data.SqlClient;

namespace BridgeOn_Review.Services.Project
{
    public class ProjectServices : IProjectServices
    {
        private string _connectionSetting;

        public ProjectServices(IConfiguration configuration)
        {
            _connectionSetting = configuration.GetConnectionString("DefaultConnection");
        }

        private ProjectModel allData(SqlDataReader reader)
        {
            return new ProjectModel
            {
                Id = (int)reader["ID"],
                Title = reader["TITLE"].ToString(),
                Modules = reader["MODULES"].ToString(),
                ShortDescription = reader["SHORT_DESCRIPTION"].ToString(),
                ProposedDate = (DateTime)reader["PROPOSED_DATE"],
                Status = reader["STATUS"].ToString(),
                Domain_Id = (int)reader["DOMAIN_ID"],
                Student_Id = (int)reader["STUDENT_ID"],
                Mentor_Id = (int)reader["MENTOR_ID"],                
            };
        }
        public async Task<List<ProjectModel>> GetProjectAll()
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "C";
                SqlCommand cmd = new SqlCommand("SP_PROJECT_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                var response = new List<ProjectModel>();
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

        public async Task<ProjectModel> GetProjectById(int projectId)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "D";
                SqlCommand cmd = new SqlCommand("SP_PROJECT_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", projectId);
                ProjectModel response = null;
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

        public async Task AddProject(ProjectModel projectModel)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";
                SqlCommand cmd = new SqlCommand("SP_PROJECT_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@TITLE", projectModel.Title);
                cmd.Parameters.AddWithValue("@MODULES", projectModel.Modules);
                cmd.Parameters.AddWithValue("@SHORT_DESC", projectModel.ShortDescription);
                cmd.Parameters.AddWithValue("@PROPOSED_DATE", projectModel.ProposedDate);
                cmd.Parameters.AddWithValue("@STATUS", projectModel.Status);
                cmd.Parameters.AddWithValue("@DOMAIN_ID", projectModel.Domain_Id);
                cmd.Parameters.AddWithValue("@STUDENT_ID", projectModel.Student_Id);
                cmd.Parameters.AddWithValue("@MENTOR_ID", projectModel.Mentor_Id);
                
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task<ProjectModel> UpdateProjectById(int projectId, ProjectModel projectModel)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "B";
                SqlCommand cmd = new SqlCommand("SP_PROJECT_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", projectId);
                cmd.Parameters.AddWithValue("@TITLE", projectModel.Title);
                cmd.Parameters.AddWithValue("@MODULES", projectModel.Modules);
                cmd.Parameters.AddWithValue("@SHORT_DESC", projectModel.ShortDescription);
                cmd.Parameters.AddWithValue("@PROPOSED_DATE", projectModel.ProposedDate);
                cmd.Parameters.AddWithValue("@STATUS", projectModel.Status);
                cmd.Parameters.AddWithValue("@DOMAIN_ID", projectModel.Domain_Id);
                cmd.Parameters.AddWithValue("@STUDENT_ID", projectModel.Student_Id);
                cmd.Parameters.AddWithValue("@MENTOR_ID", projectModel.Mentor_Id);
                
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return projectModel;
            }
        }
        public async Task DeleteProject(int projectId)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "";
                SqlCommand cmd = new SqlCommand("SP_PROJECT_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", projectId);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }       

       
    }
}
