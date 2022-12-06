using Microsoft.AspNetCore.Mvc;
using OctopusProjectVariables.Models;
using OctopusVariablePublisher.Services;

namespace OctopusVariablePublisher.Controllers;
[ApiController]
[Route("[controller]")]
public class OctopusPublisherController : Controller
{
    private readonly IOctopusVariableServices _octopusService;

    public OctopusPublisherController(IOctopusVariableServices octopusService)
    {
        _octopusService = octopusService;
    }
    
    [HttpPost]
    public async Task<IActionResult> PostVariables(UpsertVariableRequest requestData)
    {
        try
        {
            await _octopusService.UpsertVariable(requestData);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        
    }
}