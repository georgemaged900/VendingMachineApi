namespace FlapKap.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public int Deposit { get; set; }
    }

}
