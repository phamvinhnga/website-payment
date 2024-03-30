using Serilog;
using Serilog.Formatting.Elasticsearch;
using Serilog.Formatting.Json;

namespace Website.Api.Services.ServiceBuilders
{
    public static class SerilogServiceBuilder
    {
        public static void UseSwaggerSerilogBuilder(this ConfigureHostBuilder host)
        {
            host.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .Enrich.FromLogContext()
                    .WriteTo.File(new JsonFormatter(), @"logs\log-.txt", rollingInterval: RollingInterval.Day)
                    .WriteTo.Console();
                    //.WriteTo.Console(new ElasticsearchJsonFormatter(renderMessageTemplate: true));
            });
        }
    }
}
