using Aguacongas.Configuration.Razor.Options;
using Aguacongas.Configuration.Razor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aguacongas.Configuration.Razor
{
    public partial class Settings
    {
        [Parameter]
        public string? Path { get; set; }

        [Inject]
        private IConfigurationService Service { get; set; }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();

        }
    }
}
