using BridgeOn_Review.DTOs.ReviewDTO;
using BridgeOn_Review.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BridgeOn_Review.Services.Review
{
    public class ReviewServices : IReviewServices
    {
        private string _connectionSetting;

        public ReviewServices(IConfiguration configuration)
        {
            _connectionSetting = configuration.GetConnectionString("DefaultConnection");
        }

        private ReviewModel allData(SqlDataReader reader)
        {
            return new ReviewModel
            {
                Id = (int)reader["ID"],
                WeekNo = (int)reader["WEEK_NO"],
                ScheduledDate = (DateTime)reader["SCHEDULED_DATE"],
                PostponedDate = (DateTime)reader["POSTPONED_DATE"],
                TaskMarks = (int)reader["TASK_MARKS"],
                ReviewMode = reader["REVIEW_MODE"].ToString(),
                Status = reader["STATUS"].ToString(),
                StatusDescription = reader["STATUS_DESCRIPTION"].ToString(),
                StudentId = (int)reader["STUDENT_ID"],
                MentorId = (int)reader["MENTOR_ID"],
                ReviewerId = (int)reader["REVIEWER_ID"]
            };
        }

        public async Task<List<ReviewModel>> GetReviewAll()
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "C";
                SqlCommand cmd = new SqlCommand("SP_REVIEW_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                var response = new List<ReviewModel>();
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

        public async Task<ReviewModel> GetReviewById(int reviewId)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "D";
                SqlCommand cmd = new SqlCommand("SP_REVIEW_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", reviewId);
                ReviewModel response = null;
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
        public async Task AddReview(ReviewModel reviewModel)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "A";
                SqlCommand cmd = new SqlCommand("SP_REVIEW_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@WEEK_NO", reviewModel.WeekNo);
                cmd.Parameters.AddWithValue("@SHEDULE_DATE", reviewModel.ScheduledDate);
                cmd.Parameters.AddWithValue("@POSTPONED_DATE", reviewModel.PostponedDate);
                cmd.Parameters.AddWithValue("@TASK_MARKS", reviewModel.TaskMarks);
                cmd.Parameters.AddWithValue("@REVIEW_MODE", reviewModel.ReviewMode);
                cmd.Parameters.AddWithValue("@REVIEW_ID", reviewModel.Status);
                cmd.Parameters.AddWithValue("@STATUS_DESCRIPTION", reviewModel.StatusDescription);
                cmd.Parameters.AddWithValue("@STUDENT_ID", reviewModel.StudentId);
                cmd.Parameters.AddWithValue("@MENTOR_ID", reviewModel.MentorId);
                cmd.Parameters.AddWithValue("@REVIEWER_ID", reviewModel.ReviewerId);
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }

        public async Task<ReviewModel> UpdateReviewById(int reviewId, ReviewModel reviewModel)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "B";
                SqlCommand cmd = new SqlCommand("SP_REVIEW_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ID", reviewId);
                cmd.Parameters.AddWithValue("@WEEK_NO", reviewModel.WeekNo);
                cmd.Parameters.AddWithValue("@SHEDULE_DATE", reviewModel.ScheduledDate);
                cmd.Parameters.AddWithValue("@POSTPONED_DATE", reviewModel.PostponedDate);
                cmd.Parameters.AddWithValue("@TASK_MARKS", reviewModel.TaskMarks);
                cmd.Parameters.AddWithValue("@REVIEW_MODE", reviewModel.ReviewMode);
                cmd.Parameters.AddWithValue("@REVIEW_ID", reviewModel.Status);
                cmd.Parameters.AddWithValue("@STATUS_DESCRIPTION", reviewModel.StatusDescription);
                cmd.Parameters.AddWithValue("@STUDENT_ID", reviewModel.StudentId);
                cmd.Parameters.AddWithValue("@MENTOR_ID", reviewModel.MentorId);
                cmd.Parameters.AddWithValue("@REVIEWER_ID", reviewModel.ReviewerId);
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return reviewModel;
            }
        }

        public async Task DeleteReview(int reviewId)
        {
            using (SqlConnection con = new SqlConnection(_connectionSetting))
            {
                string type = "";
                SqlCommand cmd = new SqlCommand("SP_REVIEW_ALL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Id", reviewId);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                return;
            }
        }


    }
}
