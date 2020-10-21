using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.BL;

namespace HistoryTracking.Tests
{
    public class BaseServiceTests<TService> : BaseTest where TService : BaseService
    {
        protected TService Service { get; }

        public BaseServiceTests()
        {
            Service = DependencyManager.Resolve<TService>();
        }
    }
}
