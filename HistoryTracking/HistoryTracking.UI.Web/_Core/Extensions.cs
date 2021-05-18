#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace HistoryTracking.UI.Web
{
    public static class Extensions
    {
        public static ValueTask<string> ExecuteAsync(this IJSRuntime script, string jsFunctionName, params object?[]? scriptAndParams)
        {
            return script.InvokeAsync<string>(jsFunctionName, scriptAndParams);
        }
    }
}
