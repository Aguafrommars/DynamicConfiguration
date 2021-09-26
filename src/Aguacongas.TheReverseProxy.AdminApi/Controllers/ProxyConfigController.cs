// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using Aguacongas.Configuration.Redis;
using Aguacongas.TheReverseProxy.AdminApi.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Yarp.ReverseProxy.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Aguacongas.TheReverseProxy.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProxyConfigController : ControllerBase
    {
        private readonly IConfigurationRoot _rootConfiguration;
        private readonly IConfigurationProvider _provider;
        private readonly IProxyConfigProvider _proxyConfigProvider;

        public ProxyConfigController(IConfigurationRoot rootConfiguration)
        {
            _rootConfiguration = rootConfiguration ?? throw new ArgumentNullException(nameof(rootConfiguration));
            _provider = rootConfiguration.Providers.First();
        }

        // GET: api/<Configuration>
        [HttpGet]
        public ProxyConfig Get()
        => _rootConfiguration.GetSection("ReverseProxy").Get<ProxyConfig>();

        // PUT api/<Configuration>
        [HttpPut()]
        public void Put([FromBody] ProxyConfig value)
        {
            _provider.Set("ReverseProxy", JsonSerializer.Serialize(value));
        }
    }
}
