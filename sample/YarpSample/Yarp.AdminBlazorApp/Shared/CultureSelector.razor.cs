// Project: Aguafrommars/DynamicConfiguration
// Copyright (c) 2021 @Olivier Lefebvre

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;

namespace Yarp.AdminBlazorApp.Shared
{
    public partial class CultureSelector
    {
        [Inject]
        private IJSRuntime? JSRuntime { get; set; }

        [Inject]
        private NavigationManager? NavigationManager { get; set; }

        private CultureInfo[] supportedCultures = new[]
        {
            new CultureInfo("en-US"),
            new CultureInfo("fr-FR")        };

        private CultureInfo Culture
        {
            get => CultureInfo.CurrentCulture;
            set
            {
                if (CultureInfo.CurrentCulture != value)
                {
                    var js = JSRuntime as IJSInProcessRuntime;
                    js?.InvokeVoid("blazorCulture.set", value.Name);

                    NavigationManager?.NavigateTo(NavigationManager.Uri, forceLoad: true);
                }
            }
        }
    }
}
