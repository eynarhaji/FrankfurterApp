using System.ComponentModel.DataAnnotations;

namespace FrankfurterApp.Authentication;

public class AuthenticationSettings
{
    [Required]
    public string Issuer { get; set; }
    [Required]
    public string Audience { get; set; }
    [Required]
    public string Secret { get; set; }
    [Required]
    public int TokenExpirationInMinutes { get; set; }
}