using Microsoft.AspNetCore.Components;

namespace Yarp.AdminBlazorApp.Pages
{
    public partial class Index
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (NavigationManager is null)
            {
                throw new InvalidOperationException($"{nameof(NavigationManager)} cannot be null");
            }

            NavigationManager.NavigateTo("/settings", true);
        }

    }
}
