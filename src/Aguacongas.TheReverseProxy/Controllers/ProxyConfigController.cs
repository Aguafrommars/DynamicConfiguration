// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using Aguacongas.TheReverseProxy.Abstractions;
using Aguacongas.TheReverseProxy.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Aguacongas.TheReverseProxy.AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProxyConfigController : ControllerBase
    {
        private readonly IConfigurationService _service;

        public ProxyConfigController(IConfigurationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET: api/<Configuration>
        [HttpGet("clusters")]
        public Task<Page<IDictionary<string, object>>> GetClusters([FromQuery] Request request)
        => _service.GetClusterConfigAsync(request);

        // PUT api/<Configuration>
        [HttpPut("{key}")]
        public Task PutCluster(string key, [FromBody] ClusterConfig cluster)
        => _service.SetClusterAsync(key, cluster);
    }
}
