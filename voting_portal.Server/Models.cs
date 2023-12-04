namespace voting_portal.Server
{
    public class loginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class userModel
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime DOB { get; set; }
        public decimal AddressLongitude { get; set; }
        public decimal AddressLatitude { get; set; }
        public string Country { get; set; }
    }

    public class voteModel
    {
        public int VoteID { get; set; }
        public int UserID { get; set; }
        public int QuestionID { get; set; }
        public string Response { get; set; }
    }

    public class registrationModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Postcode { get; set; }
        public decimal AddressLongitude { get; set; }
        public decimal AddressLatitude { get; set; }
    }

    public class LocationApiResponse
    {
        public int status { get; set; }
        public LocationDetails result { get; set; }
    }

    public class LocationDetails
    {
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string country { get; set; }
    }



}
