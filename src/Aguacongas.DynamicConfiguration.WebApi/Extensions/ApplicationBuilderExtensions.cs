// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Aguacongas.DynamicConfiguration.Abstractions;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Defines extensions for <see cref="IApplicationBuilder"/>
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Auto reload configuration changes.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder SubscribeToConfigurationChanges(this IApplicationBuilder app)
        {
            var autoReloadService = app.ApplicationServices.GetRequiredService<IAutoReloadConfigurationService>();
            autoReloadService.SubscribeToChanges();
            return app;
        }
    }
}
