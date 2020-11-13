using System.Text.Json.Serialization;

namespace VirtualWallet.Application.DTOs.Account
{
    public class AuthenticationResponse
    {
        public string JWToken { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}
