namespace OctopusProjectVariables.Models;

public class OctopusVariable
{
    public string Name { get; set; }
    public string Value { get; set; }
    public string Type { get; set; }
    public bool IsSensitive { get; set; }
    
    //-- future improvement
    public string Scope { get; set; }
}
