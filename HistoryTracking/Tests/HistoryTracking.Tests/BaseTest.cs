using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HistoryTracking.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoryTracking.Tests
{
    public abstract class BaseTest
    {
        private static bool _isFirstCall = true;
        protected DataContext Storage { get; private set; }

        [TestInitialize]
        public void BaseInitialize()
        {
            if (_isFirstCall)
            {
                Storage = new DataContext();
                DatabaseInitializer.SeedWithTestData(Storage);
                _isFirstCall = false;
            }
        }
    }
}
