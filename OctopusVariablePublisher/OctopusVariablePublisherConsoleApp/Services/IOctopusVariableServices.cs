using Newtonsoft.Json.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectVariables.Models;

namespace OctopusVariablePublisher.Services;

public interface IOctopusVariableServices
{
    Task UpsertVariable(UpsertVariableRequest request);

    Task<List<OctopusVariable>> CreateOctopusVariables(JObject data, string prefix);

    Task ExecuteVariableUpdate(List<OctopusVariable> data,IOctopusSpaceRepository repositoryForSpace, VariableSetResource projectVariables);
}