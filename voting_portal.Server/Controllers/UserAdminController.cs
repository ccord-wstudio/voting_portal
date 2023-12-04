using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;


namespace voting_portal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAdminController : Controller
    {
        [HttpGet]
        public JsonResult GetUsers()
        {
            List<userModel> users = new List<userModel>();

            string connectionString = "Data Source=.;Initial Catalog=voting_portal;Integrated Security=True;Encrypt=False";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Users";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userModel user = new userModel
                            {
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                DOB = reader.GetDateTime(reader.GetOrdinal("DOB")),
                                AddressLongitude = reader.GetDecimal(reader.GetOrdinal("AddressLongitude")),
                                AddressLatitude = reader.GetDecimal(reader.GetOrdinal("AddressLatitude")),
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                Country = reader.GetString(reader.GetOrdinal("Country"))
                            };
                            users.Add(user);
                        }
                    }
                }
            }

            return Json(users);
        }
        [HttpPost("update")]
        public IActionResult UpdateUser([FromBody] userModel updatedUser)
        {
            Console.WriteLine($"Received user data: {JsonConvert.SerializeObject(updatedUser)}");
            try
            {
                string connectionString = "Data Source=.;Initial Catalog=voting_portal;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE Users SET email = @Email, firstName = @FirstName, lastName = @LastName, " +
                                         "password = @Password, dob = @DOB, addressLongitude = @AddressLongitude, " +
                                         "addressLatitude = @AddressLatitude, country = @Country WHERE userID = @UserID";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@email", updatedUser.Email);
                        command.Parameters.AddWithValue("@firstName", updatedUser.FirstName);
                        command.Parameters.AddWithValue("@lastName", updatedUser.LastName);
                        command.Parameters.AddWithValue("@password", updatedUser.Password);
                        command.Parameters.AddWithValue("@dob", updatedUser.DOB);
                        command.Parameters.AddWithValue("@addressLongitude", updatedUser.AddressLongitude);
                        command.Parameters.AddWithValue("@addressLatitude", updatedUser.AddressLatitude);
                        command.Parameters.AddWithValue("@userID", updatedUser.UserID);
                        command.Parameters.AddWithValue("@country", updatedUser.Country);

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
                Console.WriteLine($"Error updating user: {ex.Message}");
                return BadRequest(ex.Message); // Handle update failure
            }
        }
        [HttpDelete("udelete/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            try
            {
                string connectionString = "Data Source=.;Initial Catalog=voting_portal;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM Users WHERE userID = @UserID";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@userID", userId);

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
                Console.WriteLine($"Error deleting user: {ex.Message}");
                return BadRequest(ex.Message); // Handle delete failure
            }
        }
        [HttpDelete("vdelete/{voteID}")]
        public IActionResult DeleteVote(int voteID)
        {
            try
            {
                string connectionString = "Data Source=.;Initial Catalog=voting_portal;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM Votes WHERE voteID = @VoteID";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@voteID", voteID);

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
                Console.WriteLine($"Error deleting vote: {ex.Message}");
                return BadRequest(ex.Message); // Handle delete failure
            }
        }
    }

}
