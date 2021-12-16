using Aguacongas.DynamicConfiguration.Razor;
using System.Reflection;

namespace Yarp.AdminBlazorApp
{
    public partial class App
    {
        private static IEnumerable<Assembly> AdditionalAssemblies => new Assembly[] { typeof(Settings).Assembly };
    }
}
