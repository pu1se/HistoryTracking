using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.BL;
using HistoryTracking.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoryTracking.Tests
{
    public class BaseServiceTests<TService> : BaseTest where TService : BaseService
    {
        protected TService Service { get; private set; }

        [TestInitialize]
        public void BaseServiceTestInitialize()
        {
            DependencyManager.RegisterComponents();
            Service = DependencyManager.Resolve<TService>();
        }

        [TestCleanup]
        public void BaseServiceTestCleanup()
        {
            Service.Dispose();
        }
    }
}
