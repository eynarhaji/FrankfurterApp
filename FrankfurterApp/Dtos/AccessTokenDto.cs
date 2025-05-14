using System;

namespace FrankfurterApp.Dtos;

public class AccessTokenDto
{
    public string AccessToken { get; set; }
    public DateTime ExpiresAt { get; set; }
}