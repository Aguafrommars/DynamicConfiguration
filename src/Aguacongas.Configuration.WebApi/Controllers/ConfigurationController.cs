// Project: Aguafrommars/TheIdServer
// Copyright (c) 2021 @Olivier Lefebvre
using Aguacongas.Configuration.Abstractions;
using Aguacongas.TheReverseProxy.Formatters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Aguacongas.Configuration.WebApi.Controllers
{
    /// <summary>
    /// Configuration controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _service;

        /// <summary>
        /// Initialize a new instance of <see cref="ConfigurationController"/>
        /// </summary>
        /// <param name="service">A <see cref="IConfigurationService"/> instance</param>
        /// <exception cref="ArgumentNullException">If service is null</exception>
        public ConfigurationController(IConfigurationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        // GET: api/<Configuration>
        /// <summary>
        /// Gets a configuration
        /// </summary>
        /// <param name="key">The configuration's key</param>
        /// <param name="typeName">The configuration's assembly-qualified type name</param>
        /// <returns>The configuration</returns>
        [HttpGet("{key}/{typeName}")]
        public Task<object> Get(string key, string typeName)
        => _service.GetAsync(key, typeName);

        // PUT api/<Configuration>
        /// <summary>
        /// Sets a configuration
        /// </summary>
        /// <param name="key">The configuration's key</param>
        /// <param name="json">The JSON serialized value</param>
        /// <returns></returns>
        [HttpPut("{key}")]
        [Consumes(RawRequestBodyFormatter.CONTENTTYPE)]
        public Task PutCluster(string key, [FromBody] string json)
        => _service.SetAsync(key, json);
    }
}
