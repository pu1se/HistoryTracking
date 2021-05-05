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
    public abstract class BaseTest
    {
        protected DataContext Storage { get; private set; }

        [TestInitialize]
        public void BaseInitialize()
        {
            DependencyManager.RegisterComponents();
            Storage = DependencyManager.Resolve<DataContext>();
        }

        [TestCleanup]
        public void BaseCleanup()
        {
            Storage.Dispose();
        }

        public void CleanStorageCache()
        {
            Storage.Dispose();
            Storage = DependencyManager.Resolve<DataContext>();
        }
    }
}
