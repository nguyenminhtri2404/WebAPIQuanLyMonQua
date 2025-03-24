namespace GiftManagement_Version2.Models
{
    public class UserRegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public IFormFile? Image { get; set; }
    }
}
