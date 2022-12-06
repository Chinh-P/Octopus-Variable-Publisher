// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json.Linq;
using OctopusProjectVariables.Models;
using OctopusProjectVariables.Services;

Console.WriteLine("Hello, World!");
var service = new OctopusVariableServices();
 
var requestObject = new UpsertVariableRequest()
{
    OctopusUrl = "-YOUR OctopusUrl-",
    OctopusApiKey = "-REPLACE-WITH-YOUR-KEY-",
    SpaceName = "Default",
    ProjectName = "Your Project name", 
    ProjectPrefix = "Your project predictor", 
    ConfigData =  JObject.Parse(File.ReadAllText("./Config.json")) // Replace with your Json file, like appsetting.json
};

await service.UpsertVariable(requestObject);
Console.WriteLine("Finish updating");
Console.ReadLine();