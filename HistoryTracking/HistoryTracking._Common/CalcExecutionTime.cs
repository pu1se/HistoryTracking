using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryTracking
{
    public static class CalcExecutionTime
    {
        public static TimeSpan For(Action func)
        {
            var startTime = DateTime.UtcNow;
            func();
            var executionTime = DateTime.UtcNow - startTime;
            return executionTime;
        }
    }
}
