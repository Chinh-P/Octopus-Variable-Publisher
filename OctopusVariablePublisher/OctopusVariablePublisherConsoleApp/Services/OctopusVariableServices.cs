using Newtonsoft.Json.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectVariables.Models;
using OctopusVariablePublisher.Services;

namespace OctopusProjectVariables.Services;

public class OctopusVariableServices:IOctopusVariableServices
{
    public async Task UpsertVariable(UpsertVariableRequest requestData)
    {
        var variables = await CreateOctopusVariables(requestData.ConfigData, requestData.ProjectPrefix);

        var (repositoryForSpace, projectVariables) = ConstructOctopusResource(requestData);
        await this.ExecuteVariableUpdate(variables, repositoryForSpace, projectVariables);

    }

    public Task<List<OctopusVariable>> CreateOctopusVariables(JObject data, string prefix)
    {
        List<OctopusVariable> variables = new List<OctopusVariable>();
     
        foreach (var a in data.DescendantsAndSelf())
        {
            if (a is JObject obj)
                foreach (var prop in obj.Properties())
                {
                    if (!(prop.Value is JObject) && !(prop.Value is JArray))
                    {
                        variables.Add( new OctopusVariable()
                        {
                            Name = prefix + "_"+ prop.Path.Replace('.',':'),
                            Value = prop.Value.ToString(),
                            IsSensitive = false,
                            Type = "String"
                        });
                    }

                }
        }

        return Task.FromResult(variables) ;
    }

    private (IOctopusSpaceRepository, VariableSetResource) ConstructOctopusResource(UpsertVariableRequest requestData)
    {
        // Create repository object
        var endpoint = new OctopusServerEndpoint(requestData.OctopusUrl, requestData.OctopusApiKey);
        var repository = new OctopusRepository(endpoint);
        var client = new OctopusClient(endpoint);

 
        // Get space
        var space = repository.Spaces.FindByName(requestData.SpaceName);
        var repositoryForSpace = client.ForSpace(space);

        // Get project
        var project = repositoryForSpace.Projects.FindByName(requestData.ProjectName);

        // Get project variables
        var projectVariables = repositoryForSpace.VariableSets.Get(project.VariableSetId);

        return (repositoryForSpace, projectVariables);
    }
    
    public Task ExecuteVariableUpdate(List<OctopusVariable> data,IOctopusSpaceRepository repositoryForSpace, VariableSetResource projectVariables)
    {
        foreach (var variable in data)
        {
            var variableToUpdate = projectVariables.Variables.FirstOrDefault(v => v.Name == (variable.Name));
            if (variableToUpdate == null)
            {
                // Create new variable object
                variableToUpdate = new Octopus.Client.Model.VariableResource();
                variableToUpdate.Name = variable.Name;
                variableToUpdate.Value = variable.Value.ToString();
                variableToUpdate.Type = (Octopus.Client.Model.VariableType)Enum.Parse(typeof(Octopus.Client.Model.VariableType), variable.Type);
                variableToUpdate.IsSensitive = variable.IsSensitive;

                // Add to collection
                projectVariables.Variables.Add(variableToUpdate);

                Console.WriteLine(variable.Name + ">>>" + variable.Value);
            }
            else
            {
                // Update value
                variableToUpdate.Value = variable.Value;
            }
        }
        // Check to see if variable exists


        // Update collection
        repositoryForSpace.VariableSets.Modify(projectVariables);
        return Task.CompletedTask;
        ;
    }
}