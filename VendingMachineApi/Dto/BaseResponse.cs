namespace FlapKapBackendChallenge.Dto
{
    public class BaseResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Operation Succeeded";
        public object Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
