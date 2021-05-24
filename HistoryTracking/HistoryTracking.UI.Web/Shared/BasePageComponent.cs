using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HistoryTracking.UI.Web.Shared
{
    public class BasePageComponent<TPage> : ComponentBase where TPage : BasePageModel, new()
    {
        [Inject] protected IJSRuntime Script { get; set; }

        [Inject] protected NavigationManager Navigation { get; set; }

        [Inject] protected ServerApiCall ApiCall { get; set; }

        protected TPage Model { get; } = new TPage();

        protected void ShowError(string errorMessage)
        {
            Model.ErrorMessage = errorMessage;
        }

        protected void ShowInfo(string message)
        {
            Model.LogMessage = message;
        }
    }
}
