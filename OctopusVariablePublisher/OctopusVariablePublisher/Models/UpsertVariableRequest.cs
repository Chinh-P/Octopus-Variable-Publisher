using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace OctopusProjectVariables.Models;

public class UpsertVariableRequest
{
    [Required]
    public string OctopusUrl { get; set; }
    [Required]
    public string OctopusApiKey { get; set; }
    [Required]
    public string SpaceName { get; set; }
    [Required]
    public string ProjectName { get; set; }
    public string ProjectPrefix { get; set; }
    [Required]
    public JObject ConfigData { get; set; }
}