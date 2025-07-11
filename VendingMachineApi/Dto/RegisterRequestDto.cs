using System.ComponentModel.DataAnnotations;

namespace FlapKapBackendChallenge.Dto
{
    public class RegisterRequestDto
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string role { get; set; } = "buyer";
    }
}
