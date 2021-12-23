// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Aguacongas.DynamicConfiguration.Abstractions;
using Aguacongas.DynamicConfiguration.Formatters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Aguacongas.DynamicConfiguration.WebApi.Controllers
{
    /// <summary>
    /// Configuration controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ConfigurationController : ControllerBase
    {
        /// <summary>
        /// Defines the dynamic configuration reader authorization policy
        /// </summary>
        public const string DYNAMIC_CONFIGURATION_READER_POLICY = "DynamicConfigurationReaderPolicy";
        /// <summary>
        /// Defines the dynamic configuration writter authorization policy
        /// </summary>
        public const string DYNAMIC_CONFIGURATION_WRITTER_POLICY = "DynamicConfigurationWritterPolicy";

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
        /// <param name="typeName">The configuration's assembly-qualified type name</param>
        /// <param name="key">The configuration's key</param>
        /// <returns>The configuration</returns>
        [HttpGet("{typeName}")]
        [HttpGet("{typeName}/{key}")]
        [Authorize(Policy = DYNAMIC_CONFIGURATION_READER_POLICY)]
        public Task<object> Get(string typeName, string? key)
        => _service.GetAsync(typeName, key);

        // PUT api/<Configuration>
        /// <summary>
        /// Sets a configuration
        /// </summary>
        /// <param name="key">The configuration's key</param>
        /// <param name="json">The JSON serialized value</param>
        /// <returns></returns>
        [HttpPut("{key}")]
        [Consumes(RawRequestBodyFormatter.CONTENTTYPE)]
        [Authorize(Policy = DYNAMIC_CONFIGURATION_WRITTER_POLICY)]
        public Task Put(string key, [FromBody] string json)
        => _service.SetAsync(key, json);
    }
}
