using Aguacongas.TheReverseProxy.Abstractions;
using Community.OData.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Yarp.ReverseProxy.Configuration;

namespace Aguacongas.TheReverseProxy.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRoot _rootConfiguration;
        private readonly IConfigurationProvider _provider;
        private readonly IOptions<Model.ReverseProxyOptions> _options;

        public ConfigurationService(IConfigurationRoot rootConfiguration, IOptions<Model.ReverseProxyOptions> options)
        {
            _rootConfiguration = rootConfiguration ?? throw new ArgumentNullException(nameof(rootConfiguration));
            _provider = rootConfiguration.Providers.First();
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public Task<Model.Page<IDictionary<string, object>>> GetClusterConfigAsync(Model.Request request)
        {
            var config = _rootConfiguration.GetSection(_options.Value.ReverserProxySection).Get<Model.ProxyConfig>();

            var clusters = config.Clusters.Select(kv => new ClusterConfig
            {
                ClusterId = kv.Key,
                Destinations = kv.Value.Destinations == null ? null : new ReadOnlyDictionary<string, DestinationConfig>(kv.Value.Destinations),
                HealthCheck = kv.Value.HealthCheck,
                HttpClient = kv.Value.HttpClient,
                HttpRequest = kv.Value.HttpRequest,
                LoadBalancingPolicy = kv.Value.LoadBalancingPolicy,
                Metadata = kv.Value.Metadata == null ? null : new ReadOnlyDictionary<string, string>(kv.Value.Metadata),
                SessionAffinity = kv.Value.SessionAffinity
            }).AsQueryable()
                .OData(edmModel: GetClusterConfigEdmModel());
            
            if (request.Filter != null)
            {
                clusters = clusters.Filter(request.Filter);
            }

            if (request.Sort != null)
            {
                clusters = clusters.OrderBy(request.Sort);
            }

            var count = clusters.Count();

            clusters = clusters.TopSkip(request.Top, request.Skip);

            return Task.FromResult(new Model.Page<IDictionary<string, object>>
            {
                Count = count,
                Items = clusters.SelectExpand(request.Select ?? "*", request.Expand ?? string.Empty)
                    .Select(w => new Dictionary<string, object>(w.ToDictionary()
                        .Where(kv => kv.Value != null)
                        .Select(kv => new KeyValuePair<string, object>($"{char.ToLowerInvariant(kv.Key[0])}{kv.Key[1..]}", kv.Value))))
            });
        }

        public Task SetClusterAsync(string key, Model.ClusterConfig cluster)
        {
            _provider.Set($"{_options.Value.ReverserProxySection}:Clusters:{key}", JsonSerializer.Serialize(cluster));
            return Task.CompletedTask;
        }

        private static IEdmModel GetClusterConfigEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            var entitySet = builder.EntitySet<ClusterConfig>(typeof(ClusterConfig).Name);
            var entityType = entitySet.EntityType;
            entityType.HasKey(c => c.ClusterId);
            return builder.GetEdmModel();
        }
    }
}
