namespace Website.Api.Services.ServiceBuilders
{
    internal static class InjectionServiceBuilder
    {
        internal static void UseInjectionServiceBuilder(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IVnPayService, VnPayService>();
        }
    }
}
