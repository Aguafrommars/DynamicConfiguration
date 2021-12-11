using Aguacongas.Configuration.Razor;
using System.Reflection;

namespace Aguacongas.TheReverseProxy.BlazorApp
{
    public partial class App
    {
        private IEnumerable<Assembly> AdditionalAssemblies => new Assembly[] { typeof(Settings).Assembly };
    }
}
