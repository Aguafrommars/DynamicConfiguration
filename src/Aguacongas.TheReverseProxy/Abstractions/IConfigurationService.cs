using Aguacongas.TheReverseProxy.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aguacongas.TheReverseProxy.Abstractions
{
    public interface IConfigurationService
    {
        Task<Page<IDictionary<string, object>>> GetClusterConfigAsync(Request request);
        Task SetClusterAsync(string key, ClusterConfig cluster);
    }
}
