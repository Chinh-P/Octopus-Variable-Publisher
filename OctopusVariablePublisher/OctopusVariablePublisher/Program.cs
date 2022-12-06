using OctopusProjectVariables.Services;
using OctopusVariablePublisher.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();;
builder.Services.AddSingleton<IOctopusVariableServices, OctopusVariableServices>();

var app = builder.Build();

app.MapControllers();

app.Run();