using System.ComponentModel.DataAnnotations;

namespace FrankfurterApp.Logging;

public class SerilogSettings
{
    [Required]
    public string RestrictionLevel { get; set; }
    public bool ForceElasticsearch { get; set; } = true;
    public string NodeUri { get; set; }
    public bool ForceAuthentication { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string IndexFormat { get; set; }
    public string NumberOfReplicas { get; set; }
}