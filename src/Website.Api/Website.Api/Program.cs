using Website.Api.Options;
using Website.Api.Services.ServiceBuilders;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add log
builder.Host.UseSwaggerSerilogBuilder();

// Add services to the container.
builder.Services.AddOptions<VnPayOptions>()
    .Bind(builder.Configuration
    .GetSection(VnPayOptions.Position))
    .ValidateDataAnnotations();

builder.Services.UseSwaggerServiceBuilder(configuration);
builder.Services.UseInjectionServiceBuilder(configuration);
builder.Services.UseAuthServiceBuilder(configuration);
builder.Services.UseWebServiceBuilder(configuration);

var app = builder.Build();
app.UseSwaggerApplicationBuilder(configuration);

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
