using FlapKapBackendChallenge.Models;

namespace FlapKap.Models
{
    public class UserRole
    {
        public int userId { get; set; }
        public int roleId { get; set; }

        public User User { get; set; }
        public Role Role { get; set; }
    }
}
