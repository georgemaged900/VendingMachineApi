using FlapKap.Models;

namespace FlapKapBackendChallenge.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Deposit { get; set; } = 0;
        public string Role { get; set; } = "buyer"; // buyer or seller

        public ICollection<UserRole> UserRoles { get; set; }

    }
}
