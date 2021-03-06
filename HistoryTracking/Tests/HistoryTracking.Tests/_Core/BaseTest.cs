﻿using System;
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
        private static bool _isFirstCall = true;
        protected DataContext Storage { get; private set; }
        protected DateTime DateWhenProjectWasStarted = new DateTime(2021, 05, 10);

        [TestInitialize]
        public void BaseInitialize()
        {
            DependencyManager.RegisterComponents();
            Storage = DependencyManager.Resolve<DataContext>();

            if (_isFirstCall)
            {
                DatabaseInitializer.SeedWithTestData(Storage);
                _isFirstCall = false;
            }
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
