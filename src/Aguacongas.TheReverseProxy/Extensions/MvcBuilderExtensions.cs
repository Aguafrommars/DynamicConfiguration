using Aguacongas.TheReverseProxy.AdminApi.Controllers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddTheReverseProxyAdmin(this IMvcBuilder builder)
        {
            builder.Services.AddTheReverseProxy();
            return builder.AddApplicationPart(typeof(ProxyConfigController).Assembly);
        }
    }
}
