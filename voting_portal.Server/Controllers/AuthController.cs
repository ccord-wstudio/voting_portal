using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;

namespace voting_portal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] loginModel model)
        {
            // Validate user against the SQL database
            (bool isValidUser, int userId) = IsValidUser(model.Email, model.Password);

            if (isValidUser)
            {
                return Ok(new { message = "Login successful!", userId });
            }
            else
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] registrationModel model)
        {
            // Validate and register the user in the SQL database
            (bool isRegistrationSuccessful, int userId, string errorMessage) = RegisterUser(model);

            if (isRegistrationSuccessful)
            {
                return Ok(new { message = "Registration successful!", userId });
            }
            else
            {
                return BadRequest(new { message = errorMessage });
            }
        }


        public (bool isValidUser, int userId) IsValidUser(string email, string password)
        {
            string connectionString = "Data Source=.;Initial Catalog=voting_portal;Integrated Security=True;Encrypt=False";

            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                string query = "SELECT UserId FROM Users WHERE Email = @Email AND Password = @Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    var userId = command.ExecuteScalar();

                    // Check for null before casting
                    if (userId != null && userId != DBNull.Value)
                    {
                        return (true, (int)userId);
                    }
                    else
                    {
                        return (false, 0); // Or any other default value for userId
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception using a logging framework
                Console.WriteLine(ex.Message);
                throw; // Rethrow the exception for further handling if needed
            }
        }

        private static bool IsValidEmail(string email)
        {
            // The regular expression pattern for a simple email validation
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }
        private bool IsEmailAlreadyRegistered(string email)
        {
            string connectionString = "Data Source=.;Initial Catalog=voting_portal;Integrated Security=True;Encrypt=False";

            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";

                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                int count = (int)command.ExecuteScalar();

                // If count is greater than 0, the email is already registered
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw; // Rethrow the exception for further handling if needed
            }
        }

        private bool IsValidPassword(string password)
        {
            // Password must contain a minimum of 8 characters and at least 1 number
            return password.Length >= 8 && password.Any(char.IsDigit);
        }

        public (bool isRegistrationSuccessful, int userId, string errorMessage) RegisterUser(registrationModel model)
        {
            // Email format validation
            if (!IsValidEmail(model.Email))
            {
                return (false, 0, "Invalid Email Format");
            }
            // Check if the email address is already registered
            if (IsEmailAlreadyRegistered(model.Email))
            {
                return (false, 0, "Email already registered");
            }
            // Validate the FirstName and LastName properties
            if (string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName))
            {
                // Return an error message indicating invalid first name or last name
                return (false, 0, "Invalid First Name or Last Name");
            }
            // Validate the DateOfBirth property
            if (model.DOB == default(DateTime))
            {
                // Handle the case where DateOfBirth is not set correctly
                return (false, 0, "Invalid Date of Birth");
            }

            // Password validation
            if (!IsValidPassword(model.Password))
            {
                return (false, 0, "Password is invalid. Must contain a minimum of 8 characters and at least 1 number");
            }

            string connectionString = "Data Source=.;Initial Catalog=voting_portal;Integrated Security=True;Encrypt=False";

            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                // Get location details based on the postcode
                var locationDetails = GetLocationDetails(model.Postcode);

                // Check if locationDetails is null (indicating an incorrect postcode)
                if (locationDetails == null)
                {
                    // Return an error message indicating incorrect postcode
                    return (false, 0, "Incorrect Postcode");
                }

                string formattedDateOfBirth = model.DOB.ToString("yyyyMMdd"); // Convert to YYYYMMDD format

                string query = "INSERT INTO Users (Email, Password, FirstName, LastName, DOB, AddressLongitude, AddressLatitude, Country) " +
                                "VALUES (@Email, @Password, @FirstName, @LastName, @DateOfBirth, @AddressLongitude, @AddressLatitude, @Country);" +
                                "SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@Password", model.Password);
                    command.Parameters.AddWithValue("@FirstName", model.FirstName);
                    command.Parameters.AddWithValue("@LastName", model.LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", formattedDateOfBirth); // Use the formatted date
                    command.Parameters.AddWithValue("@AddressLongitude", locationDetails.longitude);
                    command.Parameters.AddWithValue("@AddressLatitude", locationDetails.latitude);
                    command.Parameters.AddWithValue("@Country", locationDetails.country);

                    var newUserId = command.ExecuteScalar();

                    if (newUserId != null && newUserId != DBNull.Value)
                    {
                        return (true, Convert.ToInt32(newUserId), string.Empty);
                    }
                    else
                    {
                        return (false, 0, "Registration failed. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw; // Rethrow the exception for further handling if needed
            }
        }





        private LocationDetails GetLocationDetails(string postcode)
        {
            string apiUrl = $"https://api.postcodes.io/postcodes/{postcode.Replace(" ", "")}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = client.GetStringAsync(apiUrl).Result; // Synchronously wait for the response

                    // Deserialize the response to a class representing the JSON structure
                    var result = JsonConvert.DeserializeObject<LocationApiResponse>(response);

                    // Create a class to represent the expected JSON structure
                    return new LocationDetails
                    {
                        longitude = result.result.longitude,
                        latitude = result.result.latitude,
                        country = result.result.country
                    };
                }
                catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    // Handle 404 error (Not Found)
                    // Display an alert message on the webpage
                    Console.WriteLine("Incorrect Postcode");
                    return null; // Or handle it as needed in your application
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    Console.WriteLine($"Error: {ex.Message}");
                    return null; // Or handle it as needed in your application
                }
            }
        }




    }
}
