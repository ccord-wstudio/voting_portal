using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace voting_portal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        [HttpPost]
        public IActionResult SubmitVote([FromBody] voteModel vote)
        {
            try
            {
                int userId = vote.UserID;
                int questionId = vote.QuestionID;

                if (HasUserVoted(userId, questionId))
                {
                    return BadRequest(new { message = "User has already voted for this question." });
                }

                InsertVoteIntoDatabase(vote);

                return Ok(new { message = "Vote submitted successfully!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "Internal server error." });
            }
        }

        private bool HasUserVoted(int userId, int questionId)
        {
            string connectionString = "Data Source=.;Initial Catalog=voting_portal;Integrated Security=True;Encrypt=False";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM Votes WHERE UserId = @UserId AND QuestionId = @QuestionId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@QuestionId", questionId);

                        int voteCount = (int)command.ExecuteScalar();

                        return voteCount > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        private void InsertVoteIntoDatabase(voteModel vote)
        {
            string connectionString = "Data Source=.;Initial Catalog=voting_portal;Integrated Security=True;Encrypt=False";

            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                string query = "INSERT INTO Votes (UserID, QuestionID, Response) VALUES (@UserID, @QuestionID, @Response)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Ensure vote.UserID is not null
                    int? userId = vote.UserID; // Use int? for nullable int
                    command.Parameters.AddWithValue("@UserID", userId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@QuestionID", vote.QuestionID);
                    command.Parameters.AddWithValue("@Response", vote.Response);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw; // Rethrow the exception for further handling if needed
            }
        }

    }
}

