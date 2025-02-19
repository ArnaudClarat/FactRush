namespace FactRush.Models
{
    public class TokenResponse
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; } = "";
        public string Token { get; set; } = "";
    }
}
