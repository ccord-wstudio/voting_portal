using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;


namespace voting_portal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteAdminController : Controller
    {
        [HttpGet]
        public JsonResult GetVotes()
        {
            List<voteModel> votes = new List<voteModel>();

            string connectionString = "Data Source=.;Initial Catalog=voting_portal;Integrated Security=True;Encrypt=False";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Votes";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            voteModel vote = new voteModel
                            {
                                VoteID = reader.GetInt32(reader.GetOrdinal("VoteID")),
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                QuestionID = reader.GetInt32(reader.GetOrdinal("QuestionID")),
                                Response = reader.GetString(reader.GetOrdinal("Response"))
                            };
                            votes.Add(vote);
                        }
                    }
                }
            }

            return Json(votes);
        }
        [HttpPost("update")]
        public IActionResult UpdateVotes([FromBody] voteModel updatedVote)
        {
            Console.WriteLine($"Received vote data: {JsonConvert.SerializeObject(updatedVote)}");
            try
            {
                string connectionString = "Data Source=.;Initial Catalog=voting_portal;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE Votes SET response = @Response WHERE voteID = @VoteID";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@voteID", updatedVote.VoteID);
                        command.Parameters.AddWithValue("@userID", updatedVote.UserID);
                        command.Parameters.AddWithValue("@questionID", updatedVote.QuestionID);
                        command.Parameters.AddWithValue("@response", updatedVote.Response);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok();
                        }
                        else
                        {
                            return NotFound(); // Handle not found scenario
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating vote: {ex.Message}");
                return BadRequest(ex.Message); // Handle update failure
            }
        }
    }

}
